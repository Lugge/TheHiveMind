using System;
using UnityEngine;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using B83.ExpressionParser;

/*
 * This class represents the actual AI. It evaluates its knowledge and decides the next steps based on his configuration.
 * 
 * @author: Lukas Krose
 * @version: 1.0
 */
namespace AntHill
{
	public class AntHillAI: AntHillAIInt
	{
		private AntHillMemory mem = new AntHillMemory();
		private GameUtils util = new GameUtils();

		//If this is set to true, the hill will display every single step in his decision process.
		private bool debug = false;
		private string debugTask = "";

		/*
		 * Constructor. Sets the initial configuration and the ant template.
		 * 
		 * @param: AntHillAIConf c The configuration for the hill
		 * @param: Rigidbody a The ant template
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public AntHillAI (AntHillAIConf c, Rigidbody a)
		{
			mem.setAntHillConfiguration (c);
			mem.ant = a;
		}

		/*
		 * Initialises the AI ands sets the Hill object.
		 * 
		 * @param: AntHill h The representation of the hill
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public void init (AntHill h){
			mem.hill = h;
		}

		/*
		 * Decides which food should be collected next (where the worker should be send to).
		 * This decision is based on the configuration in the AnthillAIConf.
		 * 
		 * @return: Food The food that should be collected next
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public Food decideFood(String providedFunction, string type){
			Food decided = null;
			double lastEval = -1000;

			foreach (Food food in mem.knownFood) {
				string func = providedFunction; 
				func = func.Replace ("metric", food.path.metric.ToString ());
				func = func.Replace ("searcherPerFood", food.sentAnts.ToString ());
				func = func.Replace ("optimizerPerFood", food.sentOptimizer.ToString ());
				double funcVal = evaluateFunction (func);

				if(type == "OptimizeTask"){
					if(food.path.metric <= mem.conf.minStepsToOpt || food.pathIsOptimal){
						funcVal = -1000;
					}
				}

				if(food.isEmpty){
					funcVal = funcVal - 10000;
				}
				if (lastEval <= funcVal) {
					decided = food;
					lastEval = funcVal;
				}
			}
			return decided;
		}

		/*
		 * Returns if the hill is able to make his next decision.
		 * It is based on the thinkingtime in the configuration.
		 * 
		 * @return: bool True if the hill can make his next decision
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public bool canMakeNextDecision(){
			if (mem.thinkDelay <= mem.conf.maxThinkTime) {
				mem.thinkDelay++;
				return false;
			}
			
			mem.thinkDelay = 0;
			return true;
		}

		/*
		 * Main method for the AI to make its next decision. Redirects to the specific implementations.
		 * 
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public void decide () {

			if (debugTask == "OptimizeTask") {
				debugOptimize ();
				return;
			}

			string decision = evaluateNextDecision ();

			typeof(AntHillAI).GetMethod (decision).Invoke (this, new object[]{});

		}

		/*
		 * Evaluates the decision based on the configuration and the knowledge the hill currently has.
		 * This method returns the decision of the hill.
		 * 
		 * @param: string The function the hill has decided to perform.
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		private string evaluateNextDecision(){
			System.Random rnd = new System.Random();
			string lastBestOption = "nothing";
			float optionVal = 0;
			Dictionary<string, float> funcCount = new Dictionary<string, float>();

			foreach (var evaluation in mem.evalFunc){
				foreach(var item in evaluation.Value ){
					string func = item.Value;
					if(mem.conf.impact[evaluation.Key].ContainsKey(item.Key)){
					   func = func.Replace ("impact", mem.conf.impact[evaluation.Key][item.Key]);
					}
					if(!funcCount.ContainsKey(item.Key)){
						funcCount.Add(item.Key, 0);
					}
					funcCount[item.Key]++;

					mem.currentEval[item.Key] = mem.currentEval[item.Key] + evaluateFunction(func) * rnd.Next(1, mem.conf.randomness);
				}
			}


			foreach(var eval in mem.currentEval){
				float finalVal = (float)eval.Value;
				if(mem.conf.useKOCriteria && evaluateKOCriteria(eval.Key)){
					finalVal = 0;
				}

				if(debug){
					Debug.Log(eval.Key + ":" + finalVal);
				}
				if(finalVal > optionVal){
					optionVal = finalVal;
					lastBestOption = eval.Key;
				}
			}

			if((mem.antCaps || mem.foodCaps) && mem.hill.getFoodCount() > 10000){
				lastBestOption = "increaseHillSize";
			}
			//Debug.Log (lastBestOption);
			mem.resetEval ();
			return lastBestOption;
		}

		/*
		 * This method is calls once for every entry in the configuration. 
		 * It evaluates the function in the string and returns the calculated value.
		 * 
		 * @return: double The value of the function
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		private float evaluateFunction (string func){
			func = func.Replace ("foodCount", mem.hill.getFoodCount().ToString());
			func = func.Replace ("knownFood", mem.activeFoodSources.ToString());
			func = func.Replace ("searcherAnts", mem.ants["Searcher"].Count.ToString());
			func = func.Replace ("workerAnts", mem.ants["Worker"].Count.ToString());
			func = func.Replace ("searcherInBase", mem.antsInBase["Searcher"].Count.ToString());
			func = func.Replace ("workerInBase", mem.antsInBase["Worker"].Count.ToString());

			
			return evaluateExpression (func);
		}

		private float evaluateExpression(string func) {
			var parser = new ExpressionParser();
			Expression exp = parser.EvaluateExpression(func);
			return (float)exp.Value;
		}

		private bool evaluateKOCriteria(string decision) {
			if (mem.hill.getFoodCount () < 0)
				return true;
			switch(decision){
			case "spawnSearcher":
				if (mem.hill.getFoodCount () < 1500)
					return true;
				if (mem.antCaps)
					return true;
				break;
			case "spawnWorker":
				if (mem.hill.getFoodCount () < 1000)
					return true;
				if (mem.activeFoodSources == 0)
					return true;
				if (mem.antCaps)
					return true;
				break;
			case "sendSearcherToSearch":
				if (mem.antsInBase["Searcher"].Count == 0)
					return true;
				break;
			case "sendSearcherToOptimize":
				if (mem.antsInBase["Searcher"].Count == 0)
					return true;
				bool isFoodLeft = false;
				foreach(Food f in mem.knownFood) {
					if(f.sentOptimizer == 0 && f.path.metric > mem.conf.minStepsToOpt && !f.pathIsOptimal){
						isFoodLeft = true;
					}
				}
				if(!isFoodLeft)return true;
				break;
			case "sendWorkerToCollect":
				if (mem.antsInBase["Worker"].Count == 0)
					return true;
				if (mem.activeFoodSources == 0)
					return true;
				break;
			}
			return false;
		}

		/*
		 * Handles the behaviour when the hill wants to communicate with the ant.
		 * Currently used to send an ant on its task.
		 * 
		 * @param: Ant ant The ant the hill wants to comminucate with
		 * @param: Task task The Task the ant should perfom
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public void communicate(Ant ant, Task task){
			mem.antsInBase [ant.getMemory().getType ()].Remove(ant);
			instructAnt (ant, task);

			int supplies = ant.getNeededSupplies ();
			mem.hill.updateFoodCount(- supplies);
			ant.supply (supplies);
			ant.setIdle (false);
		}

		/*
		 * This function is called when an ant has returned to the base.
		 * Sets the ant to idle and transfers all collected objects to the hill.
		 * 
		 * @param: Ant ant the ant that has returned
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public void antHasReachedBase(Ant ant){
			AntMemory antMem = ant.getMemory();
			if (antMem.foundFood != null) {
				if(ant.getTask() == "OptimizeTask"){
					mem.updateFoodList(antMem.foundFood, true);
				}else {
					mem.updateFoodList(antMem.foundFood, false);
				}
				
			}
			if (ant.carriedWeight > 0) {
				transferCollectedfromAnt(ant, antMem.typeOfCarriedObjects);
			}

			if (ant.getType () == "Worker" && ant.getTask() == "CollectTask") {
				mem.getFoodAtPos(ant.mvm.getInitialTarget()).sentAnts--;
			}
			if (ant.getType () == "Searcher" && ant.getTask() == "OptimizeTask") {
				mem.getFoodAtPos(ant.mvm.getInitialTarget()).sentOptimizer --;
			}
			ant.reset();
			mem.antsInBase [antMem.getType ()].Add(ant);
			ant.setIdle (true);
		}

		/*
		 * Instructs ant with a default task
		 * 
		 * @param: Ant ant The ant that should be instructed
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public void instructAnt(Ant ant){
			Task def = new DefaultTask();
			def.setTarget(mem.hill.position);
			instructAnt (ant, def);
		}

		/*
		 * Instructs an ant with a given task
		 * 
		 * @param: Ant ant The ant that needs instructions
		 * @param: Task task The task that should be assigned to the ant
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public void instructAnt(Ant ant, Task task){
			ant.instructAnt (mem.getCurrentInformation());
			ant.setTask(task);
		}

		/*
		 * Transfers the collected items from the ant to the hill
		 * 
		 * @param: Ant ant The respective ant
		 * @param: String type The type of the resource
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public void transferCollectedfromAnt(Ant ant, String type){
			if(type == "Food"){
				try{
				mem.hill.updateFoodCount (ant.carriedWeight);
				}catch (UnityException e){
					Debug.Log (e);
				}
			}
		}
		/*
		 * Function if the hill decides to do nothing.
		 * Does nothing.
		 * 
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public void nothing(){
			return;
		}

		/*
		 * Handles the case if the hill decided to spwan a searcher
		 * 
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public void spawnSearcher(){
			if (mem.getAntCount () + 1 >= mem.hill.maxAnts) {
				mem.antCaps = true;
				return;
			}
			mem.hill.updateFoodCount(- mem.hill.antCost);
			Ant searcher = util.spawnSearcher (mem.ant, mem.hill.position, mem.hill.rotation);
			mem.addAnt (searcher);
			mem.antsInBase ["Searcher"].Add(searcher);
		}

		/*
		 * Handles the case if the hill decided to spwan a worker
		 * 
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public void spawnWorker(){
			if (mem.getAntCount () + 1 >= mem.hill.maxAnts) {
				mem.antCaps = true;
				return;
			}
			mem.hill.updateFoodCount(- mem.hill.antCost);
			Ant worker = util.spawnWorker (mem.ant, mem.hill.position, mem.hill.rotation);
			mem.addAnt (worker);
			mem.antsInBase ["Worker"].Add(worker);
		}

		/*
		 * Handles the case if the hill decided to send a searcher to search
		 * 
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public void sendSearcherToSearch(){
			Task search = new SearchTask ();
			search.setTarget(mem.hill.position);
			sendAnt ("Searcher", search);
		}

		/*
		 * Handles the case if the hill decided to send a searcher to optimize
		 * 
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public void sendSearcherToOptimize(){
			Task optimize = new OptimizeTask ();
			Food decided = decideFood(mem.conf.foodToOptimizeFormula, "OptimizeTask");
			if (decided == null) {
				Debug.Log("No food to optimize");
				return;
			}
			optimize.setTarget (decided.foodObject.transform.position);
			optimize.setPath (decided.path);
			decided.sentOptimizer++;
			sendAnt ("Searcher", optimize);
		}

		/*
		 * Handles the case if the hill decided to send a worker to collect
		 * 
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public void sendWorkerToCollect(){
			Task collect = new CollectTask ();
			Food collectFood = decideFood(mem.conf.foodToCollectFormula, "CollectTask");
			if (collectFood == null) {
				Debug.Log("No food to collect");
				return;
			}
			//collect.targetFood = collectFood;
			collect.setTarget (collectFood.foodObject.transform.position);
			collect.setPath (collectFood.path);
			//Debug.Log (collect.getTarget());
			//collectFood.path.debugPath ();
			collectFood.sentAnts++;

			sendAnt ("Worker", collect);
		}

		/*
		 * Sends an ant on a given task
		 * 
		 * @param: Ant ant The respective ant
		 * @param: Task task The task for the ant
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public void sendAnt(string type, Task task){
			if (mem.antsInBase [type].Count > 0) {
				Ant antInBase = mem.antsInBase [type][0];
				setColor (antInBase, task.getType());
				communicate (antInBase, task);
			} else {
				Debug.Log("Could not send Ant. No ant in base.");
			}
		}
		
		
		
		
		public void setColor(Ant ant, string task) {
			Renderer antRenderer = ant.antRenderer;
			Debug.Log((Material)Resources.Load("Ant_work", typeof(Material)));
			switch (task) {
			case "CollectTask":
				
				antRenderer.sharedMaterial = (Material)Resources.Load("Ant_work", typeof(Material));
				break;
			case "SearchTask":
				antRenderer.sharedMaterial = (Material)Resources.Load("Ant_search", typeof(Material));
				break;
			case "OptimizeTask":
				antRenderer.sharedMaterial = (Material)Resources.Load("Ant_opt", typeof(Material));
				break;
				
			}
			
		}

		public void increaseHillSize() {
			mem.antCaps = false;
			mem.foodCaps = false;
			mem.hill.updateSize ();
		}

		private void debugOptimize() {
			if (mem.ants ["Searcher"].Count == 0) {
				spawnSearcher();
				return;
			}
			if (mem.knownFood.Count == 0) {
				sendSearcherToSearch();
				return;
			}
			sendSearcherToOptimize ();
		}

		public void setFoodCaps(bool caps){
			mem.foodCaps = caps;
		}

	}
}


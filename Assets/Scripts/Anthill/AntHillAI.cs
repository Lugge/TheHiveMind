using System;
using UnityEngine;
using System.Data;
using System.Collections;
using System.Collections.Generic;

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
		public Food decideFoodToCollect(){
			Food decided = null;
			double lastEval = -1000;

			foreach (Food food in mem.knownFood) {
				String func = mem.conf.foodToCollectFormula;
				func = func.Replace ("metric", food.path.metric.ToString ());
				func = func.Replace ("searcherPerFood", food.sentAnts.ToString ());
				double funcVal = evaluateFunction (func);
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
			string lastBestOption = "";
			double optionVal = 0;
			Dictionary<string, double> funcCount = new Dictionary<string, double>();

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
				double finalVal = eval.Value / funcCount[eval.Key];

				if(finalVal > optionVal){
					if(debug){
						Debug.Log(eval.Key + ":" + optionVal);
					}
					optionVal = finalVal;
					lastBestOption = eval.Key;
				}
			}

			Debug.Log (lastBestOption);
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
		private double evaluateFunction (string func){
			func = func.Replace ("foodCount", mem.hill.getFoodCount().ToString());
			func = func.Replace ("knownFood", mem.activeFoodSources.ToString());
			func = func.Replace ("searcherAnts", mem.ants["Searcher"].Count.ToString());
			func = func.Replace ("workerAnts", mem.ants["Worker"].Count.ToString());
			func = func.Replace ("searcherInBase", mem.antsInBase["Searcher"].Count.ToString());
			func = func.Replace ("workerInBase", mem.antsInBase["Worker"].Count.ToString());

			DataTable table = new DataTable();
			table.Columns.Add("expression", typeof(string), func);
			DataRow row = table.NewRow();
			table.Rows.Add(row);
			return double.Parse((string)row["expression"]);
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
			instructAnt (ant, task);
			mem.antsInBase [ant.getMemory().getType ()].Remove(ant);
			mem.hill.updateFoodCount(- 1);
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
				mem.updateFoodList(antMem.foundFood);
				
			}
			if (ant.carriedWeight > 0) {
				transferCollectedfromAnt(ant, antMem.typeOfCarriedObjects);
			}

			if (ant.getType () == "Worker" && ant.getTask() == "CollectTask") {
				mem.knownFood[mem.knownFood.IndexOf(ant.mvm.targetFood)].sentAnts--;

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
			def.target = mem.hill.position;
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
				mem.hill.updateFoodCount (ant.carriedWeight);
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
			mem.hill.updateFoodCount(- 10);
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
			mem.hill.updateFoodCount(- 10);
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
			search.target = mem.hill.position;
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
			optimize.targetFood = decideFoodToCollect();
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
			Food collectFood = decideFoodToCollect();
			if (collectFood == null) {
				Debug.Log("No food to collect");
				return;
			}
			collect.targetFood = collectFood;
			mem.knownFood[mem.knownFood.IndexOf(collectFood)].sentAnts++;

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
				communicate (mem.antsInBase [type][0], task);
			} else {
				Debug.Log("Could not send Ant. No ant in base.");
			}
		}


	}
}


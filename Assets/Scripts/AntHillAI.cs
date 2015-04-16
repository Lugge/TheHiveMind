using UnityEngine;
using System.Collections;
using System.Collections.Generic;

	/*
	 * This class is called once per frame. It is the Behaviour of the AntHill
	 * 
	 * @ToDo: Refactor this! Specific AI classes etc...
	 * 
	 * @author: Lukas Krose
	 * @version: 1.0
	 */
namespace AntHill{
	public class AntHillAI : MonoBehaviour {

		public Rigidbody Ant;

		private int searcherCount = 0;
		private int workerCount = 0;
		private int thinkingtime = 100;
		private Information info = new Information ();

		/*
		 * This function is called upon initialization of the AntHill
		 * 
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		void Start () {		
			info.antHillPosition = transform.position;
		}
		
		/*
		 * This function is called once per frame. The Anthill must decide what step to do next.
		 * 
		 * @ToDo: Develop the perfect learning, self aware and most beautiful AI ever
		 * 
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		void Update () {
			if (searcherCount <= 9 && thinkingtime == 100) {
				spawnSearcher();
				searcherCount++;
				thinkingtime = 0;
			}
			if (workerCount <= 49 && info.knownFood.Count > 0 && thinkingtime == 100) {
				spawnWorker();
				workerCount++;
				thinkingtime = 0;
			}
			if (thinkingtime < 100) {
				thinkingtime++;
			}
		}

		/*
		 * This function creates a new searcher
		 * 
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		void spawnSearcher() {
			Rigidbody searcherClone = (Rigidbody) Instantiate(Ant, transform.position, transform.rotation);
			searcherClone.GetComponent<AntBehaviour> ().init ("Searcher", info);
			instructAnt (searcherClone.GetComponent<AntBehaviour> ().ant);
		}

		/*
		 * This function creates a new worker
		 * 
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		void spawnWorker() {
			Rigidbody searcherClone = (Rigidbody) Instantiate(Ant, transform.position, transform.rotation);
			searcherClone.GetComponent<AntBehaviour> ().init ("Worker", info);
			instructAnt (searcherClone.GetComponent<AntBehaviour> ().ant);
		}

		/*
		 * This function is called when the hill encounters a collision. (e.g. an ant enters the collider)
		 * 
		 * @param: Collider other The object that walked in the colldier
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		void OnTriggerEnter(Collider other) {
			handleAntInBase (other);
		}

		/*
		 * This function is called every frame if a object is in the collider (e.g. the ant is in the base)
		 *
		 * @param: Collider other The object that walked in the colldier
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		void OnTriggerStay(Collider other) {
			handleAntInBase (other);
		}

		/*
		 * Handels the behaviour if an ant is in the base. Supplies the ant if it does not want to communicate
		 * 
		 * @param: Collider other The object that walked in the colldier
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		void handleAntInBase(Collider other) {
			Ant ant = other.GetComponent<AntBehaviour> ().ant;
			if (other.tag == "Ant") {
				if (ant.wantsToCommunicate ()) {
					if(ant.hasReachedTarget())communicate (ant);
				} else if(ant.needsSupply()) {
					ant.supply ();
				}
			}
		}

		/*
		 * Handels the behaviour if an ant wants to communicate with the hill
		 * 
		 * @param: Ant ant The ant that wants to communicate
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		void communicate(Ant ant){
			AntMemory mem = ant.getMemory();
			if (mem.foundFood != null) {
				updateFoodList(mem.foundFood);
			}
			instructAnt (ant);
		}

		/*
		 * Decides what to do next with an given ant.
		 * Instruct the ant when it has decided
		 * 
		 * @param: Ant ant The ant that needs instructions
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		void instructAnt(Ant ant){
			string type = ant.getMemory ().getType ();
			Task task;

			ant.setIdle (true);
			ant.reset();
			ant.instructAnt (info);

			switch (type)
			{
			case "Searcher":
				task = new SearchTask();
				task.target = transform.position;
				break;
			case "Worker":
				task = new CollectTask();
				task.targetFood = decideFoodToCollect();
				break;
			default:
				task = new DefaultTask();
				Debug.Log("Type not found!");
				break;
			}

			ant.setTask(task);
			ant.setIdle (false);
		}

		/*
		 * Decides which food should be collected next (where the worker should be send to)
		 * 
		 * @return: Food The food that should be collected next
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		Food decideFoodToCollect(){
			Food decided = new Food ();
			foreach (Food food in info.knownFood) {
				decided = food;
				break;
			}
			return decided;
		}

		/*
		 * This function is called if an and has found a food pile that needs to be evaluated. If the food is 
		 * already known but the metric is better, it will be updated. New foods will be saved. 
		 * 
		 * @param: Food The food that needs to be evaluated
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		void updateFoodList(Food updateFood) {
			Food newFood = updateFood;
			foreach(Food food in info.knownFood){
				if (food.foodObject == updateFood.foodObject){
					newFood = food;
					if(newFood.path.metric > updateFood.path.metric){
						newFood = updateFood;
						Debug.Log("Update");
					}
					newFood.isEmpty = updateFood.isEmpty;
					info.knownFood.Remove(food);
					break;
				}
			}
			/*
			foreach (Vector3 v in newFood.path.path) {
				Debug.Log(v);
			}
			*/
			info.knownFood.Add (newFood);
		}
	}
}
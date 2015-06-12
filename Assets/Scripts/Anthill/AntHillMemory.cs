using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * This class holds all the information the hill has.
 * 
 * @Todo write getter and setter. All variables have to be private
 * @Todo anntsInBase should not be a dictionars. Refactor the antMemory so it has a position of the ant. 
 * 										Alter all occurences of antsInBase to read from the ants memory.
 * 
 * @author: Lukas Krose
 * @version: 1.0
 */
namespace AntHill
{
	public class AntHillMemory
	{

		public AntHillAIConf conf;
		public Rigidbody ant;

		public AntHill hill;
		public int thinkDelay = 0;

		public int activeFoodSources = 0;
		public List<Food> knownFood = new List<Food>();

		public Dictionary<string, List<Ant>> antsInBase =  new Dictionary<string, List<Ant>>(){
			{"Searcher", new List<Ant>()},
			{"Worker", new List<Ant>()},
		};
		public Dictionary<string, List<Ant>> ants =  new Dictionary<string, List<Ant>>(){
			{"Searcher", new List<Ant>()},
			{"Worker", new List<Ant>()},
		};

		private Dictionary<string,double> baseEval = new Dictionary<string, double>();
		public Dictionary<string,double> currentEval = new Dictionary<string, double>();
		public Dictionary<string, Dictionary <string, string>> evalFunc = new Dictionary<string, Dictionary <string, string>>();

		public AntHillMemory ()
		{
		}

		/*
		 * This function is called if an ant has found a food pile that needs to be evaluated. If the food is 
		 * already known but the metric is better, it will be updated. New foods will be saved. 
		 * It also remembers the number of active food piles (piles that have food left).
		 * Note:Food piles must not be deleted from the known food sources since some ants could be still doing something with that pile.
		 * 
		 * @param: Food The food that needs to be evaluated
		 * @author: Lukas Krose
		 * @version: 1.1
		 */
		public void updateFoodList(Food updateFood) {
			Food newFood = updateFood;

			foreach(Food food in knownFood){
				if (food.foodObject == updateFood.foodObject){
					//newFood = food;
					if(food.path.metric > updateFood.path.metric){
						Debug.Log("Update");
						food.path = updateFood.path;
					}
					if(!food.isEmpty && updateFood.isEmpty){
						food.isEmpty = updateFood.isEmpty;
						Debug.Log(activeFoodSources);
						activeFoodSources--;
					}
					return;
				}
			}
			if (!newFood.isEmpty) {
				activeFoodSources++;
			}
			knownFood.Add (newFood);
		}

		/*
		 * Returns the updated information object
		 * 
		 * @return Information The current information
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public Information getCurrentInformation(){
			Information info = new Information ();

			info.antHillPosition = hill.position;
			info.knownFood = knownFood;

			return info;
		}

		/*
		 * Sets the configuration for the AI.
		 * 
		 * @param AntHillAIConf c The configuration for the AI
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public void setAntHillConfiguration (AntHillAIConf c){
			conf = c;
			baseEval = new Dictionary<string, double>(conf.initialEvalConfig);
			currentEval = new Dictionary<string, double>(conf.initialEvalConfig);
			evalFunc = new Dictionary<string, Dictionary <string, string>>(conf.baseEvalFunc);
		}

		/*
		 * Resets the evaluation Dicitionary of the current desicion
		 * 
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public void resetEval() {
			currentEval = new Dictionary<string, double>(baseEval);
		}

		/*
		 * Adds an ant to the hills memory.
		 * 
		 * @param Ant ant The ant object to be added
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public void addAnt(Ant ant) {
			ants[ant.getType()].Add (ant);
		}

		/*
		 * Removes an ant from the memory.
		 * 
		 * @param Ant ant The ant to be removed
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public void killAnt(Ant ant) {
			ants[ant.getType()].Remove (ant);
		}
	}
}


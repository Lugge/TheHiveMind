using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * This class represents the ant hill. Holds all physical aspects of the hill, as well as a reference to its AI.
 * 
 * @author: Lukas Krose
 * @version: 1.0
 */

namespace AntHill
{
	public class AntHill
	{
		private int foodCount;
		public int size = 1;
		public Vector3 position;
		public Quaternion rotation;
		public int antCost;
		public int maxFood;
		public int maxAnts;
		private int baseFoodCountMax;

		private AntHillAI ai;

		/*
		 * Constructor. Sets the initial values.
		 * 
		 * @param: AntHillAI hillAi The AI for the hill
		 * @param: Vector3 pos The position of the hill
		 * @param: Quaternion rot The rotation of the hill
		 * @param: int initFoodCount The initial amount of food the hill has
		 * @author: Lukas Krose
		 * @version: 1.0
		 */

		public AntHill (AntHillAI hillAI, Vector3 pos, Quaternion rot, int initFoodCount, int cost)
		{
			foodCount = initFoodCount;
			baseFoodCountMax = initFoodCount;
			position = pos;
			rotation = rot;
			ai = hillAI;
			ai.init (this);
			antCost = cost;
			maxFood = baseFoodCountMax * size;
			maxAnts = size * 50;
		}

		/*
		 * This function has to be called once per frame. It updates the hill and redirects the decision making process to the AI.
		 * 
		 * @author: Lukas Krose
		 * @version: 1.0
		 */

		public void Update(){
			if (!ai.canMakeNextDecision()) {
				return;
			}
			ai.decide();
		}

		/*
		 * Updates the food count of the hill.
		 * 
		 * @param: int incBy The amount the food in the hill should be increased / decreased by. 
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public void updateFoodCount (int incBy){
			if (foodCount + incBy > maxFood) {
				ai.setFoodCaps(true);
				throw new UnityException("Cant store more Food");
			}
			ai.setFoodCaps(false);
			foodCount = foodCount + incBy;
		}

		/*
		 * Returns the current food count
		 * 
		 * @param: int The current food count
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public int getFoodCount () {
			return foodCount;
		}

		public void updateSize(){
			Debug.Log ("UpdateSize");
			updateFoodCount (-10000);
			size = size + 1;
			maxFood = baseFoodCountMax * size;
			maxAnts = size * 50;
		}

		/*
		 * Handels the behaviour if an ant is in the base. Supplies the ant if it does not want to communicate
		 * 
		 * @param: Collider other The object that walked in the colldier
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public void handleAntInBase(Collider other) {
			Ant ant = other.GetComponent<AntBehaviour> ().getAnt();
			if (other.tag == "Ant") {
				if (ant.wantsToCommunicate ()) {
					if(ant.hasReachedTarget())ai.antHasReachedBase (ant);
				} else if(ant.needsSupply()) {
					int supplies = ant.getNeededSupplies();
					updateFoodCount(-supplies);
					ant.supply (supplies);
				}
			}
		}
	}
}


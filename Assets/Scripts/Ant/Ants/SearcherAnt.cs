using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AntHill
{
	/*
	 * Specific class / Behaviour for the Searcher-ant
	 * 
	 * @author: Lukas Krose
	 * @version: 1.0
	 */
	public class SearcherAnt : Ant
	{

		private bool hasFoundFood = false;
		private bool hasToUpdateFood = false;
		private GameObject foodToUpdate;

		/*
		 * Returns the next movement desicion of the ant.
		 *
		 * @return: Vector3 The next movement target
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public override Vector3 getNextMovement(){

			if (isIdle ())return mvm.getCurrentPos();

			if(hasToUpdateFood && mvm.isAtPosition(foodToUpdate.transform.position)){
				updateFood();
			}
			if (!mvm.hasStepsLeft()) {
				mvm.phase = 3;
				goHome();
			}

			return mvm.perform ();
		}

		/*
		 * Returns the type of the ant
		 * 
		 * @return: string The type
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public override string getType() {
			return "Searcher";
		}

		/*
		 * Handles the moment when an ant gets a collision with a collider of a food object.
		 * Checks if the food has already been found and if yes it checks if the metric of the current path 
		 * is better, e.g. it has found a shorter route to the food (random optimization).
		 * 
		 * If a valid food pile has been found it sets next target position to the food position and updates the search task to phase 2.
		 *
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public void handleFood(Collider other) {
			if (hasFoundFood) {
				return;
			}

			foreach(Food food in mem.knownFood){
				if (food.foodObject == other.gameObject){
					if (food.path.metric <= mvm.getStepCount() && !food.isEmpty){
						return;
					}
					break;
				}
			}
			mvm.phase = 2;
			mvm.target = other.gameObject.transform.position;
			foodToUpdate = other.gameObject;
			hasToUpdateFood = true;

		}

		/*
		 * Handles the moment when an ant gets a collision. If the collision is a food object it triggers the handleFood method.
		 * 
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public override void handleCollissionEnter(Collider other){
			base.handleCollissionEnter(other);
			if (other.tag == "Food" && Util.isValid(other.transform.position, mvm.getCurrentPos())) {
				handleFood(other);
			}
		}

		/*
		 * Resets the ant to the same state it has been initialized
		 * 
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public override void reset() {
			base.reset ();
		}

		/*
		 * Supplies the ant and resets the movement
		 * 
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public override void supply() {
			mvm.supply (mem.antHillPosition);
		}
				
		/*
		 * Updates the food object in the memory when food has been found.
		 * Sets the phase of the Search-Task to 3.
		 * 
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		private void updateFood(){
			Food foundFood = new Food();
			foundFood.foodObject = foodToUpdate;
			foundFood.path = mvm.path;
			if(!foodToUpdate.GetComponent<foodObject> ().hasFoodLeft()){
				foundFood.isEmpty = true;
			}
			
			mem.foundFood = foundFood;
			hasFoundFood = true;
			Debug.Log("found Food");
			
			mvm.phase = 3;
			goHome ();
		}
	}
}


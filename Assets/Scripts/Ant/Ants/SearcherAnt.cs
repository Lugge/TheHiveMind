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
			if (!mvm.hasReachedNextPosition ())return mvm.getStepTarget();
			if (isIdle ())return mvm.getCurrentPos();

			int add = 0;
			if (getTask () == "SearchTask") {
				add = 1;
			}

			if(hasToUpdateFood && mvm.isAtPosition(foodToUpdate.transform.position)){
				updateFood();
			}

			if (getTask () == "OptimizeTask" && mvm.success() && mvm.finished() && mvm.isAtFinalTarget() && !hasFoundFood) {
				try{
					foodToUpdate = mem.getCloseObjectAtPosition(mvm.getCurrentPos(), "Food").gameObject;
					hasFoundFood = true;
					updateFood();
				}
				catch(UnityException e){
					Debug.Log(e + " Position:" + mvm.getCurrentPos());
				}
			}

			if (foodSupplies <= (maxSupplies / 2) + add) {
				goHome();
			}
			foodSupplies--;


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
			if (!Util.isValid (other.gameObject.transform.position, mvm.getCurrentPos())) {
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
			mvm.setTarget(other.gameObject.transform.position);
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
			if (other.tag == "Food" && Util.isValid(other.transform.position, mvm.getCurrentPos()) && getTask() == "SearchTask" ) {
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
			hasFoundFood = false;
			hasToUpdateFood = false;
			foodToUpdate = null;
			base.reset ();
		}

		/*
		 * Supplies the ant and resets the movement
		 * 
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public override void supply(int supplies) {
			base.supply (supplies);
			mvm.supply (mem.antHillPosition);
		}
				
		/*
		 * Updates the food object in the memory when food has been found.
		 * 
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		private void updateFood(){
			Food foundFood = new Food();
			foundFood.foodObject = foodToUpdate;
			foundFood.path = new Path (mvm.traveledPath);
			if(!foodToUpdate.GetComponent<foodObject> ().hasFoodLeft()){
				foundFood.isEmpty = true;
			}
			
			mem.foundFood = foundFood;
			hasFoundFood = true;
			Debug.Log("found Food");

			mvm.transferPath ();

			goHome ();
		}
	}
}


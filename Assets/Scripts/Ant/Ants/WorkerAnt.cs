using UnityEngine;
using System.Collections;

namespace AntHill
{
	/*
	 * Specific class / Behaviour for the Worker-ant
	 * 
	 * @author: Lukas Krose
	 * @version: 1.0
	 */
	public class WorkerAnt : Ant
	{

		private bool isAtFood = false;

		/*
		 * Returns the next movement desicion of the ant.
		 *
		 * @return: Vector3 The next movement target
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public override Vector3 getNextMovement(){
			if (!mvm.hasReachedNextPosition ())return mvm.getStepTarget();
			if (isIdle () && !isAtFood)return mvm.getCurrentPos();

			if (foodSupplies <= (maxSupplies / 2) + 1){
				goHome();
			}

			if (mvm.hasReachedTarget () && !mvm.finished()) {
				handleReachedTarget ();
			} else {
				foodSupplies--;
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
			return "Worker";
		}

		/*
		 * Handles when the ant has reached its target. Collects the food and checks if the food pile is empty
		 * 
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		private void handleReachedTarget(){
			if(!mem.getCloseObjectAtPosition (mvm.getTarget(), "Food")){
				return;
			}
			isAtFood = true;
			setIdle(true);
			foodObject food = mem.getCloseObjectAtPosition (mvm.getTarget(), "Food").GetComponent<foodObject> ();
			if (carriedWeight < prop.carryCapability && food.hasFoodLeft ()) {
				collect ();
			} else if (!food.hasFoodLeft ()) {
				Food foundFood = new Food();
				foundFood.foodObject = food.gameObject;
				foundFood.path = mvm.providedPath;
				foundFood.isEmpty = true;

				mem.foundFood = foundFood;
				Debug.Log("Food Empty");

				setIdle(false);
				goHome();
				mem.initCommunication = true;
				isAtFood = false;
			}else {
				setIdle(false);
				goHome();
				mem.initCommunication = true;
				isAtFood = false;
			}
		}

		/*
		 * Collects the resources
		 * 
		 * @author: Lukas Krose
		 * @version: 1.0
		 */

		private void collect(){
			foodObject food = mem.getCloseObjectAtPosition (mvm.getTarget(), "Food").GetComponent<foodObject> ();
			if (mem.typeOfCarriedObjects == "none") {
				mem.typeOfCarriedObjects = food.tag;
			}
			carriedWeight++; 
			food.lowerFoodPoints (1);
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
		public override void supply(int supplies) {
			base.supply (supplies);
			return;
		}
	}
}


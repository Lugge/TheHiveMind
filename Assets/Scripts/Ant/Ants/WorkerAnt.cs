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
		/*
		 * Returns the next movement desicion of the ant.
		 *
		 * @return: Vector3 The next movement target
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public override Vector3 getNextMovement(){

			if (!mvm.hasStepsLeft()){
				goHome();
			}


			if(mvm.hasReachedTarget()){
				handleReachedTarget();
			}


			if (isIdle())return mvm.getCurrentPos();
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
			setIdle(true);
			if (carriedWeight < prop.carryCapability && mvm.targetFood.foodObject.GetComponent<foodObject> ().hasFoodLeft ()) {
				collect ();
			} else if (!mvm.targetFood.foodObject.GetComponent<foodObject> ().hasFoodLeft ()) {
				Food foundFood = new Food();
				foundFood.foodObject = mvm.targetFood.foodObject;
				foundFood.path = mvm.path;
				foundFood.isEmpty = true;

				mem.foundFood = foundFood;
				Debug.Log("Food Empty");

				setIdle(false);
				goHome();
				mem.initCommunication = true;
			}else {
				setIdle(false);
				goHome();
				mem.initCommunication = true;
			}
		}

		/*
		 * Collects the resources
		 * 
		 * @author: Lukas Krose
		 * @version: 1.0
		 */

		private void collect(){
			if (mem.typeOfCarriedObjects == "none") {
				Collider obj = mem.getCloseObjectAtPosition (mvm.getCurrentPos ());
				mem.typeOfCarriedObjects = obj.gameObject.tag;
			}
			carriedWeight++; 
			mvm.targetFood.foodObject.GetComponent<foodObject> ().lowerFoodPoints (1);
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
			return;
		}
	}
}


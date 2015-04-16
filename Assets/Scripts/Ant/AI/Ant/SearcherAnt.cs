using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AntHill
{
	public class SearcherAnt : AntAI
	{

		public override Vector3 getNextMovement(){

			if (hasReachedNextPosition()) {
				decideMovement ();
			}
			return nextMovementTarget;
		}

		public void handleFood(Collider other) {
			if (memory.foundFood != null) {
				return;
			}
			foreach(Food food in memory.knownFood){
				if (food.foodObject == other.gameObject){
					if (food.path.metric <= currentStepCount && !food.isEmpty){
						return;
					}
					break;
				}
			}
			
			moveTo (other.transform.position);
			
			Food foundFood = new Food();
			foundFood.foodObject = other.gameObject;
			foundFood.path = memory.path;
			if(other.gameObject.GetComponent<foodHandler> ().foodPoints <= 0){
				foundFood.isEmpty = true;
			}

			memory.foundFood = foundFood;
			Debug.Log("found Food");
			
		}

		public override void handleCollission(Collider other){
			if (other.tag == "Food" && Util.isValid(other.transform.position, currentPosition)) {
				handleFood(other);
			}
		}

		private void decideMovement() {
			stepCount++;
			if (isIdle) {
				nextMovementTarget = currentPosition;
				return;
			}

			if ((memory.foundFood != null || maxMovements <= stepCount)) {
				goHome();
			}

			if (target != currentPosition) {
				moveToStep (target);
				return;
			}

			currentStepCount++;
			memory.path.addMovement (nextMovementTarget);			
			target = nextMovementTarget;
			
			switch (memory.currentTask.taskType)
			{
			case 0:
				setIdle(true);
				break;
			case 1:
				search();
				break;
			case 2:
				optimize();
				break;
			default:
				Debug.Log("Task not found!");
				break;
			}
		}
		

		private void optimize() {
			

			Vector3 lastCoord = memory.currentTask.target;
			
			if(currentPosition == lastCoord){
				goHome();
				return;
			}

			Vector3 direction = currentPosition + lastCoord;
			Vector3 clampedDir = Vector3.ClampMagnitude(direction, maxTrvl);

			if (Util.isValid (clampedDir, currentPosition)) {
				nextMovementTarget = clampedDir;
				return;
			}

			nextMovementTarget = clampedDir;

		}
		public override void reset() {
			currentStepCount = 0;
			stepCount = 0;
			memory.reset ();
		}
		public override void supply() {
			currentStepCount = 0;
			stepCount = 0;
			memory.supply ();
		}
	}
}


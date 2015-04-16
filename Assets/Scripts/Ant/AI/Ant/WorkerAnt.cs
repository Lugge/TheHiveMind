using UnityEngine;
using System.Collections;

namespace AntHill
{
	public class WorkerAnt : AntAI
	{

		public override Vector3 getNextMovement(){
			
			if (hasReachedNextPosition()) {
				decideMovement ();
			}
			return nextMovementTarget;
		}
		public override void handleCollission(Collider other){
			return;
		}
		private void decideMovement() {
			stepCount++;
			if (isIdle) {
				nextMovementTarget = currentPosition;
				return;
			}
			if (currentPosition == target) {
				goHome();
			}
			if (target != currentPosition) {
				moveToStep (target);
			}
		}
		public override void reset() {
			currentStepCount = 0;
			stepCount = 0;
			memory.reset ();
		}
		public override void supply() {
			return;
		}
	}
}


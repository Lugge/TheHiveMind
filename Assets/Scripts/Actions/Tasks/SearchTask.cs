
using UnityEngine;
using System.Collections;

namespace AntHill
{
	/*
	 * Custom Task to search for new food positions.
	 * 
	 * @author: Lukas Krose
	 * @version: 1.0
	 */
	public class SearchTask : Task
	{

		public SearchTask(){

		}

		/*
		 * Returns the type of the task
		 * 
		 * @return: string The type
		 * @author: Lukas Krose
		 * @version: 1.0
		 */

		public override string getType ()
		{
			return "SearchTask";
		}

		/*
		 * Initializes the task. Path is resetet and the position of the ant hill is addeed as first entry to the path.
		 * The current phase is reseted to 1.
		 * 
		 * @param: Vector3 anthillPosition The Position of the ant hill
		 * @param: AntProperties prop The properties of the ant
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public override void init(Vector3 anthillPosition, AntProperties properties){
			currentStepCount = 0;
			stepCount = 0;

			baseInit (anthillPosition, properties);
			path = new Path ();
			path.addMovement (anthillPosition);
			currentStepCount++;
			phase = 1;
			return;
		}

		/*
		 * Runs the task and returns the next position the ant should go to.
		 * Performs based on the current active phase.
		 * 
		 * Phases:
		 * - Phase 1: Ant moves random and searches for food
		 * - Phase 2: Food has been found and the ant moves directly to the given food position
		 * - Phase 3: Food has been found and visited. The ant travels back to the hill on the path it has taken.
		 * 
		 * 
		 * @return Vector3 The current step if the targtet has not been reached or the next step on the path
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public override Vector3 perform() {
			if (!hasReachedNextPosition ())return nextMovementTarget;
			stepCount++;
			if (phase == 2) {
				return phase2();
			}
			if (phase == 3) {
				return phase3();
			}
			return phase1();
		}

		/*
		 * Resets the task. 
		 * Just reinits.
		 * 
		 * @param: Vector3 anthillPosition The Position of the ant hill
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public override void reset(Vector3 anthillPosition){
			init (anthillPosition, prop);
		}

		/*
		 * Resets the movement. 
		 * Just reinits.
		 * 
		 * @param: Vector3 anthillPosition The position of the ant hill 
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public override void supply(Vector3 anthillPosition){
			currentStepCount = 0;

			baseInit (anthillPosition, prop);
			path = new Path ();
			path.addMovement (anthillPosition);
			currentStepCount++;
			phase = 1;
			return;
		}



		/*
		 * Phase 1 of the task.
		 * Tries to find a valid next movemnt target (with no obstacles in between) and randomly runs trough the world.
		 * The step is added to the path.
		 * 
		 * @return: Vector3 A valid next movement target 
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		private Vector3 phase1(){

			do{
				nextMovementTarget = currentPosition + new Vector3 ((Random.value - 0.5f) * prop.maxTrvl, 0.0f, (Random.value - 0.5f) * prop.maxTrvl);
			}while(!Util.isValid (nextMovementTarget, currentPosition));
			
			currentStepCount++;
			path.addMovement (nextMovementTarget);			
			target = nextMovementTarget;
			return nextMovementTarget;
		}

		/*
		 * Phase 2 of the task.
		 * Goes to the position of the target - variable, which should be set to the found food position at this point.
		 * The step is added to the path.
		 * 
		 * (Note: This step has to be done in order to add the last step (position of the food source) to the path)
		 * 
		 * @return: Vector3 The position of the target 
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		private Vector3 phase2(){
			return moveTo(target);
		}

		/*
		 * Phase 3 of the task.
		 * The ant goes back to the ant hill
		 * 
		 * @return: Vector3 The next step on the path to the ant hill 
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		private Vector3 phase3(){
			return getNextStepOnPath(target);
		}
	}
}

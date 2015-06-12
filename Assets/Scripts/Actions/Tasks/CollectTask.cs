using UnityEngine;
using System.Collections;

namespace AntHill
{
	/*
	 * Custom Task to Collect Food.
	 * 
	 * @author: Lukas Krose
	 * @version: 1.0
	 */
	public class CollectTask : Task
	{

		/*
		 * Returns the type of the task
		 * 
		 * @return: string The type
		 * @author: Lukas Krose
		 * @version: 1.0
		 */

		public override string getType ()
		{
			return "CollectTask";
		}

	
		/*
		 * Initializes the task. Sets the path to the path to the targeted food.
		 * 
		 * @param: Vector3 anthillPosition The Position of the ant hill
		 * @param: AntProperties prop The properties of the ant
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public override void init(Vector3 anthillPosition, AntProperties properties){
			baseInit (anthillPosition, properties);
			path = targetFood.path;
			target = targetFood.foodObject.transform.position;

			return;
		}

		/*
		 * Runs the task and returns the next position the ant should go to. 
		 * Returns the current target if the next position has not been reached.
		 * Otherwise it returns the next step on the path to the food.
		 * 
		 * @return Vector3 The current step if the targtet has not been reached or the next step on the path
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public override Vector3 perform(){
			if (!hasReachedNextPosition ())return nextMovementTarget;
			stepCount++;
			return getNextStepOnPath (target);
		}
		/*
		 * Resets the task. 
		 * Clears the current and total step count and reinitializes itself.
		 * 
		 * @param: Vector3 anthillPosition The Position of the ant hill
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public override void reset(Vector3 anthillPosition){
			currentStepCount = 0;
			stepCount = 0;
			path = new Path();
			init (anthillPosition, prop);
		}
		
		/*
		 * Resets the movement. 
		 * Currently has no use since the Collect-task doesn't have to be supplied. 
		 * (Movements, steps, etc are given by the path to the food)
		 * 
		 * @param: Vector3 anthillPosition The Position of the ant hill
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public override void supply(Vector3 anthillPosition){

		}
	}
}


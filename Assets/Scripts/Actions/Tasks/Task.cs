using UnityEngine;
using System.Collections;

namespace AntHill
{
	/*
	 * Abstract parent class for the individual tasks.
	 * Holds all needed informations in order to perform each task
	 * 
	 * @author: Lukas Krose
	 * @version: 1.0
	 */
	public abstract class Task
	{
		//Ant specific Properties, should be set initially and never be changed
		protected AntProperties prop;

		//Movement-related variables, should only be changed by tasks
		protected int currentStepCount = 0;
		protected int stepCount = 0;
		protected Vector3 currentPosition;
		protected Vector3 nextMovementTarget;

		//Task related variables
		public Path path = new Path();
		public int phase = 0;
		public Food targetFood;
		public Vector3 target;

		public abstract void init (Vector3 anthillPosition, AntProperties prop);
		public abstract Vector3 perform ();
		public abstract void reset (Vector3 anthillPosition);
		public abstract void supply (Vector3 anthillPosition);
		public abstract string getType ();


		/*
		 * Moves to the next step on a path
		 * Returns the next position on the current path in direction of a given position (on the path)
		 * Returns the current position if the target position is not in the list or the target has been reached
		 * 
		 * @param: Vector3 position The target position on the path
		 * @return: Vector3 The next step on the path or the current position
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public Vector3 getNextStepOnPath(Vector3 position) {

			int arrayPos = path.path.IndexOf (position);

			if (arrayPos == -1 || currentStepCount == arrayPos)
				return currentPosition;

			if (currentStepCount < arrayPos) {
				currentStepCount++;
			} else{
				currentStepCount--;
			}

			nextMovementTarget = path.getMovement (currentStepCount);
			return nextMovementTarget;			
		}

		/*
		 * Moves to a given position
		 * Returns the next valid position in the direction of an given position or the position if it is directly reachable.
		 * Returns the current position if the target is not reachable
		 * 
		 * @ToDo: Split path into multiple vectors if position is too far away
		 * @ToDo obstacle avoidance?, return to origin if the position could not be reached?
		 * 
		 * @param: Vector3 position The target position
		 * @return: Vector3 The next step or the current position
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public Vector3 moveTo(Vector3 position) {

			currentStepCount++;
			path.addMovement (position);
			nextMovementTarget = position;
			return nextMovementTarget;
		}

		/*
		 * Updates the current position of the ant. Has to be called each frame.
		 * 
		 * @param: Vector3 position The current position
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public void updatePosition(Vector3 position){
			currentPosition = position;
		}

		/*
		 * Checks if the next position on the path has been reached
		 * 
		 * @return: bool
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public bool hasReachedNextPosition(){
			return currentPosition == nextMovementTarget;
		}

		/*
		 * Checks if the final position has been reached
		 * 
		 * @return: bool
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public bool hasReachedTarget(){
			return target == currentPosition;
		}

		/*
		 * Checks if the ant has steps left or has to return to the hill.
		 * 
		 * @return: bool
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public bool hasStepsLeft(){
			return prop.maxMovements >= stepCount;
		}

		/*
		 * Checks if the ant is at a given position
		 * 
		 * @param: Vector3 position The position to check
		 * @return: bool
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public bool isAtPosition(Vector3 position){
			return currentPosition == position;
		}

		/*
		 * Indicates if the ant needs to be supplied
		 * (Note: The ant needs to be supplied if it has made more than three steps, because by then it has been surely out of the hill)
		 * 
		 * @return: bool
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public bool needsSupply(){
			return currentStepCount > 2;
		}

		/*
		 * Returns the current position
		 *
		 * @return: Vector3 The current position
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public Vector3 getCurrentPos(){
			return currentPosition;
		}

		/*
		 * Returns the current number of steps taken
		 *
		 * @return: int The number of steps taken
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public int getStepCount(){
			return stepCount;
		}

		/*
		 * Initializes the basic values of the task
		 * Is only called on entry of custom, task specific init.
		 * 
		 * @param: Vector3 anthillPosition The Position of the ant hill
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		protected void baseInit(Vector3 anthillPosition, AntProperties properties){
			currentPosition = anthillPosition;
			nextMovementTarget = currentPosition;
			prop = properties;

		}


	}
}


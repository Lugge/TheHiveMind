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
		protected Vector3 currentPosition;
		protected Vector3 nextMovementTarget;
		protected bool taskSuccessful = false;
		protected bool taskFinished = false;


		//Task related variables
		public Path traveledPath = new Path();
		public Path providedPath = new Path();


		public int phase = 0;
		//public Food targetFood;
		protected Vector3 finalTarget;
		protected Vector3 initialTarget;

		public abstract void init (Vector3 anthillPosition, AntProperties prop);
		public abstract Vector3 perform ();
		public abstract void reset (Vector3 anthillPosition);
		public abstract void supply (Vector3 anthillPosition);
		public abstract string getType ();
		public abstract void goHome ();


		/*
		 * Retruns a step on the path
		 * Returns the next position on the current path in direction of a given position (on the path)
		 * Returns the current position if the target position is not in the list or the target has been reached
		 * 
		 * @param: Vector3 position The target position on the path
		 * @return: Vector3 The next step on the path or the current position
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public Vector3 moveOnPath(Vector3 position, Path localPath) {

			int arrayPos = localPath.path.IndexOf (position);
			Vector3 currentStepObject = localPath.getStepObject (currentPosition);
			if (!localPath.path.Contains (currentStepObject)) {
				throw new UnityException ("You are not on a path");
			}

			int currentPathPosition = localPath.path.IndexOf (currentStepObject);

			if (arrayPos == -1 || currentPathPosition == arrayPos)
				return currentStepObject;

			if (currentPathPosition < arrayPos) {
				currentPathPosition++;
			} else{
				currentPathPosition--;
			}
			nextMovementTarget = localPath.getMovement (currentPathPosition);

			traveledPath.addMovement (nextMovementTarget);			
			return nextMovementTarget;			
		}

		public Vector3 moveOnPath(Vector3 position) {
			return moveOnPath (position, providedPath);
		}

		public Vector3 moveRandom() {
			do{
				nextMovementTarget = currentPosition + new Vector3 ((Random.value - 0.5f) * prop.maxTrvl, 0.0f, (Random.value - 0.5f) * prop.maxTrvl);
			}while(!Util.isValid (nextMovementTarget, currentPosition));
			
			currentStepCount++;
			traveledPath.addMovement (nextMovementTarget);			
			return nextMovementTarget;
		}
	
		/*
		 * Return a Vector towards a given position
		 * Returns the next valid position in the direction of an given position or the position if it is directly reachable.
		 * Returns the current position if the target is not reachable
		 * 
		 * @ToDo obstacle avoidance?, return to origin if the position could not be reached?
		 * 
		 * @param: Vector3 position The target position
		 * @return: Vector3 The next step or the current position
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public Vector3 moveDirection(Vector3 position) {
			currentStepCount++;
			Vector3 dirVector = position - currentPosition;
			Vector3 direction = position;

			if(dirVector.magnitude > prop.maxTrvl){
				direction = currentPosition + Vector3.ClampMagnitude(dirVector, prop.maxTrvl);;

			}
			if (!Util.isValid (direction, currentPosition)) {
				throw new UnityException("Direction not valid");
			}
			nextMovementTarget = direction;
			traveledPath.addMovement (nextMovementTarget);			
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
			return finalTarget == currentPosition;
		}

		/*
		 * Checks if the ant has steps left or has to return to the hill.
		 * 
		 * @return: bool
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public int getCurrentStepCount(){
			return currentStepCount;
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
			return currentStepCount;
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

		public void setTarget (Vector3 target) {
			finalTarget = target;
		}

		public void setPath (Path path) {
			providedPath = path;
		}

		public Vector3 getTarget() {
			return finalTarget;
		}

		public Vector3 getStepTarget() {
			return nextMovementTarget;
		}

		public bool success() {
			return taskSuccessful;
		}

		public bool finished() {
			return taskFinished;
		}

		public void transferPath () {
			providedPath = new Path(traveledPath);
		}

		public Vector3 getInitialTarget() {
			return initialTarget;
		}

		public bool isAtFinalTarget() {
			return currentPosition == initialTarget;
		}
	}
}


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AntHill
{
	/*
	 * Dummy Task. This task is asigned as a default, so the ant can access the movement methods in the abstract base class.
	 * 
	 * @author: Lukas Krose
	 * @version: 1.0
	 */
	public class DefaultTask : Task
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
			return "DefaultTask";
		}


		/*
		 * Initializes the task. Path is reset and the position of the ant hill is addeed as first entry to the path.
		 * 
		 * @param: Vector3 anthillPosition The Position of the ant hill
		 * @param: AntProperties prop The properties of the ant
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public override void init(Vector3 anthillPosition, AntProperties properties){
			baseInit (anthillPosition, properties);
			path = new Path ();
			path.addMovement (anthillPosition);
			currentStepCount++;
			return;
		}
		/*
		 * Runs the task. 
		 * Does nothing (ant stays at its position).
		 *
		 * @return Vector3 The current position of the ant
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public override Vector3 perform(){
			if (!hasReachedNextPosition ())return currentPosition;
			return currentPosition;
		}

		/*
		 * Resets the task. 
		 * Does nothing since the task does not need to be reset
		 * 
		 * @param: Vector3 anthillPosition The Position of the ant hill
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public override void reset(Vector3 anthillPosition){
			return;
		}
		/*
		 * Resets the movement. 
		 * Does nothing since the task does not need to be reset
		 * 
		 * @param: Vector3 anthillPosition The Position of the ant hill
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public override void supply(Vector3 anthillPosition){
			return;
		}

	}
}


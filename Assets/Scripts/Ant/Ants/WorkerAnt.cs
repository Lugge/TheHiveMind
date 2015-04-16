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
			if (isIdle())return mvm.getCurrentPos();

			if (!mvm.hasStepsLeft() || mvm.hasReachedTarget()) {
				goHome();
			}
			if (returnHome && mvm.hasReachedTarget ()) {
				mem.initCommunication = true;
			}

			return mvm.perform ();
		}

		/*
		 * Handles the moment when an ant gets a collision.
		 * 
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public override void handleCollission(Collider other){
			return;
		}

		/*
		 * Resets the ant to the same state it has been initialized
		 * 
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public override void reset() {
			returnHome = false;
			mvm.reset(mem.antHillPosition);
			mem.reset ();
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


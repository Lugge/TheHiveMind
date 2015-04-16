using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AntHill
{
	/*
	 * Basic behaviour for ants
	 * 
	 * @author: Lukas Krose
	 * @version: 1.0
	 */
	public abstract class Ant : Behaviour
	{
		protected AntMemory mem;
		protected AntAI ai;
		protected AntProperties prop;

		public Task mvm;
		public bool returnHome = false;


		public abstract Vector3 getNextMovement();
		public abstract void handleCollission (Collider other);
		public abstract void supply ();
		public abstract void reset ();


		/*
		 * Inits the ant. Set all relevant information.
		 * 
		 * @param: string type Type of the ant
		 * @param: Information info The info providedd by the hill
		 * @param: int maxMvnts The maximum steps an ant can take
		 * @param: int maxT The maximum distance an ant can travel
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public void init (string type, Information info, int maxMvnts, float maxT)
		{	
			mem = new AntMemory (type);
			prop = new AntProperties ();
			prop.maxMovements = maxMvnts;
			prop.maxTrvl = maxT;
			ai = new AntAI ();
			setIdle (true);

			mem.antHillPosition = info.antHillPosition;
			setTask(new DefaultTask ());		
		}

		/*
		 * Updates the ant with new information from the hill
		 * 
		 * @param: Information The Information from the hill
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public void instructAnt(Information info){
			mem.knownFood = info.knownFood.ToArray();
			mem.antHillPosition = info.antHillPosition;
		}

		/*
		 * Sets the ant idle so it will not do anyhting
		 * 
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public void setIdle(bool idle){
			ai.setIdle (idle);
		}

		/*
		 * Assings a task to the ant
		 * 
		 * @param: task The task that should be assigned
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public void setTask(Task task) {
			mvm = task;
			mvm.init (mem.antHillPosition, prop);
		}

		/*
		 * Tells the ant to go back to the hill and init communication
		 * (e.g. task is finished)
		 * 
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public void goHome(){
			mem.initCommunication = true;
			mvm.target = mem.antHillPosition;
		}

		/*
		 * Updates the current position of the ant. Has to be called once per frame.
		 * 
		 * @param: Vector3 position The current position of the ant
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public void updatePosition(Vector3 position){
			mvm.updatePosition (position);
		}


		/*
		 * Returns The Memory-Information of the ant
		 * 
		 * @return: AntMemory The Memory of the ant
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public AntMemory getMemory(){
			return mem;
		}

		/*
		 * Returns if the ant is idle
		 * 
		 * @return: bool True if the ant is idle
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public bool isIdle(){
			return ai.idle();
		}

		/*
		 * Returns if the needs to be supplied
		 * (If the wantsToCommunicate-Variable is set, the ant has finished its task and needs instructions, not supplies)
		 * 
		 * @return: bool True if the ant needs supplies
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public bool needsSupply(){
			if (wantsToCommunicate ()) {
				Debug.Log("hier");
				return false;
			}
			return mvm.needsSupply ();
		}

		/*
		 * Returns if the ant has reached its final target
		 * 
		 * @return: bool True if the ant has reached its target
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public bool hasReachedTarget(){
			return mvm.hasReachedTarget ();
		}

		/*
		 * Returns if the ant has reached its next position
		 * 
		 * @return: bool True if the ant has reached its position
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public bool hasReachedNextPosition(){
			return mvm.hasReachedNextPosition ();
		}

		/*
		 * Returns if the wants to communicate with the hill for further instructions
		 * 
		 * @return: bool True if the ant wants to communicate
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public bool wantsToCommunicate(){
			return mem.initCommunication;
		}

	}
}


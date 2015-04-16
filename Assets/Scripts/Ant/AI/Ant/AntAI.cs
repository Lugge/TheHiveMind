using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AntHill
{
	public abstract class AntAI : Behaviour
	{

		protected Vector3 target;

		protected Vector3 nextMovementTarget = new Vector3();
		protected bool returnHome = false;
		protected Vector3 currentPosition;
		protected int maxMovements = 0;
		protected float maxTrvl;
		protected bool isIdle = true;
		protected AntMemory memory;

		public abstract Vector3 getNextMovement();
		public abstract void handleCollission (Collider other);
		public abstract void supply ();
		public abstract void reset ();

		public void init(string type, int maxMvnts, float maxT, Information info){
			AntMemory mem = new AntMemory (type);
			mem.rememberAntHillPos (info.antHillPosition);
			mem.knownFood = info.knownFood.ToArray();
			memory = mem;

			maxMovements = maxMvnts;
			maxTrvl = maxT;
			updatePosition (info.antHillPosition);
			nextMovementTarget = currentPosition;
		}
		public void setIdle(bool idle){
			isIdle = idle;
		}
		public void updatePosition(Vector3 position){
			currentPosition = position;
		}
		public bool hasReachedNextPosition(){
			return currentPosition == nextMovementTarget;
		}

		public void instructAnt(string type, Information info){
			memory.knownFood = info.knownFood.ToArray();
			memory.rememberAntHillPos(info.antHillPosition);
		}
		public AntMemory getMemory(){
			return memory;
		}
		public bool idle(){
			return isIdle;
		}

		public bool isAtHomeBase(){
			return currentPosition == memory.antHillPosition;
		}
		public bool wantsToCommunicate(){
			return memory.initCommunication;
		}


		

		protected void goHome(){
			memory.initCommunication = true;
			target = memory.antHillPosition;
		}
		public void setTask(AbstractTask task) {
			memory.setTask (task);
			target = task.target;

			switch (memory.currentTask.taskType) {
			case 0:
				setIdle (true);
				break;
			case 1:

				break;
			case 2:

				break;
			case 3:
				memory.path = task.targetFood.path;
				break;
			default:
				Debug.Log ("Task not found!");
				break;

			}
		}

	}
}


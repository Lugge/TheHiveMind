using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AntHill
{
	public class AntMemory
	{
		public Vector3 antHillPosition;
		public Food[] knownFood = new Food[0];
		public Food foundFood = null;

		public Path path = new Path();
		protected Vector3 target;

		public AbstractTask currentTask;
		public Vector3 lastValidOptimizationPos = new Vector3 ();
		public bool initCommunication = false;

		private string antType;

		public AntMemory (string type)
		{
			antType = type;
		}
		public string getType(){
			return antType;
		}
		public void reset() {
			knownFood = new Food[0];
			foundFood = null;		
			path = new Path();
			currentTask = null;
			target = antHillPosition;
			initCommunication = false;
			rememberAntHillPos (antHillPosition);
		}
		public void supply() {	
			path = new Path();
			rememberAntHillPos (antHillPosition);
		}
		public void rememberAntHillPos(Vector3 position) {
			antHillPosition = position;
			path.addMovement (position);
		}
		public void setTask(AbstractTask task){
			currentTask = task;
		}
	}
}
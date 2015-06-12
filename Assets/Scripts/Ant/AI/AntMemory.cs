using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AntHill
{
	/*
	 * Memory class for the ant. Holds all information the ant has.
	 * 
	 * @author: Lukas Krose
	 * @version: 1.0
	 */
	public class AntMemory
	{

		//Type of ant, can not be changed
		private string antType;
		private List<Collider> closeObjects = new List<Collider>();


		public Vector3 antHillPosition;
		public Food[] knownFood = new Food[0];
		public Food foundFood = null;
		public bool initCommunication = false;
		public string typeOfCarriedObjects = "none";


		/*
		 * Constructor for the Memory, sets the type
		 * 
		 * @param: string type Type of the ant
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public AntMemory (string type)
		{
			antType = type;
		}

		/*
		 * Returns the type of the ant
		 * 
		 * @return: string type Type of the ant
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public string getType(){
			return antType;
		}

		/*
		 * Resets the Memory
		 * 
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public void reset() {
			knownFood = new Food[0];
			foundFood = null;		
			initCommunication = false;
		}

		public void enterCloseObject(Collider obj){
			closeObjects.Add (obj);
		}
		public void exitCloseObject(Collider obj){
			closeObjects.Remove (obj);
		}
		public Collider getCloseObjectAtPosition(Vector3 pos){
			foreach (Collider col in closeObjects) {
				if(col.transform.position == pos)return col;
			}
			return new Collider ();
		}
	}
}
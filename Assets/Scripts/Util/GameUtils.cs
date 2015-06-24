using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/*
 * This class contains helper methods for the AntHill
 *  
 * @author: Lukas Krose
 * @version: 1.0
 */
namespace AntHill
{
	
	public class GameUtils {
		
		/*
		 * This function creates a new searcher
		 * 
		 * @param Rigidbody ant The ant template
		 * @param Vector3 at The position where the ant should be spawned
		 * @param Quaternion rotation The rotation of the ant
		 * @return Ant The created ant
		 * @author: Lukas Krose
		 * @version: 1.1
		 */
		public Ant spawnSearcher(Rigidbody ant, Vector3 at, Quaternion rotation) {
			
			Rigidbody searcherClone = (Rigidbody) MonoBehaviour.Instantiate(ant, at, rotation);
			
			searcherClone.GetComponent<AntBehaviour> ().init ("Searcher");
			Ant newAnt = searcherClone.GetComponent<AntBehaviour> ().getAnt ();
			newAnt.antRenderer = searcherClone.GetComponent<Renderer> ();
			return newAnt;
		}
		
		
		/*
		 * This function creates a new worker
		 * 
		 * @param Rigidbody ant The ant template
		 * @param Vector3 at The position where the ant should be spawned
		 * @param Quaternion rotation The rotation of the ant
		 * @return Ant The created ant
		 * 
		 * @author: Lukas Krose
		 * @version: 1.1
		 */
		public Ant spawnWorker(Rigidbody ant, Vector3 at, Quaternion rotation) {
			Rigidbody searcherClone = (Rigidbody) MonoBehaviour.Instantiate(ant, at, rotation);
			searcherClone.GetComponent<AntBehaviour> ().init ("Worker");
			Ant newAnt = searcherClone.GetComponent<AntBehaviour> ().getAnt ();
			newAnt.antRenderer = searcherClone.GetComponent<Renderer> ();
			return newAnt;
		}
		
	}
}



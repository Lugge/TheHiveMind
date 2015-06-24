using UnityEngine;
using System.Collections;
using System.Collections.Generic;

	/*
	 * This class is called once per frame. It is the Behaviour of the AntHill
	 * 
	 * @author: Lukas Krose
	 * @version: 1.1
	 */
namespace AntHill{
	public class AntHillBehaviour : MonoBehaviour {

		private AntHill hill;
		public int foodCount = 500;
		public Rigidbody ant;
		public string intialConf;
		public string baseConf;
		public string impactConf;
		public string defaultConf;
		public int antCost;

		/*
		 * This function is called upon initialization of the AntHill
		 * 
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		void Start () {		
			AntHillAIConf conf = new AntHillAIConf (intialConf, baseConf, impactConf, defaultConf);
			AntHillAI ai = new AntHillAI (conf, ant);

			hill = new AntHill (ai, transform.position, transform.rotation, foodCount, antCost);
		}
		
		/*
		 * This function is called once per frame. Redirects all information to the relevant classes.
		 * This function also updates the foodcount.
		 * 
		 * @ToDo: Extend AI
		 * 
		 * @author: Lukas Krose
		 * @version: 1.1
		 */
		void Update () {

			foodCount = hill.getFoodCount ();
			hill.Update ();
		}



		/*
		 * This function is called when the hill encounters a collision. (e.g. an ant enters the collider)
		 * 
		 * @param: Collider other The object that walked in the colldier
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		void OnTriggerEnter(Collider other) {
			hill.handleAntInBase (other);
		}

		/*
		 * This function is called every frame if a object is in the collider (e.g. the ant is in the base)
		 *
		 * @param: Collider other The object that walked in the colldier
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		void OnTriggerStay(Collider other) {
			hill.handleAntInBase (other);
		}
	}
}
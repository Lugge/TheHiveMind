using UnityEngine;
using System.Collections;

namespace AntHill
{
	/*
	 * Beaviour class for ants, is called once per frame by unity
	 * 
	 * @author: Lukas Krose
	 * @version: 1.0
	 */
	public class AntBehaviour : MonoBehaviour {

		//Variables that can be defined in the unity environmnet
		public float speed;
		public int maxMovements;
		public float maxTrvl;
		public int carryCap;
		public int foodSupplies;
		private Ant ant;


		private Vector3 nextMovementTarget;
		private float startTime;


		/*
		 * Creates and initializes an new ant
		 * 
		 * @param: strin type The type of the ant
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		public void init(string type) {
			AntProperties p = new AntProperties ();
			p.maxTrvl = maxTrvl;
			p.carryCapability = carryCap;
			p.maxMovements = maxMovements;
			//int maxMvnts, float maxT
			switch (type)
			{
			case "Searcher":
				ant = new SearcherAnt();
				break;
			case "Worker":
				ant = new WorkerAnt();
				break;
			default:
				Debug.Log("Type not found!");
				break;
			}

			ant.init (type, p, transform.position);

			nextMovementTarget = transform.position;
		}

		public Ant getAnt(){
			return ant;
		}

				
		/*
		 * This function is called once per frame by unity.
		 * It asks the ant for the next position to go to and then travels the Vector
		 * 
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		void Update () {
			ant.updatePosition (transform.position);
			foodSupplies = ant.getSuppliesLeft ();

			Vector3 newMovementTarget = ant.getNextMovement();
			if (newMovementTarget != nextMovementTarget) {
				startTime = Time.time;
				nextMovementTarget = newMovementTarget;
			}

			//transform.position = Vector3.Lerp(transform.position, nextMovementTarget, (Time.time - startTime) * speed);
			transform.position = Vector3.MoveTowards(transform.position, nextMovementTarget, (Time.time - startTime) * speed);
		}

		/*
		 * This class is called if the ant entcounters a collision
		 * 
		 * @author: Lukas Krose
		 * @since: 1.0
		 */
		void OnTriggerEnter(Collider other) {
			ant.handleCollissionEnter(other);
		}

		void OnTriggerExit(Collider other) {
			ant.handleCollissionExit(other);
		}
	}
}

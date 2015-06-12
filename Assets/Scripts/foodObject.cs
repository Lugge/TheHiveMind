using UnityEngine;
using System.Collections;

namespace AntHill{
	/*
	 * This class is called once per frame. It is the behaviour of the food
	 * (Note: only needed to hold the food points of the food)
	 * 
	 * @author: Lukas Krose
	 * @version: 1.0
	 */
	public class foodObject : MonoBehaviour {

		public int foodPoints;
		private int currentFood;


		/*
		 * This function is called upon initialization of the AntHill.
		 * It sets the current food points provided by the editor.
		 * 
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		void Start () {
			currentFood = foodPoints;
		}

		/*
		 * Returns if the food pile has food left.
		 * 
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public bool hasFoodLeft() {
			return currentFood > 0;
		}

		/*
		 * Lowers the food by the given parameter
		 * 
		 * @param int amount The amount the food pile should be lowered
		 * 
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public void lowerFoodPoints(int amount){
			currentFood = currentFood - amount;
			foodPoints = currentFood - amount;
		}
	}
}

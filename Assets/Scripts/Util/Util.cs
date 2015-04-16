using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
namespace AntHill
{
	/*
	 * Utility class
	 * 
	 * @author: Lukas Krose
	 * @version: 1.0
	 */
	public static class Util
	{
		/*
		 * Checks if the path between two positions is valid (e.g. no obstacle in the way)
		 * 
		 * @ToDo: Return false if position is too far away?
		 * 
		 * @param: Vector3 position The TO position
		 * @param: Vector3 currentposition The FROM position
		 * @return: bool True if the path between two positions is valdi
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public static bool isValid(Vector3 position, Vector3 currentPosition) {
			Vector3 direction = ( position - currentPosition).normalized;
			RaycastHit[] hits;
			hits = Physics.RaycastAll(currentPosition, direction, 100.0F);
			int i = 0;

			while (i < hits.Length) {
				RaycastHit hit = hits[i];
				if(hit.transform.gameObject.tag == "Obstacle"){
					return false;
				}
				i++;
			}

			return true;
		}
	}
}


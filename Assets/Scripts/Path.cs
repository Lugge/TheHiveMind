using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;

namespace AntHill
{
	/*
	 * This class holds all information to a given path
	 * 
	 * @author: Lukas Krose
	 * @version: 1.0
	 */
	public class Path
	{
		public int metric = 0;
		public List<Vector3> path = new List<Vector3>();

		public Path ()
		{
		}

		/*
		 * Adds a Vector to the path array and increases the metric
		 * 
		 * @param: Vector3 movement The Vector to be saved
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public void addMovement(Vector3 movement){
			path.Add (movement);
			metric++;
		}

		/*
		 * Returns the movement at the given step
		 * 
		 * @param: int step The number of the step (pos in the array)
		 * @return: Vector3 The desired step
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public Vector3 getMovement(int step){
			return path [step];
		}
	}
}


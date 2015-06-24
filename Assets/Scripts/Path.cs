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

		public Path (Path p)
		{
			path = new List<Vector3>();
			foreach (Vector3 v in p.path) {
				path.Add(v);
			}
			metric = p.metric;
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

		public Vector3 getStepObject(Vector3 pos){
			for (int c = 0; c< path.Count; c++) {
				if(path[c] == pos) {
					return path[c];
				}
			}
			return new Vector3();
		}

		public void debugPath() {
			foreach (Vector3 v in path) {
				Debug.Log (v);
			}
		}


	}
}


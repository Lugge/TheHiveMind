using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace AntHill
{
	/*
	 * Information class. Holds all information that can be excanged between Hill and ant.
	 * 
	 * @ToDo: Maybe refactor this to anthill memory
	 * 
	 * @author: Lukas Krose
	 * @version: 1.0
	 */
	public class Information
	{
		public List<Food> knownFood = new List<Food>();
		public Vector3 antHillPosition;
	}
}

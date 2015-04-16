using System.Collections;
using System;
using UnityEngine;

namespace AntHill
{
	/*
	 * This class contains all information of a food pile
	 * 
	 * @ToDo: Refactoring
	 * 
	 * @author: Lukas Krose
	 * @version: 1.0
	 */
	public class Food
	{
		public Path path;
		public bool isEmpty = false;
		public GameObject foodObject;
	}
}
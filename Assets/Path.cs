//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.18444
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------
using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;

namespace AntHill
{
	public class Path
	{
		public int metric = 0;
		public List<Vector3> path = new List<Vector3>();

		public Path ()
		{
		}

		public void addMovement(Vector3 movement){
			path.Add (movement);
			metric++;
		}

		public Vector3 getMovement(int step){
			return path [step];
		}
	}
}


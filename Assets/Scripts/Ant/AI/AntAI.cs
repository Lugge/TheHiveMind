using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AntHill
{
	/*
	 * AI class for the ant.
	 * Currently only indicates if the ant is idle
	 * 
	 * @ToDo: Develop an AI for the ant?
	 * 
	 * @author: Lukas Krose
	 * @version: 1.0
	 */
	public class AntAI
	{
		private bool isIdle = true;

		/*
		 * Changes the idle state of the ant
		 * 
		 * @param: bool idle True if the ant shoul be idle
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public void setIdle(bool idle){
			isIdle = idle;
		}

		/*
		 * Returns if the ant is idle
		 * 
		 * @return: bool True if the ant is idle
		 * @author: Lukas Krose
		 * @version: 1.0
		 */
		public bool idle(){
			return isIdle;
		}

	}
}


using System;
using UnityEngine;
using System.Collections;

namespace AntHill
{
	public interface InterfaceAI
	{
		void init(string type, int maxMvnts, float maxT, Information info);
		Vector3 getNextMovement();
		void handleCollission(Collider other);
		void reset();
		void supply();
		//void handleEvent(String EventDesc);
		//bool isReturnedHome();
		void setIdle(bool idle);
		bool hasReachedNextPosition();
		void instructAnt(string type, Information info);
		AntMemory getMemory();
		void updatePosition(Vector3 position);
		bool idle();
		bool isAtHomeBase();
		bool wantsToCommunicate();
		void setTask(Task task);
	}

}


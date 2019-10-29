using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Emmanuel
{
	public class GameObjectList : ScriptableObject
	{
		public GameObjectList(List<GameObject> goList)
		{
			GameObjects = goList;
		}
		
		public GameObjectList(params GameObject[] gameObjects)
		{
			foreach (GameObject go in gameObjects)
			{
				GameObjects.Add(go);
			}
		}

		private List<GameObjectList> gameObjectList;

		public List<GameObject> GameObjects { get; set; }


		public void Clear()
		{
			GameObjects.Clear();
		}

		public void Add(GameObject go)
		{
			GameObjects.Add(go);
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using Lodis.GamePlay.GridScripts;
using UnityEngine;

namespace Lodis.GamePlay.AIFolder
{
	public class AISpawnBehaviour : MonoBehaviour
	{
		private AIMovementBehaviour _moveScript;
		[SerializeField]
		private PanelBehaviour _testGoal;
		private PlayerSpawnBehaviour _spawnScript;
		// Use this for initialization
		void Start()
		{
			
		}

		private void Awake()
		{
			
		}

		public void SelectBlock(int choice)
		{
			_moveScript = GetComponent<AIMovementBehaviour>();
			_spawnScript = GetComponent<PlayerSpawnBehaviour>();
			switch (choice)
			{
				case 0:
				{
					_spawnScript.SelectBlock0();
					break;
				}
				case 1:
				{
					_spawnScript.SelectBlock1();
					break;
				}
				case 2:
				{
					_spawnScript.SelectBlock2();
					break;
				}
				case 3:
				{
					_spawnScript.SelectBlock3();
					break;
				}
			}
		}
		public void Build()
		{
			SelectBlock(0);
			_spawnScript.FindNeighbors();
			_spawnScript.PlaceBlockLeft();
		}
		// Update is called once per frame
		void Update()
		{
		}
	}
}

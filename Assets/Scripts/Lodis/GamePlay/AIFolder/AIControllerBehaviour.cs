using System;
using Lodis.GamePlay.GridScripts;
using UnityEngine;
using VariableScripts;

namespace Lodis.GamePlay.AIFolder
{
	public class AIControllerBehaviour : MonoBehaviour
	{
		private BinaryTreeBehaviour _decisionTree;
		private AIMovementBehaviour _moveScript;
		private void Start()
		{
			_decisionTree = GetComponent<BinaryTreeBehaviour>();
			_moveScript = GetComponent<AIMovementBehaviour>();
		}

		public void ActivateDefenseMode()
		{
			_decisionTree.Decisions.SetCondition("Defend", true);
		}
    
		public void ActivateAttackMode()
		{
			_decisionTree.Decisions.SetCondition("Defend", false);
		}
		private void Update()
		{
			if (_moveScript.EnemyBulletList.Objects.Count >0)
			{
				ActivateDefenseMode(); 
			}
			else
			{
				ActivateAttackMode();
			}
		}
	}
}

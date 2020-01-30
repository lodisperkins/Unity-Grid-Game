using Lodis.GamePlay.GridScripts;
using UnityEngine;
using VariableScripts;

namespace Lodis.GamePlay.AIFolder
{
	public class AIControllerBehaviour : MonoBehaviour
	{

		[SerializeField] private BinaryTree decisions;
			// Use this for initialization
		void Start ()
		{
			
		}

		public void ActivateDefenseMode()
		{
			decisions.SetCondition("Defend", true);
		}

		public void ActivateAttackMode()
		{
			decisions.SetCondition("Defend", false);
		}
		// Update is called once per frame
		void Update () {
			if (GridBehaviour.bulletListP1.Objects.Count > 0)
			{
				ActivateDefenseMode();
			}
			else
			{
				ActivateAttackMode();
			}
			decisions.TraverseTree();
		}
	}
}

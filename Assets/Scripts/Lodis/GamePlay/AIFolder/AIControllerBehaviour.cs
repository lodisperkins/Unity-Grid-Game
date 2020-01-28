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
				StartCoroutine(decisions.TraverseTree());
			}
		
		// Update is called once per frame
		void Update () {
			
		}
	}
}

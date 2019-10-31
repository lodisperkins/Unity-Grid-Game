using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationBehaviour : MonoBehaviour
{
	//the animator attached to this gameobject
	[SerializeField] private Animator _animator;
	
	public void EnableMoveAnimation()
	{
		if(_animator != null)
			_animator.SetBool("IsMoving",true);
	}

	public void DisableMoveAnimation()
	{
		if(_animator != null)
			_animator.SetBool("IsMoving",false);
	}
}

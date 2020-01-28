using System.Collections;
using System.Collections.Generic;
using Lodis.GamePlay.GridScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace VariableScripts
{
	[CreateAssetMenu(menuName = "AI/Node")]
	public class Node : ScriptableObject
	{
		public Node parent;
		private Node _childLeft;
		private Node _childRight;
		public UnityEvent actions;
		private bool _leftTriggerSet;
		private bool _rightTriggerSet;
		public string rightTrigger;
		public string leftTrigger;
		public bool LeftTriggerSet
		{
			get { return _leftTriggerSet; }
		}

		public Node ChildLeft
		{
			get { return _childLeft; }
			set
			{
				_childLeft = value;
				_childLeft.parent = this;
			}
		}

		public Node ChildRight
		{
			get { return _childRight; }
			set
			{
				_childRight = value;
				_childRight.parent = this;
			}
		}

		public bool RightTriggerSet
		{
			get { return _rightTriggerSet; }
		}
		public void SetTriggerRight()
		{
			_rightTriggerSet = true;
		}
		public void SetTriggerLeft()
		{
			_leftTriggerSet = true;
		}

		public void ResetTriggers()
		{
			_rightTriggerSet = false;
			_leftTriggerSet = false;
		}
		public Node(Node parent = null, Node childLeft = null, Node childRight = null)
		{
			this.parent = parent;
			_childLeft = childLeft;
			_childRight = childRight; 
		}
	}
}


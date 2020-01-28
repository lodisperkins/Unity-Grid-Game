using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VariableScripts
{
	public class Node
	{
		public Node Parent;
		public Node ChildLeft;
		public Node ChildRight;
		
		public Node(Node parent = null, Node childLeft = null, Node childRight = null)
		{
			Parent = parent;
			ChildLeft = childLeft;
			ChildRight = childRight;
		}
	}

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VariableScripts
{
    [CreateAssetMenu(menuName = "AI/BinaryTree")]
    public class BinaryTree : ScriptableObject
    {
        public List<Node> nodes;
        public Node root;
        public Node currentNode;
        public float decisionDelay;
        public void SetCondition(string conditionName, bool value)
        {
            foreach (Node node in nodes)
            {
                if (node.conditionName == conditionName)
                {
                    node.ConditionMet = value;
                }
            }
        }
        public void TraverseTree()
        {
            currentNode = nodes[0];
            for (int i = 0; i < nodes.Count;)
            {
                currentNode.actions.Invoke();
                if (currentNode.HasChildren())
                {
                    if (currentNode.ConditionMet)
                    {
                        currentNode = currentNode.ChildRight;
                        i++;
                    }
                    else 
                    {
                        currentNode = currentNode.ChildLeft;
                        i++;
                    }
                }
                else
                {
                    break;
                }
            }
        }
    }
}

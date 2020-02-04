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
        
    }
}

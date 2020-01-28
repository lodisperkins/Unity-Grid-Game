using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VariableScripts
{
    [CreateAssetMenu(menuName = "AI/StateMachine")]
    public class BinaryTree : ScriptableObject
    {
        public List<Node> nodes;
        public Node root;
        public Node currentNode;

        public void SetTrigger(string triggerName)
        {
            foreach (Node node in nodes)
            {
                if (node.leftTrigger == triggerName)
                {
                    node.SetTriggerLeft();
                }
                else if(node.rightTrigger == triggerName)
                {
                    node.SetTriggerRight();
                }
            }
        }
        public IEnumerator TraverseTree()
        {
            currentNode = nodes[0];
            for (int i = 0; i < nodes.Count;)
            {
                if (currentNode != null)
                {
                    currentNode.actions.Invoke();
                    if (currentNode.RightTriggerSet)
                    {
                        currentNode = currentNode.ChildRight;
                        i++;
                    }
                    else if (currentNode.LeftTriggerSet)
                    {
                        currentNode = currentNode.ChildLeft;
                        i++;
                    }
                }
                else
                {
                    ResetTriggers();
                    break;
                }
            }
            yield return new WaitForSeconds(0);
        }

        public void ResetTriggers()
        {
            foreach (Node node in nodes)
            {
                node.ResetTriggers();
            }
        }
    }
}

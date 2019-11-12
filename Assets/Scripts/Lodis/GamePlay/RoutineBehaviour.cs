using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Lodis.GamePlay
{
    public class RoutineBehaviour : MonoBehaviour
    {
        //Holds the list of actions that will be done by the gameobject
        [FormerlySerializedAs("OnActionsBegin")] [SerializeField]
        private Lodis.Event onActionsBegin;
        [FormerlySerializedAs("OnActionsCompleted")] [SerializeField]
        private Lodis.Event onActionsCompleted;
        //Time it takes to invoke the actions event
        [FormerlySerializedAs("action_delay")] [SerializeField]
        private float actionDelay;
        //Number of times the actions event will be invoked
        [FormerlySerializedAs("action_limit")] [SerializeField]
        private int actionLimit;
        [SerializeField]
        private bool hasLimit;

        private bool _isOnActionsBeginNotNull;
        private bool _isOnActionsCompletedNotNull;
        private void OnEnable()
        {
            _isOnActionsBeginNotNull = onActionsBegin != null;
            _isOnActionsCompletedNotNull = onActionsCompleted != null;
            StartCoroutine(PerformActions());
        }

        public void ResetActions()
        {
            StopAllCoroutines();
            StartCoroutine(PerformActions());
        }
        private IEnumerator PerformActions()
        {
            for (var i = 0; i < actionLimit; i++)
            {
                if (_isOnActionsBeginNotNull)
                {
                    onActionsBegin.Raise(gameObject);
                }
                yield return new WaitForSeconds(actionDelay);
            }
            if (_isOnActionsCompletedNotNull)
            {
                onActionsCompleted.Raise(gameObject);
            }
        }
    }
}

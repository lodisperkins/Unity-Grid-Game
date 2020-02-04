using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Lodis
{
    delegate void Actions();
    public class GameEventListener:MonoBehaviour,IListener
    {
        //Delegate containing all functions to used when the event is invoked
        [SerializeField]
        UnityEvent actions;
        //the event the gameobject should be listening for
        
        public Lodis.Event Event;
        //The sender the gameobject is waiting for the event to be raiased by
        public GameObject intendedSender;
        // Use this for initialization
        void Start()
        {
            Event.AddListener(this);
        }
        //Invokes the actions delegate
        public void Invoke(Object Sender)
        {
            if(intendedSender == null)
            {
                actions.Invoke();
                return;
            }
            else if(intendedSender == Sender)
            {
                actions.Invoke();
                return;
            }
        }
    }
}

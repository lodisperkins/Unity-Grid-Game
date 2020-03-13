using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Lodis
{
    public class SceneTransferBehaviour : MonoBehaviour {
        [SerializeField] private Event onSelected;
        [SerializeField] private Event onSelectedP2;
        [SerializeField] private Event onCancelP1;
        [SerializeField] private Event onCancelP2;
        private bool p1IsReady;
        private bool p2IsReady;
        // Use this for initialization
        void Start () {
		
	    }
	
	    // Update is called once per frame
	    void Update () {
		    if(Input.GetButtonDown("ReadyUp1"))
            {
                onSelected.Raise(gameObject);
                p1IsReady = true;
            }
            else if(Input.GetButtonDown("Cancel"))
            {
                onCancelP1.Raise(gameObject);
                p1IsReady = false;
            }
            if(Input.GetButtonDown("ReadyUp2"))
            {
                onSelectedP2.Raise(gameObject);
                p2IsReady = true;
            }
            else if(Input.GetButtonDown("Cancel2"))
            {
                onCancelP2.Raise(gameObject);
                p2IsReady = false;
            }
            if (p1IsReady && p2IsReady)
            {
                SceneManager.LoadScene("BattleScene");
            }
        }
    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodis.GamePlay.GridScripts;
namespace Lodis.Movement
{
    [RequireComponent(typeof(SeekBehaviour),typeof(Rigidbody))]
    public class GridPhysicsBehaviour : MonoBehaviour
    {
        private PanelBehaviour trailingPanel;
        public PanelBehaviour currentPanel;
        private SeekBehaviour seekScript;
        private Rigidbody rigidbody;
        private Vector3 heightOffset;
        private Vector2 currentVelocity;
        private PanelBehaviour targetPanel;
        // Use this for initialization
        void Start()
        {
            seekScript = GetComponent<SeekBehaviour>();
            rigidbody = GetComponent<Rigidbody>();
            currentVelocity = new Vector2(0, 0);
            heightOffset = new Vector3(0, transform.position.y, 0);
        }
        public void AddForce(Vector2 force)
        {
            currentVelocity = force;
            Vector2 destination = currentPanel.Position + force;
            destination.Set((int)Mathf.Clamp(destination.x,0,9),(int) Mathf.Clamp(destination.y,0,3));
            targetPanel = BlackBoard.grid.GetPanelFromGlobalList(destination);
            seekScript.Init(targetPanel.transform.position + heightOffset, rigidbody.velocity, (int)currentVelocity.magnitude, 0.1f, true, false);
            seekScript.seekEnabled = true;
        }
        public void OnTriggerEnter(Collider other)
        {
            
            if(seekScript.seekEnabled)
            {
                if(other.CompareTag("Panel") && other != currentPanel.gameObject)
                {
                    trailingPanel = currentPanel;
                    currentPanel = other.GetComponent<PanelBehaviour>();
                }
                switch (other.tag)
                {
                    case "Block":
                        {
                            seekScript.seekEnabled = false;
                            HealthBehaviour healthScript = other.GetComponent<HealthBehaviour>();
                            healthScript.takeDamage((int)currentVelocity.magnitude);
                            targetPanel = trailingPanel;
                            seekScript.Init(targetPanel.transform.position+ heightOffset, rigidbody.velocity, (int)currentVelocity.magnitude, 0.1f, true, false);
                            seekScript.seekEnabled = true;
                            currentPanel = trailingPanel;
                            break;
                        }
                    case "Player":
                        {
                            seekScript.seekEnabled = false;
                            HealthBehaviour healthScript = other.GetComponent<HealthBehaviour>();
                            healthScript.takeDamage((int)currentVelocity.magnitude);
                            targetPanel = trailingPanel;
                            seekScript.Init(targetPanel.transform.position + heightOffset, rigidbody.velocity, (int)currentVelocity.magnitude, 0.1f, true, false);
                            seekScript.seekEnabled = true;
                            currentPanel = trailingPanel;
                            break;
                        }
                }

            }
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}



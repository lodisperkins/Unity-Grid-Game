using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Lodis.GamePlay
{
    public class BombBlockBehaviour : MonoBehaviour {

        [SerializeField]
        private int _damageVal;
        [SerializeField]
        private BlockBehaviour _block;
        private SphereCollider _explosionRadius;
        [SerializeField]
        private Text _timerText;
        [SerializeField]
        private RoutineBehaviour _routineScript;
        bool canExplode;
	    // Use this for initialization
	    void Start () {
            _explosionRadius = GetComponent<SphereCollider>();
            canExplode = true;
            if (_block == null)
            {
                GetComponent<Rigidbody>().isKinematic = true;
                canExplode = false;
            }
        }
	    public void Explode()
        {
            if(_block.Health != null && canExplode)
            { _explosionRadius.enabled = true;
                _block.Health.playDeathParticleSystems(.5f);
                _block.DestroyBlock(.1f);
            }
            
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Panel"))
            {
                if(other.gameObject == _block.currentPanel && _explosionRadius.enabled)
                {
                    other.GetComponent<GridScripts.PanelBehaviour>().Occupied = false;
                }
                other.GetComponent<GridScripts.PanelBehaviour>().BreakPanel(5);
            }
            else if(other.gameObject != _block.gameObject)
            {
                var health = other.GetComponent<HealthBehaviour>();
                if(health != null)
                {
                    health.takeDamage(_damageVal);
                }
            }
        }
        // Update is called once per frame
        void Update () {
            _timerText.text = (_routineScript.numberOfActionsLeft + 1).ToString();
	    }
    }
}



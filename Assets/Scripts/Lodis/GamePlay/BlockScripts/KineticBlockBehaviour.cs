using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Lodis
{


    public class KineticBlockBehaviour : MonoBehaviour {
        private List<Rigidbody> _rigidbodies;
        private List<BulletBehaviour> _bullets;
        private List<Vector3> velocityVals;
        [SerializeField]
        private BlockBehaviour block;
        [SerializeField]
        private GameEventListener _eventListener;
        public int bulletCapacity;
	    // Use this for initialization
	    void Start() 
        {
            _rigidbodies = new List<Rigidbody>();
            _bullets = new List<BulletBehaviour>();
            velocityVals = new List<Vector3>();
            _eventListener.intendedSender = block.owner;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Projectile"))
            {
                _bullets.Add(other.GetComponent<BulletBehaviour>());
                Rigidbody temp = other.GetComponent<Rigidbody>();
                velocityVals.Add(temp.velocity);
                _rigidbodies.Add(temp);
                temp.velocity = Vector3.zero;
            }
        }
        public void DetonateBlock()
        {
            block.DestroyBlock(0);
            for (int i = 0; i < _rigidbodies.Count; i++)
            {
                if (_rigidbodies[i] != null)
                {
                    _bullets[i].Owner = block.owner.name;
                    _rigidbodies[i].AddForce(-(velocityVals[i]) *2, ForceMode.Impulse); 
                }
            }
        }
        // Update is called once per frame
        void Update() {
            if (_rigidbodies.Count >= bulletCapacity)
            {
                block.DestroyBlock(1);
            }
        }
    }
}

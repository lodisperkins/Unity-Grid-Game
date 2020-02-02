using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Lodis
{


    public class KineticBlockBehaviour : MonoBehaviour {
        List<Rigidbody> bullets;
        List<Vector3> velocityVals;
        [SerializeField]
        HealthBehaviour block;
	// Use this for initialization
	void Start() {
            bullets = new List<Rigidbody>();
            velocityVals = new List<Vector3>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Projectile"))
            {
                Rigidbody temp = other.GetComponent<Rigidbody>();
                velocityVals.Add(temp.velocity);
                bullets.Add(temp);
                temp.velocity = Vector3.zero;
            }
        }
        private void OnDestroy()
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].AddForce(-(velocityVals[i]), ForceMode.Impulse);
            }
        }
        // Update is called once per frame
        void Update() {
            if (bullets.Count > 10)
            {
                block.DestroyBlock(1);
            }
        }
    }
}

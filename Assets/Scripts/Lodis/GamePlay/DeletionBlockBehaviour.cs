using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Lodis
{
    public class DeletionBlockBehaviour : MonoBehaviour
    {
        //temporary gameobject used to delete the deletion block without deleting the prefab
        private GameObject TempObject;
        //particles to be played when a block is deleted
        [SerializeField] private ParticleSystem ps;
        private void Start()
        {
            TempObject = gameObject;
            GetComponent<BlockBehaviour>().DestroyBlock(.5f);
        }
        //plays the particles when a block is deleted for a spcified duration
        public void playDeathParticleSystems(float duration)
        {
            var tempPs = Instantiate(ps,transform.position,transform.rotation);
            tempPs.Play();
            tempPs.playbackSpeed = 2.0f;
            Destroy(tempPs, duration);
        }
        private void OnTriggerEnter(Collider other)
        {
            var block = other.GetComponent<BlockBehaviour>();
            if (block != null)
            {
                playDeathParticleSystems(1.5f);
                block.DestroyBlock(1.0f);
            }
        }
    }
}

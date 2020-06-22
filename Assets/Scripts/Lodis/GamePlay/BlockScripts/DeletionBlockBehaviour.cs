using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Lodis
{
    //deletion blocks are invisible blocks used to delete others. They are only selected while
    //the player is in the deletion state.
    public class DeletionBlockBehaviour : MonoBehaviour,IUpgradable
    {
        private PlayerSpawnBehaviour _player;
        [SerializeField] private Event _onDelete;
        private BlockBehaviour _deletionBlock;
        //particles to be played when a block is deleted
        [SerializeField] private ParticleSystem ps;

        public BlockBehaviour block
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
                throw new System.NotImplementedException();
            }
        }

        GameObject IUpgradable.specialFeature
        {
            get
            {
                return null;
            }
        }

        public string Name
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public Color displayColor
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
                throw new System.NotImplementedException();
            }
        }

        public bool CanBeHeld
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public GameObject specialFeature;

        private void Start()
        {
            _deletionBlock = GetComponent<BlockBehaviour>();
            _player = _deletionBlock.owner.GetComponent<PlayerSpawnBehaviour>();
            _deletionBlock.DestroyBlock(.5f);
        }
        //plays the particles when a block is deleted for a spcified duration
        public void PlayParticleSystems(float duration)
        {
            var tempPs = Instantiate(ps,transform.position,transform.rotation);
            tempPs.Play();
            tempPs.playbackSpeed = 2.0f;
            Destroy(tempPs, duration);
        }
        //Refunds the player half of the energy used to build the block
        private void GetRefund(BlockBehaviour block)
        {
            _player.AddMaterials(block.cost /2);
        }
        public void UpgradeBlock(GameObject otherBlock)
        {
            throw new System.NotImplementedException();
        }

        public void TransferOwner(GameObject otherBlock)
        {
            throw new System.NotImplementedException();
        }

        public void ResolveCollision(GameObject collision)
        {
            var block = collision.GetComponent<BlockBehaviour>();
            if (block != null && !block.deleting &&block.canDelete)
            {
                block.deleting = true;
                PlayParticleSystems(1.5f);
                block.DestroyBlock(1.0f);
                _onDelete.Raise();
            }
        }

        public void ActivateDisplayMode()
        {
            throw new System.NotImplementedException();
        }

        public void UpgradePlayer(PlayerAttackBehaviour player)
        {
            throw new System.NotImplementedException();
        }

        public void ActivatePowerUp()
        {
            throw new System.NotImplementedException();
        }

        public void DetachFromPlayer()
        {
            throw new System.NotImplementedException();
        }

        public void Stun()
        {
            throw new System.NotImplementedException();
        }

        public void Unstun()
        {
            throw new System.NotImplementedException();
        }

        public void DeactivatePowerUp()
        {
            throw new System.NotImplementedException();
        }
    }
}

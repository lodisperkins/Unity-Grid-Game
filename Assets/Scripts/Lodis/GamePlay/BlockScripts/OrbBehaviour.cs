using System;
using UnityEngine;

namespace Lodis.GamePlay.BlockScripts
{
    public class OrbBehaviour : MonoBehaviour
    {

        public BlockBehaviour block;
        [SerializeField] private int DamageVal;

        private void OnTriggerEnter(Collider other)
        {
            ResolveCollision(other.gameObject);
        }

        private void ResolveCollision(GameObject other)
        {
            switch (other.tag)
            {
                case "Block":
                {
                    var health = other.GetComponent<HealthBehaviour>();
                    if (health != null)
                    {
                        other.GetComponent<BlockBehaviour>().GiveMoneyForKill(block.owner.name,DamageVal);
                        health.takeDamage(DamageVal);
                    }
                    break;
                }
                default:
                {
                    var health = other.GetComponent<HealthBehaviour>();
                    if (health != null && other.name != block.owner.name)
                    {
                        health.takeDamage(DamageVal);
                    }
                    break;
                }
            }
        }
    }
}

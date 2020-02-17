﻿using System;
using UnityEngine;

namespace Lodis.GamePlay.BlockScripts
{
    public class OrbBehaviour : MonoBehaviour
    {

        public BlockBehaviour block;
        [SerializeField] public int DamageVal;

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
                    if(other == block.gameObject)
                    {
                        break;
                    }
                    var health = other.GetComponent<HealthBehaviour>();
                    if (health != null)
                    {
                        other.GetComponent<BlockBehaviour>().GiveMoneyForKill(block.owner.name,DamageVal);
                        health.takeDamage(DamageVal);
                    }
                    break;
                }
                case "Player":
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
        private void Update()
        {
            if(block == null)
            {
                GameObject temp = gameObject;
                Destroy(temp);
            }
        }
    }
}

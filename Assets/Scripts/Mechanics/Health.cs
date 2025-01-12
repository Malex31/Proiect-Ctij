using System;
using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    /// <summary>
    /// Represebts the current vital statistics of some game entity.
    /// </summary>
    public class Health : MonoBehaviour
    {
        /// <summary>
        /// The maximum hit points for the entity.
        /// </summary>
        public int maxHP = 5;


        public bool IsAlive => currentHP > 0;

        int currentHP;
        private PlayerController playerController;

        public int CurrentHP => currentHP;

        public void ResetHealth()
        {
            currentHP = maxHP;
        }

        /// <summary>
        /// Increment the HP of the entity.
        /// </summary>
        public void Increment()
        {
            currentHP = Mathf.Clamp(currentHP + 1, 0, maxHP);
        }

        /// <summary>
        /// Decrement the HP of the entity. Will trigger a HealthIsZero event when
        /// current HP reaches 0.
        /// </summary>
        public void Decrement()
        {
            currentHP = Mathf.Clamp(currentHP - 1, 0, maxHP);
            if (currentHP == 0)
            {
                var ev = Schedule<HealthIsZero>();
                ev.health = this;
            }
        }

        /// <summary>
        /// Decrement the HP of the entitiy until HP reaches 0.
        /// </summary>
        public void Die()
        {
            // 
            if (PlayerController.lastCheckPointPos != Vector2.zero)
            {
                
                transform.position = PlayerController.lastCheckPointPos;
                Debug.Log($"Player respawned at: {transform.position}");
            }
            else
            {
                Debug.LogError("No checkpoint set, player is at the default position.");
                transform.position = new Vector2(0, 0);
            }
            ResetHealth();
            if (playerController != null)
            {
                playerController.Respawn();
            }
            //while (currentHP > 0) Decrement();
        }

            void Awake()
             {
                playerController = GetComponent<PlayerController>();

                currentHP = maxHP;
            }

        public void TakeDamage(int _damage)
        {
            currentHP = Mathf.Clamp(currentHP - _damage, 0, maxHP);
        }
    }
}

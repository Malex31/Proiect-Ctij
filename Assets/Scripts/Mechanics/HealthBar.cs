using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer.Mechanics
{


    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health playerHealth;
        [SerializeField] private Image totalHealthBar;
        [SerializeField] private Image currentHealthBar;

        private void Start()
        {

            totalHealthBar.fillAmount = 1f;
            UpdateHealthBar();
        }

        private void Update()
        {

            UpdateHealthBar();
        }

        private void UpdateHealthBar()
        {

            currentHealthBar.fillAmount = (float)playerHealth.CurrentHP / playerHealth.maxHP;
        }
    }
}

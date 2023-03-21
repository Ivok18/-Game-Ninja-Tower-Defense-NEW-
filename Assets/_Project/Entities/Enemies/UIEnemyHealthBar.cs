using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TD.Entities.Enemies
{
    public class UIEnemyHealthBar : MonoBehaviour
    {
        public Transform Bar;
        public Transform BarFill;
        public float CurrentValue;
        public float MaxValue;
        private HealthBehaviour healthBehaviour;
        private AlmostDeadSignaler almostDeadSignaler;
        
        private void Awake()
        {
            healthBehaviour = GetComponent<HealthBehaviour>();
            almostDeadSignaler = GetComponent<AlmostDeadSignaler>();
        }

        
        void Update()
        {
            CurrentValue = (float)healthBehaviour.CurrentHealth;
            MaxValue = (float)healthBehaviour.MaxHealth;
            CurrentValue = Mathf.Clamp(CurrentValue, 0, MaxValue);

            float localScaleY = Bar.localScale.y;
            float localScaleZ = Bar.localScale.z;
            bool uiEnemyHealthBarDealsWithPositiveValues = CurrentValue > 0 && MaxValue > 0;
            if(!almostDeadSignaler.IsAlmostDead)
            {
                BarFill.gameObject.SetActive(true); ;
                if (uiEnemyHealthBarDealsWithPositiveValues)
                {
                    Bar.localScale = new Vector3(CurrentValue / MaxValue, localScaleY, localScaleZ);
                }
            }
            else
            {
                BarFill.gameObject.SetActive(false);
                Bar.localScale = new Vector3(0, localScaleY, localScaleZ);
            }
           
        }
    }
}
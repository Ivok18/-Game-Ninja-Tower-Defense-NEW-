using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TD.Entities.Enemies
{
    public class UIEnemyHealthPositionSetter : MonoBehaviour
    {
        private EnemyMovement enemyMovement;
        private AlmostDeadSignaler almostDeadSignaler;
        [SerializeField] private Transform healthIconUp;
        [SerializeField] private Transform healthIconLeft;
        [SerializeField] private Transform skullIcon;
  

        private void Awake()
        { 
            enemyMovement = GetComponent<EnemyMovement>();
            almostDeadSignaler = GetComponent<AlmostDeadSignaler>();
        }

        private void Update()
        {
            if (enemyMovement == null)
                return;


            if(enemyMovement.HasHorizontalDirection)
            {
                if(almostDeadSignaler.IsAlmostDead)
                {
                    skullIcon.gameObject.SetActive(true);
                }
                if(!almostDeadSignaler.IsAlmostDead)
                {
                    healthIconUp.gameObject.SetActive(true);
                    healthIconLeft.gameObject.SetActive(false);
                    skullIcon.gameObject.SetActive(false);
                }
            }

            if(enemyMovement.HasVerticalDirection)
            {
                if (almostDeadSignaler.IsAlmostDead)
                {
                    skullIcon.gameObject.SetActive(true);
                }
                if (!almostDeadSignaler.IsAlmostDead)
                {
                    healthIconUp.gameObject.SetActive(false);
                    healthIconLeft.gameObject.SetActive(true);
                    skullIcon.gameObject.SetActive(false);
                }
            }
        }
    }
}

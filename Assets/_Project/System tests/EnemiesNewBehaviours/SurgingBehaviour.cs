using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TD.Entities.Enemies
{

    public class SurgingBehaviour : MonoBehaviour
    {

        public float SurgingDistance;
        public float SurgingDistanceRemaining;
        public float SurgingSpeed;
        public bool CanSurge;
        public bool CanStartSurge;
        private EnemyMovement enemyMovement;

        private void Awake()
        {
            enemyMovement = GetComponent<EnemyMovement>();

        }

        private void OnEnable()
        {
            EnemyHitDetection.OnEnemyHit += TryDodge;
        }

        private void OnDisable()
        {
            EnemyHitDetection.OnEnemyHit -= TryDodge;
        }

        private void Start()
        {
            SurgingDistanceRemaining = SurgingDistance;
        }

        private void Update()
        {
            if (!CanStartSurge)
                return;

            DodgeAttack();
        }
        public void DodgeAttack()
        {
            if (SurgingDistanceRemaining > 0)
            {
                SurgingDistanceRemaining -= SurgingSpeed * Time.deltaTime;
            }
            else
            {
                enemyMovement.CurrentSpeed = enemyMovement.Speed;
                CanStartSurge = false;
                SurgingDistanceRemaining = SurgingDistance;
            }
        }

        public void TryDodge(Transform enemy, Transform attackingTower)
        {
            if (this.transform != enemy)
                return;

            if (!CanSurge)
                return;

            enemyMovement.CurrentSpeed = SurgingSpeed;
            CanStartSurge = true;
        }
    }
}

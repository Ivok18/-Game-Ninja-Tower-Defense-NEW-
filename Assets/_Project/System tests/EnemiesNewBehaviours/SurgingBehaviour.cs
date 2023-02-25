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
            EnemyHitDetection.OnEnemyHit += TrySurge;
        }

        private void OnDisable()
        {
            EnemyHitDetection.OnEnemyHit -= TrySurge;
        }

        private void Start()
        {
            SurgingDistanceRemaining = SurgingDistance;
        }

        private void Update()
        {
            if (!CanStartSurge)
                return;

            Surge();
        }
        public void Surge()
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

        public void TrySurge(Transform enemy, Transform attackingTower, Vector3 hitPosition)
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

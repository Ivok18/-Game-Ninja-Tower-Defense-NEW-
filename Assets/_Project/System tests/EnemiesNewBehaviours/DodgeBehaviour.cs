using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TD.Entities.Enemies
{
    public class DodgeBehaviour : MonoBehaviour
    {

        public float DodgeDistance;
        public float DodgeDistanceRemaining;
        public float DodgeSpeed;
        public bool CanDodge;
        public bool CanStartDodge;
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
            DodgeDistanceRemaining = DodgeDistance;
        }

        private void Update()
        {
            if (!CanStartDodge)
                return;

            DodgeAttack();
        }
        public void DodgeAttack()
        {
            if(DodgeDistanceRemaining > 0)
            {
                DodgeDistanceRemaining -= DodgeSpeed * Time.deltaTime;
            }
            else
            {
                enemyMovement.CurrentSpeed = enemyMovement.Speed;
                CanStartDodge = false;
                DodgeDistanceRemaining = DodgeDistance;
            }
        }

        public void TryDodge(Transform enemy, Transform attackingTower)
        {
            if (this.transform != enemy)
                return;

            if (!CanDodge)
                return;

            enemyMovement.CurrentSpeed = DodgeSpeed;
            CanStartDodge = true;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using TD.Entities.Towers.States;
using TD.Entities.Towers;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Callbacks;
using TD.StatusSystem;

namespace TD.Entities.Enemies
{
    public class EnemyHitDetection : MonoBehaviour
    {
        private HealthBehaviour healthBehaviour;

        public delegate void EnemyHitCallback(Transform enemy, Transform attackingTower, Vector3 hitPosition);
        public static event EnemyHitCallback OnEnemyHit;

        private void Awake()
        {
            healthBehaviour = GetComponent<HealthBehaviour>();

        }

        //Conditions to trigger hit on enemy:
        // - collision is a tower
        // - the enemy holding this script is its target
        // - he tower is in attack state 
        private void OnTriggerStay2D(Collider2D collision)
        {
            bool hasCollidedWithATower = collision.CompareTag("Tower");
            if (!hasCollidedWithATower)
                return;

            LockTargetState lockTargetState = collision.GetComponent<LockTargetState>();

            if (lockTargetState == null)
                return;

            bool isTargetOfTower = lockTargetState.Target == transform;
            if (!isTargetOfTower)
                return;

            TowerStateSwitcher towerStateSwitcher = collision.GetComponent<TowerStateSwitcher>();
            bool isTowerAttacking = towerStateSwitcher.CurrentTowerState == TowerState.Attacking;
            if (!isTowerAttacking)
                return;

            AttackState attackState = collision.GetComponent<AttackState>();
            if (attackState == null)
                return;
          
            OnEnemyHit?.Invoke(transform, collision.transform, transform.position);
            healthBehaviour.GetDamage(attackState.CurrentDamagePerDash, attackState.transform);
        }      
    }
}
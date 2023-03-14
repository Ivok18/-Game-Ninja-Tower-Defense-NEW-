using System.Collections;
using System.Collections.Generic;
using TD.Entities.Towers;
using TD.Entities.Towers.States;
using UnityEngine;

namespace TD.Entities.Enemies
{
    public class EnemyAssaultDetectionBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform dummyEnemyPrefab;

        public delegate void EnemyDetectedAssaultCallback(Transform targetedEnemy, Transform attackingTower);
        public static event EnemyDetectedAssaultCallback OnEnemyDetectAssault;

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.transform.tag != "Tower")
                return;

            TowerStateSwitcher towerStateSwitcher = collision.GetComponent<TowerStateSwitcher>();
            if (towerStateSwitcher == null)
                return;

            if (towerStateSwitcher.CurrentTowerState != TowerState.Attacking)
                return;

            LockTargetState lockTargetState = collision.GetComponent<LockTargetState>();
            if (lockTargetState == null)
                return;

            if (lockTargetState.Target != transform.parent)
                return;
      
            OnEnemyDetectAssault?.Invoke(transform.parent, collision.transform);
            
        }

    }
}
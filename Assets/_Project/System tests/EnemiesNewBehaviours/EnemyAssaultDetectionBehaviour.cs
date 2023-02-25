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

        public delegate void EnemyDetectedAssaultCallback(Transform targetedEnemy);
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

            DodgeBehaviour dodgeBehaviour = transform.parent.GetComponent<DodgeBehaviour>();
            if(dodgeBehaviour.NoOfAdditionalDodgeRemaining <= 0)
                return;

            //Make the attacking tower believe it has touched its target
            GameObject dummyEnemyGo = Instantiate(dummyEnemyPrefab.gameObject, transform.parent.position, Quaternion.identity);
            DummyDetector dummyDetector = dummyEnemyGo.GetComponent<DummyDetector>();
            dummyDetector.Origin = transform.parent;
            lockTargetState.Target = dummyEnemyGo.transform;
            dodgeBehaviour.NoOfAdditionalDodgeRemaining--;

            OnEnemyDetectAssault?.Invoke(transform.parent);
            

        }

    }
}
using System.Collections;
using System.Collections.Generic;
using TD.Entities.Towers;
using TD.Entities.Towers.States;
using UnityEngine;

namespace TD.Entities.Enemies
{
    public class EnemyAssaultDetectionBehaviour : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
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


            //Make the tower believe it has touched its target
            GameObject dummy = Instantiate(new GameObject(), transform.parent.position, Quaternion.identity);
            dummy.tag = "Enemy";
            lockTargetState.Target = dummy.transform;
        }
    }
}
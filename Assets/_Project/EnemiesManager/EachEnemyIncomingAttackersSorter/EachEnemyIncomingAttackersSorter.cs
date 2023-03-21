using System.Collections;
using System.Collections.Generic;
using TD.EnemiesManager.Storer;
using TD.Entities.Enemies;
using UnityEngine;


namespace TD.EnemiesManager.EachEnemyIncomingAttackersSorter
{
    public class EachEnemyIncomingAttackersSorter : MonoBehaviour
    {
        private void Update()
        {
            foreach(var enemy in EnemyStorer.Instance.Enemies)
            {
                //List all towers on map
                IncomingAttackersSortedList sortedList = enemy.GetComponent<IncomingAttackersSortedList>();

            }
        }
    }
}

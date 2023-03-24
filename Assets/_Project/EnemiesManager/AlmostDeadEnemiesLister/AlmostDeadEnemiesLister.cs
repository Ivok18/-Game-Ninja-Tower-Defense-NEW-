using System.Collections;
using System.Collections.Generic;
using TD.EnemiesManager.Storer;
using TD.Entities.Enemies;
using TD.Entities.Towers;
using TD.Entities.Towers.States;
using TD.TowersManager.Storer;
using UnityEditor;
using UnityEngine;

namespace TD.EnemiesManager.AlmostDeadEnemiesLister
{
    public class AlmostDeadEnemiesLister : MonoBehaviour
    {
        [SerializeField] private List<Transform> almostDeadEnemies;

        private void OnEnable()
        {
            HealthBehaviour.OnEnemyDead += TryRemoveFromList;
        }

        private void OnDisable()
        {
            HealthBehaviour.OnEnemyDead -= TryRemoveFromList;
        }

        // Update is called once per frame
        void Update()
        {
            foreach (var enemy in EnemyStorer.Instance.Enemies)
            {
                //bool isDead = enemy.CompareTag("Dead") ? true : false;
                if (enemy== null)
                    continue;

                HealthBehaviour healthBehaviour = enemy.GetComponent<HealthBehaviour>();
                bool isEnemyAlmostDead = healthBehaviour.CurrentHealth <= 0;
                bool isEnemyInList = Find(enemy) != null ? true : false;
                if (isEnemyAlmostDead && !isEnemyInList)
                {
                    Add(enemy);
                }
                else if (!isEnemyAlmostDead && isEnemyInList)
                {
                    Remove(enemy);
                }

            }         
        }

        public void TryRemoveFromList(Transform enemy, Transform killerTower, float reward)
        {
            if (Find(enemy) == null)
                return;

            Remove(enemy);
        }

        public void Add(Transform enemy)
        {
            AlmostDeadSignaler almostDeadSignaler = enemy.GetComponent<AlmostDeadSignaler>();
            almostDeadSignaler.IsAlmostDead = true;
            almostDeadEnemies.Add(enemy);
        }

        public void Remove(Transform enemy)
        {
            AlmostDeadSignaler almostDeadSignaler = enemy.GetComponent<AlmostDeadSignaler>();
            almostDeadSignaler.IsAlmostDead = false;
            almostDeadEnemies.Remove(enemy);
        }

        public Transform Find(Transform enemy)
        {
            foreach(var almostDeadEnemy in almostDeadEnemies)
            {
                if (enemy != almostDeadEnemy)
                    continue;

                return almostDeadEnemy;
            }

            return null;
        }
    }
}

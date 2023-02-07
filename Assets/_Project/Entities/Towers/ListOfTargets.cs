using System.Collections.Generic;
using UnityEngine;
using TD.Entities.Enemies;
using TD.Entities.Towers.States;
using TD.PlayerLife;

namespace TD.Entities.Towers
{
    public class ListOfTargets : MonoBehaviour
    {
        [SerializeField] private List<Transform> enemiesToAttack;

        private void OnEnable()
        {
            DamageReceiver.OnEnemyDead += RemoveEnemy;
        }
        private void OnDisable()
        {
            DamageReceiver.OnEnemyDead -= RemoveEnemy;
        }

        private void RemoveEnemy(Transform enemy, Transform killerTower, float reward)
        {
            enemiesToAttack.Remove(enemy);          
        }

       

        public List<Transform> EnemiesToAttack
        {
            get => enemiesToAttack;
            private set => enemiesToAttack = value;
        }


        public CloneFinder CloneFinder
        {
            get => GetComponent<CloneFinder>();
        }

        public Transform FindEnemy()
        {
            foreach(Transform enemy in EnemiesToAttack)
            {
                if (enemy != null)
                {
                    EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
                    if (!enemyMovement.IsWinded)
                    {
                        return enemy;
                    }
                }
                    
            }

            return null;
        }

        public void SwitchTargetFrom(Transform enemy)
        {
            LockTargetState lockTargetState = GetComponent<LockTargetState>();

            int enemyIndex = EnemiesToAttack.IndexOf(enemy);
            if (enemyIndex + 1 < EnemiesToAttack.Count)
            {
                lockTargetState.Target = EnemiesToAttack[enemyIndex + 1];
            }
            else
            {
                StationaryState stationaryBehaviour = GetComponent<StationaryState>();
                transform.position = stationaryBehaviour.StartPosition;

                TowerStateSwitcher towerStateSwitcher = GetComponent<TowerStateSwitcher>();
                towerStateSwitcher.SwitchTo(TowerState.Stationary);

                //ChargeAttackState chargeAttackState = GetComponent<ChargeAttackState>();
                //chargeAttackState.TimeUntilNextAttack = chargeAttackState.CurrentTimeBetweenAttacks;

            }
        }
    }

}

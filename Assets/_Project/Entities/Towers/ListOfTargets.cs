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

        public delegate void SwitchTargetCallback(Transform previousTarget, Transform newTarget, Transform attackingTower);
        public static event SwitchTargetCallback OnTargetSwitch;

        private void OnEnable()
        {
            HealthBehaviour.OnEnemyDead += RemoveEnemy;
        }
        private void OnDisable()
        {
            HealthBehaviour.OnEnemyDead -= RemoveEnemy;
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
                bool doesEnemyExist = enemy != null;
                if (!doesEnemyExist)
                    continue;

                WindedBehaviour windedBehaviour = enemy.GetComponent<WindedBehaviour>();
                bool isItAffectedByWind = windedBehaviour.IsWinded();
                if (isItAffectedByWind)
                    continue;

                return enemy;  

            }

            return null;
        }

        public void SwitchTargetFrom(Transform enemy)
        {
            LockTargetState lockTargetState = GetComponent<LockTargetState>();

            int enemyIndex = EnemiesToAttack.IndexOf(enemy);
            bool areThereOtherEnemiesToAttack = enemyIndex + 1 < EnemiesToAttack.Count;
            if (areThereOtherEnemiesToAttack)
            {
                lockTargetState.Target = EnemiesToAttack[enemyIndex + 1];
                OnTargetSwitch?.Invoke(EnemiesToAttack[enemyIndex], lockTargetState.Target, transform);
            }
            else
            {
                StationaryState stationaryBehaviour = GetComponent<StationaryState>();
                transform.position = stationaryBehaviour.StartPosition;

                TowerStateSwitcher towerStateSwitcher = GetComponent<TowerStateSwitcher>();
                towerStateSwitcher.SwitchTo(TowerState.Stationary);

                OnTargetSwitch?.Invoke(lockTargetState.Target, null, transform);

                //ChargeAttackState chargeAttackState = GetComponent<ChargeAttackState>();
                //chargeAttackState.TimeUntilNextAttack = chargeAttackState.CurrentTimeBetweenAttacks;
            }

            


        }
    }

}

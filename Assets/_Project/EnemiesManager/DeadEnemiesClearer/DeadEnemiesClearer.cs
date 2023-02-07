using System.Collections;
using System.Collections.Generic;
using TD.EnemiesManager.Storer;
using TD.Entities.Enemies;
using TD.PlayerLife;
using TD.WaveSystem;
using UnityEngine;


/*This class purpose is to:
    1) store enemies that die during the wave in a list
    2) clear the list when the players wins the wave
    3) it was created to split the job of killing enemies into 2 tasks:
        - a) enemy is flagged as a dead object when its life reach zero or when it collides with the player base, but it's not destroyed instantly
        - b) enemy is destroyed when player wins the wave

    This class handles "1" "2" "3" and "3a"
    "3b" is handled by EnemyStorer object
*/
namespace TD.EnemiesManager.DeadEnemiesClearer
{
    public class DeadEnemiesClearer : MonoBehaviour
    {
        [SerializeField] private List<Transform> deadEnemies;

       
        private void OnEnable()
        {
            DamageReceiver.OnEnemyDead += StoreDeadEnemy;
            OnEnemyBaseCollisionBehaviour.OnEnemyReachBase += StoreDeadEnemy2;
            WaveManager.OnWaveEnd += ClearListOfDeadEnemies;
            
        }
        private void OnDisable()
        {
            DamageReceiver.OnEnemyDead -= StoreDeadEnemy;
            OnEnemyBaseCollisionBehaviour.OnEnemyReachBase -= StoreDeadEnemy2;
            WaveManager.OnWaveEnd -= ClearListOfDeadEnemies;
        }

        public void StoreDeadEnemy(Transform enemy, Transform killerTower, float reward)
        {
            deadEnemies.Add(enemy);
        }

        public void StoreDeadEnemy2(Transform enemy, int enemyCurrentHealth)
        {
            deadEnemies.Add(enemy);
        }

        public void ClearListOfDeadEnemies(WaveState nextWave)
        {
            deadEnemies.Clear();
        }

    }
}

using System.Collections.Generic;
using TD.Entities.Enemies;
using TD.WaveSystem;
using UnityEngine;

namespace TD.EnemiesManager.Storer
{
    public class EnemyStorer : MonoBehaviour
    {
        public static EnemyStorer Instance;
        [SerializeField] private List<Transform> enemies;

        private void OnEnable()
        {
            HealthBehaviour.OnEnemyDead += RemoveEnemy;
            WaveManager.OnWaveEnd += DestroyAllDeadEnemies;
        }

        private void OnDisable()
        {
            HealthBehaviour.OnEnemyDead -= RemoveEnemy;
            WaveManager.OnWaveEnd -= DestroyAllDeadEnemies;
        }

        private void RemoveEnemy(Transform enemy, Transform killerTower, float reward)
        {
            Enemies.Remove(enemy);
        }

        private void DestroyAllDeadEnemies(WaveState nextWave)
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        public List<Transform> Enemies
        {
            get => Instance.enemies;
            set => Instance.enemies = value;
        }


        private void Awake()
        {
            Instance = this;
        }
    }
}


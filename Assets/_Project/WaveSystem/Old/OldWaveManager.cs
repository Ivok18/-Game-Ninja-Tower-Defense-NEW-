using UnityEngine;
using TD.EnemiesManager.Storer;
using TD.Entities.Enemies;
using TD.PlayerLife;
using UnityEngine.UI;
using System.Net.Http.Headers;
using System.Collections.Generic;
using TD.Entities;

namespace TD.WaveSystem
{
    public class OldWaveManager : MonoBehaviour
    {

        //[HideInInspector]
        [SerializeField] private Text waveText;
        //[HideInInspector]
        [SerializeField] private Button waveButton;
        [SerializeField] private Button restartGameButton;

      
        [SerializeField] private List<Transform> enemiesInCurrentWave;
        [SerializeField] private List<Transform> enemiesToSpawn;
        [SerializeField] private OldWaveData[] waves;
        private int currentWaveIndex;


        public delegate void WaveEndCallback(WaveState nextWave);
        public static event WaveEndCallback OnWaveEnd;


        public OldWaveData CurrentWave => waves[currentWaveIndex];

        public OldEnemySpawnerData Spawner => CurrentWave.spawner;


        private void OnEnable()
        {
            HealthBehaviour.OnEnemyDead += DecreaseEnemiesInCurrentWave;
            OnEnemyBaseCollisionBehaviour.OnEnemyReachBase += DecreaseEnemiesInCurrentWave2;
        }
        private void OnDisable()
        {
            HealthBehaviour.OnEnemyDead -= DecreaseEnemiesInCurrentWave;
            OnEnemyBaseCollisionBehaviour.OnEnemyReachBase -= DecreaseEnemiesInCurrentWave2;
        }

       
        private void DecreaseEnemiesInCurrentWave(Transform enemy, Transform killerTower, float reward)
        {
            //This checks resolves an error I got in the inspector although I don't know exactly the cause
            //of the error. But hey, it's still fixed it :)
            bool doesEnemyExist = enemy != null;
            if (doesEnemyExist)
            {
                enemiesInCurrentWave.Remove(enemy);
            }
            bool haveAllEnemiesBeenKilled = enemiesInCurrentWave.Count <= 0;
            if (haveAllEnemiesBeenKilled && CanEndCurrentWave())
            {
                EndWave();
            }             
        }

        private void DecreaseEnemiesInCurrentWave2(Transform enemy, int enemyCurrentHealth)
        {
            //This checks resolves an error I got in the inspector although I don't know exactly the cause
            //of the error. But hey, it's still fixed it :)
            bool doesEnemyExist = enemy != null;
            if (doesEnemyExist)
            { 
                enemiesInCurrentWave.Remove(enemy);
            }
            bool haveAllEnemiesBeenKilled = enemiesInCurrentWave.Count <= 0;
            if (haveAllEnemiesBeenKilled && CanEndCurrentWave())
            { 
                EndWave();
            }
        }


        void Update()
        {
            if (CurrentWave.waveState == WaveState.Victory) return;
            
            switch (CurrentWave.waveState)
            {
                case WaveState.LoadEnemies:
                    LoadEnemies();
                    break;
                case WaveState.Spawning:
                    SpawnWave();
                    break;
                case WaveState.Inactive:
                    break;
                default:
                    break;
            }


        }

        //This function instantiates all enemies before they enter on the map
        public void LoadEnemies()
        {
            for(int i = 0; i < Spawner.noOfEnemiesToSpawn; i++)
            {
                //Instantiate random enemy type
                int randomEnemyTypeIndex = Random.Range(0, Spawner.typeOfEnemies.Length);
                GameObject randomEnemy = Spawner.typeOfEnemies[randomEnemyTypeIndex].gameObject;
                GameObject enemy = Instantiate(randomEnemy, new Vector3(-100,-100,0), Quaternion.identity);
                SpriteGetter spriteGetter = enemy.GetComponent<SpriteGetter>();
                spriteGetter.SpriteRenderer.enabled = false;
                EnemyStorer.Instance.Enemies.Add(enemy.transform);
                enemiesToSpawn.Add(enemy.transform);
                enemiesInCurrentWave.Add(enemy.transform);
                enemy.transform.SetParent(EnemyStorer.Instance.transform);
            }

            CurrentWave.waveState = WaveState.Spawning;
        }

        public void SpawnWave()
        {
            if (Spawner.nextSpawnTime < Time.time)
            {
                //Get next enemy to push on map
                EnemyMovement enemyMovement = enemiesToSpawn[Spawner.indexOfEnemyToSpawn].GetComponent<EnemyMovement>();

                //Activate its movement
                enemyMovement.CanMove = true;
                SpriteGetter spriteGetter = enemiesToSpawn[Spawner.indexOfEnemyToSpawn].GetComponent<SpriteGetter>();

                //Make him visible
                spriteGetter.SpriteRenderer.enabled = true;

                //Teleports him the starting waypoint
                enemiesToSpawn[Spawner.indexOfEnemyToSpawn].transform.position = Spawner.spawnpoint.position;

                //Decrement no of enemies to spawn
                Spawner.noOfEnemiesToSpawn--;

                //Increment the index of enemy to spawn (used in the list "enemiesToSpawn"
                Spawner.indexOfEnemyToSpawn++;

                //Reset spawn timer
                Spawner.nextSpawnTime = Time.time + Spawner.spawnInterval;


                //Check if more enemies to spawn
                if (Spawner.noOfEnemiesToSpawn == 0)
                    CurrentWave.waveState = WaveState.Idle;
            }
        }

        public void StartNextWave()
        {
            bool isCurrentWaveTheVictoryWave = CurrentWave.waveState == WaveState.Victory;
            if (!isCurrentWaveTheVictoryWave)
            {
                CurrentWave.waveState = WaveState.LoadEnemies;
                waveText.text = "WAVE" + (currentWaveIndex + 1).ToString();
                waveButton.gameObject.SetActive(false);
            }
            else
            {             
                CurrentWave.waveState = WaveState.LoadEnemies;
                waveText.text = "VICTORY!!";
                waveButton.gameObject.SetActive(false);
                restartGameButton.gameObject.SetActive(true);
            }
        }

        public void EndWave()
        {
            bool isThereAWaveAfterThisWave = currentWaveIndex < waves.Length - 1;
            //Prevents out of bounds exception (unknown cause)
            if (!isThereAWaveAfterThisWave)
                return;
            
            waveButton.gameObject.SetActive(true);
            CurrentWave.waveState = WaveState.Inactive;
            enemiesToSpawn.Clear();
            OnWaveEnd?.Invoke(waves[currentWaveIndex+1].waveState);
            currentWaveIndex++;

            //Reset the index of enemy to spawn to zero for the next wave
            Spawner.indexOfEnemyToSpawn = 0;
        }

        public bool CanEndCurrentWave()
        {
            bool isCurrentWaveTheVictoryWave = CurrentWave.waveState == WaveState.Victory;
            if (isCurrentWaveTheVictoryWave)
                return false;

            bool haveAllEnemiesBeenKilled = enemiesInCurrentWave.Count <= 0;
            if (!haveAllEnemiesBeenKilled)
                return false;

            return true;
        }  
    }

}

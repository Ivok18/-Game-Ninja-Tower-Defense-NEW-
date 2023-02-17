using System;
using System.Collections.Generic;
using UnityEngine;

namespace TD.WaveSystem
{
    [Serializable]
    public class EnemySpawnerData
    {
        public bool enableChance;
        public int noOfEnemiesToSpawn;
        [HideInInspector] public float nextSpawnTime;
        public float spawnInterval;
        public Transform spawnpoint;
        public EnemySpawnData[] enemiesSpawnData;
        [HideInInspector] public int indexOfEnemyToSpawn;
    }
}


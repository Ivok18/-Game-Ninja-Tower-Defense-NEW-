using System;
using System.Collections.Generic;
using UnityEngine;

namespace TD.WaveSystem
{
    [Serializable]
    public class EnemySpawnerData
    {
        public int noOfEnemiesToSpawn;
		[HideInInspector] public float nextSpawnTime;
		public float spawnInterval;
		public Transform spawnpoint;
		public Transform[] typeOfEnemies;
        [HideInInspector] public int indexOfEnemyToSpawn;
	}
}


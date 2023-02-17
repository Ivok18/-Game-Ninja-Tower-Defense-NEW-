using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace TD.WaveSystem
{
    [Serializable]
    public class EnemySpawnData
    {
        public GameObject typeOfEnemy;
        [Range(0,1)]
        public float spawnChanceLimit;
        

        
    }
}

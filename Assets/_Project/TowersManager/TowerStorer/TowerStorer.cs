using System.Collections.Generic;
using TD.Entities.Towers;
using TD.TowersManager.Spawner;
using UnityEngine;

namespace TD.TowersManager.Storer
{
    public class TowerStorer : MonoBehaviour
    {
        public List<Transform> DeployedTowers;
        public List<Transform> UndeployedTowers;
        public static TowerStorer Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            TowerPlacer.OnTowerPlaced += AddToDeployedTowers;
            TowerSpawner.OnTowerSpawn += AddToUndeployedTowers;
        }
        private void OnDisable()
        {
            TowerPlacer.OnTowerPlaced -= AddToDeployedTowers;
            TowerSpawner.OnTowerSpawn -= AddToUndeployedTowers;
        }

        private void AddToUndeployedTowers(Transform tower, int cost)
        {
            tower.parent = transform;
            UndeployedTowers.Add(tower);
        }

        private void AddToDeployedTowers(Transform tower)
        {
            tower.parent = transform;
            DeployedTowers.Add(tower);
            UndeployedTowers.Remove(tower);
        }

      
       
    }
}

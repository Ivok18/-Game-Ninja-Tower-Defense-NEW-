using TD.MonetarySystem;
using TD.ShopSystem;
using UnityEngine;

namespace TD.TowersManager.Spawner
{
    public class TowerSpawner : MonoBehaviour
    {
       
        public delegate void TowerSpawnCallback(Transform tower, int cost);
        public static event TowerSpawnCallback OnTowerSpawn;

        private void OnEnable()
        {
            ShopManager.OnTowerBuy += SpawnTower;
        }

        private void OnDisable()
        {
            ShopManager.OnTowerBuy -= SpawnTower;
        }

        public void SpawnTower(Transform preview, int cost)
        {
            Transform tower = Instantiate(preview, new Vector3(-5, -5, -5), Quaternion.identity);
            OnTowerSpawn?.Invoke(tower, cost);      
        }
    }
}


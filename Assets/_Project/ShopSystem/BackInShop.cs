using TD.Entities.Towers;
using TD.TowersManager.Spawner;
using TD.TowersManager.Storer;
using UnityEngine;

namespace TD.ShopSystem
{
    public class BackInShop : MonoBehaviour
    {
        [SerializeField] private Transform towerToPutBack;
        [SerializeField] private int cost;

        public delegate void TowerBackInShopCallback(Transform tower, int cost);
        public static event TowerBackInShopCallback OnTowerBackInShop;

        public Transform Tower => towerToPutBack;

        private void OnEnable()
        {
            TowerSpawner.OnTowerSpawn += WaitForPutBack;
            TowerPlacer.OnTowerPlaced += CancelPutBack;
        }
        private void OnDisable()
        {
            TowerSpawner.OnTowerSpawn -= WaitForPutBack;
            TowerPlacer.OnTowerPlaced -= CancelPutBack;
        }

        private void WaitForPutBack(Transform tower, int cost)
        {
            towerToPutBack = tower;
            this.cost = cost;       
        }

        private void CancelPutBack(Transform tower)
        {
            towerToPutBack = null;
            cost = 0;
        }

        private void Update()
        {       
            //Right to perform action
            if (Input.GetMouseButton(1)) PutTowerBackInShop();
        }

        public void PutTowerBackInShop()
        {
            if (towerToPutBack != null)
            {          
                TowerStorer.Instance.UndeployedTowers.Remove(towerToPutBack);
                Destroy(towerToPutBack.gameObject);
                OnTowerBackInShop?.Invoke(towerToPutBack, cost);
                towerToPutBack = null;
                cost = 0;
            }
        }
    }
}

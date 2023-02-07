using UnityEngine;

namespace TD.UI
{
    public class UIShopModelSelector : MonoBehaviour
    {
        [SerializeField] private Transform towerPrefab;

        public delegate void TowerModelSelectedCallback(Transform model);
        public static event TowerModelSelectedCallback OnTowerModelSelected;

        public void SelectTowerModel()
        {
            OnTowerModelSelected?.Invoke(towerPrefab);
        }
   
    }
}


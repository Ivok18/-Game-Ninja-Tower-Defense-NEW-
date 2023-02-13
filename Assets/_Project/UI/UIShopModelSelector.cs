using UnityEngine;

namespace TD.UI
{
    public class UIShopModelSelector : MonoBehaviour
    {
        [SerializeField] private TowerScriptableObject towerData;

        public delegate void TowerModelSelectedCallback(TowerScriptableObject towerData);
        public static event TowerModelSelectedCallback OnTowerModelSelected;

        public void SelectTowerModel()
        {
            OnTowerModelSelected?.Invoke(towerData);
        }
   
    }
}


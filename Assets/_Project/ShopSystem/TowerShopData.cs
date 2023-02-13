using System;
using TD.Entities.Towers;
using UnityEngine;
using UnityEngine.UI;

namespace TD.ShopSystem
{
    [Serializable]
    public class TowerShopData
    {
        public TowerType towerType;
        public Transform towerPrefab;
        public int cost;
    }
}

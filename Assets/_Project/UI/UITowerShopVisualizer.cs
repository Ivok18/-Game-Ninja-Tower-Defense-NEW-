using System;
using System.Collections;
using System.Collections.Generic;
using TD.Entities.Towers;
using TD.MonetarySystem;
using TD.ShopSystem;
using UnityEngine;
using UnityEngine.UI;

namespace TD.UI
{
    [Serializable]
    public class TowerShopVisualData
    {
        public TowerScriptableObject towerData;
        public Text costDisplay;
        public Image panel;
    }

    public class UITowerShopVisualizer: MonoBehaviour
    {
        [SerializeField] private ShopManager shopManager;
        [SerializeField] private Color towerPanelCanBuyColor;
        [SerializeField] private Color towerPanelCannotBuyColor;
        [SerializeField] private TowerShopVisualData[] towerShopVisuals;

        private void Start()
        {
            foreach(var towerShopData in shopManager.TowersInShop)
            {
                TowerShopVisualData visualizer = FindMatchingVisual(towerShopData);
                bool hasFoundMatchingVisual = visualizer != null;
                if (!hasFoundMatchingVisual)
                    continue;

                visualizer.costDisplay.text = towerShopData.Cost.ToString();
                visualizer.costDisplay.text += "$";
            }
        }

        private void Update()
        {
            CheckTowerAvailableForBuy();
        }

        private void CheckTowerAvailableForBuy()
        {
            /*
               tower panel canBuyColor:
                   - 0.2784314
                   - 1
                   - 0
                   - 1=

               tower panel cannot buy color:
                   - 1
                   - 0.04705882
                   - 0
                   - 1
           */
            Color canBuyColor = towerPanelCanBuyColor;
            Color cannotBuyColor = towerPanelCannotBuyColor;
            foreach (var tower in shopManager.TowersInShop)
            {
                TowerShopVisualData visualizer = FindMatchingVisual(tower);
                bool hasFoundMatchingVisual = visualizer != null;
                if (!hasFoundMatchingVisual)
                    continue;

                bool hasEnoughMoney = MoneyManager.Instance.Money >= tower.Cost;
                if (hasEnoughMoney)
                {
                    visualizer.panel.color = canBuyColor;
                }
                else
                {
                    visualizer.panel.color = cannotBuyColor;
                }
            }
        }

        public TowerShopVisualData FindMatchingVisual(TowerScriptableObject towerData)
        {
            foreach(var towerShopVisual in towerShopVisuals)
            {
                bool isVisualCorrespondingToTowerShopData = towerData == towerShopVisual.towerData;
                if (!isVisualCorrespondingToTowerShopData)
                    continue;

                return towerShopVisual;
            }

            return null;
        }
    }

   
}
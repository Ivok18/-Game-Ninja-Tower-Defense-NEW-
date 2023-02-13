using System;
using System.Collections;
using System.Collections.Generic;
using TD.Entities.Towers;
using TD.MonetarySystem;
using TD.ShopSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TD.UI
{
    [Serializable]
    public class ElementShopVisualData
    {
        public ElementScriptableObject elementData;
        public TextMeshProUGUI costDisplay;
        public Image backgroundImage;
        public Image elementImage;
        public Sprite imageCanBuy;
        public Sprite imageCannotBuy;
        public Color backgroundCanBuyColor;
        public Color backgroundCannotBuyColor;
    }

    public class UIElementShopVisualizer : MonoBehaviour
    {
        [SerializeField] private ShopManager shopManager;
        [SerializeField] private ElementShopVisualData[] elementShopVisuals;

        private void Start()
        {
            foreach (var elementData in shopManager.ElementsInShop)
            {
                ElementShopVisualData visualizer = FindMatchingVisual(elementData);
                bool hasFoundMatchingVisual = visualizer != null;
                if (!hasFoundMatchingVisual)
                    continue;

                visualizer.costDisplay.text = elementData.Cost.ToString();
                visualizer.costDisplay.text += "$";
            }
        }

        private void Update()
        {
            CheckElementAvailableForBuy();
        }

        public void CheckElementAvailableForBuy()
        {
            /*
                 fire background image canBuyColor:
                    - 1 
                    - 0.7529412
                    - 0
                    - 1

                 earth backgournd image canBuyColor:
                    - 0.05098039
                    - 1
                    - 0.1490196 
                    - 1
                 wind background image canBuyColor:
                    - 0.6
                    - 1
                    - 0.9058824
                    - 1

                 cannotBuyColor is same for all background images:
                    - 0.6980392
                    - 0.6980392
                    - 0.6980392
                    - 1
            */

            foreach (var elementData in shopManager.ElementsInShop)
            {
                ElementShopVisualData visualizer = FindMatchingVisual(elementData);
                bool hasFoundMatchingVisual = visualizer != null;
                if (!hasFoundMatchingVisual)
                    continue;

                bool hasEnoughMoney = MoneyManager.Instance.Money >= elementData.Cost;
                if (hasEnoughMoney)
                {
                    visualizer.backgroundImage.color = visualizer.backgroundCanBuyColor;
                    visualizer.elementImage.sprite = visualizer.imageCanBuy;
                }
                else
                {
                    visualizer.backgroundImage.color = visualizer.backgroundCannotBuyColor;
                    visualizer.elementImage.sprite = visualizer.imageCannotBuy;
                }
            }

        }
        public ElementShopVisualData FindMatchingVisual(ElementScriptableObject elementData)
        {
            foreach (var elementShopVisual in elementShopVisuals)
            {
                bool isVisualCorrespondingToElementShopData = elementData == elementShopVisual.elementData;
                if (!isVisualCorrespondingToElementShopData)
                    continue;

                return elementShopVisual;
            }

            return null;
        }
    }
}
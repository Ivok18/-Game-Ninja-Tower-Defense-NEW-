using System;
using TD.ElementSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TD.ShopSystem
{
    [Serializable]
    public class ElementShopData
    {
        public TowerElement element;
        public int cost;
        
        [Header("Visual Settings")]
        public TextMeshProUGUI costDisplay; 
        public Image panel;
        public Image visualizer;
        [Tooltip("Element Can Buy Panel Color ")]
        public Color panelCanBuyColor;
        [Tooltip("Element Cannot Buy Panel Color ")]
        public Color panelCannotBuyColor;
        [Tooltip("Element Can Buy Icon ")]
        public Sprite canBuyVisualizer;
        [Tooltip("Element Cannot Buy Icon ")]
        public Sprite cannotBuyVisualizer;
    }
}
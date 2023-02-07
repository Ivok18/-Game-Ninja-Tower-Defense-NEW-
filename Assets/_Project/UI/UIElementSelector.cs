using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TD.ElementSystem;
using System.Data;
using TD.Map;
using TD.ShopSystem;
using UnityEngine.UI;

namespace TD.UI
{
    public class UIElementSelector : MonoBehaviour
    {
        [SerializeField] private TowerElement element;
        public bool UIElementIsSelected;

        public TowerElement Element => element;

        public delegate void UIElementClickCallback(TowerElement element);
        public static event UIElementClickCallback OnUIElementClick;
      

        public void OnClick()
        {
            OnUIElementClick?.Invoke(Element);
        }
    }
}
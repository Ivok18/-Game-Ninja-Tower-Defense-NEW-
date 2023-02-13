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
        //[SerializeField] private TowerElement element;
        [SerializeField] private ElementScriptableObject elementData;
        public bool UIElementIsSelected;

        public ElementScriptableObject ElementData => elementData;

        public delegate void UIElementClickCallback(ElementScriptableObject elementData);
        public static event UIElementClickCallback OnUIElementClick;
      

        public void OnClick()
        {
            OnUIElementClick?.Invoke(ElementData);
        }
    }
}
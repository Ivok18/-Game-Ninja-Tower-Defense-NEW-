using System.Collections;
using System.Collections.Generic;
using TD.ElementSystem;
using TD.Map;
using TD.PlayerLife;
using TD.ShopSystem;
using UnityEngine;

namespace TD.UI
{
    public class UIElementHighliter : MonoBehaviour
    {
        [SerializeField] private GameObject selectionBrackets;
        private UIElementSelector uIElementSelector;

        private void OnEnable()
        {
            ElementInQueueManager.OnElementAddedToQueue += UpdateSelectionBrackets;
            ElementInQueueManager.OnQueueClear += HideSelectionBrackets;
        }

        private void OnDisable()
        {
            ElementInQueueManager.OnElementAddedToQueue -= UpdateSelectionBrackets;
            ElementInQueueManager.OnQueueClear -= HideSelectionBrackets;
        }
        private void Awake()
        {
            uIElementSelector = GetComponent<UIElementSelector>();
        }

        private void UpdateSelectionBrackets(TowerElement element, int cost)
        {
            if(uIElementSelector.Element == element)
            {
                ShowSelectionBrackets();
            }
            else
            {
                HideSelectionBrackets(element);
            }
        }
        private void ShowSelectionBrackets()
        {
            selectionBrackets.SetActive(true);  
        }
        private void HideSelectionBrackets(TowerElement element)
        {
             selectionBrackets.SetActive(false);  
        }


    }
}
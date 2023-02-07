using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TD.ElementSystem
{
    public class SwapButtonBehaviour : Selectable
    {
        [Header("Attached element")]
        [SerializeField] private TowerElement attachedElement;
     
        [Header("Not-Highlighted values")]
        [SerializeField] Color normalColor;
        [SerializeField] string normalName;

        [Header("Highlighted values")]
        [SerializeField] Color highlightedColor;
        [SerializeField] string higlightedName;

        public delegate void ElementSwapButtonPressedCallback(TowerElement elementToBeReplaced);
        public static event ElementSwapButtonPressedCallback OnElementSwapButtonPressed;

        public void SetAttachedElement(TowerElement element)
        {
            attachedElement = element;
        }
        public void SetNormalColor(Color color)
        {
            normalColor = color;
        }
        public void SetNormalName(string name)
        {
            normalName = name;
        }
        public void SetHighlightedColor(Color color)
        {
            highlightedColor = color;
        }
        public void SetHighlightedName(string name)
        {
            higlightedName = name;
        }

        
        public void PressSwapButton()
        {
            TowerElement elementToBeReplaced = attachedElement;
            OnElementSwapButtonPressed(elementToBeReplaced);
        }

        private void Update()
        {     
            //If the mouse cursor is over the button -> display highlighted values in the inspecotr
            if (IsHighlighted())
            {
                transform.GetComponent<Image>().color = highlightedColor;
                GetComponentInChildren<TextMeshProUGUI>().text = higlightedName;             
            }
            else //If the mouse cursor is over the button -> display not-highlighted values in the inspector
            {
                transform.GetComponent<Image>().color = normalColor;
                GetComponentInChildren<TextMeshProUGUI>().text = normalName;
            }
        }
    }
}
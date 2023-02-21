using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using TD.ElementSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace UI
{
    public class UICatalystButtonBehaviour : Selectable
    {
        [SerializeField] private Color normalColor;
        [SerializeField] private Color highlightedColor;
        [SerializeField] private float alphaWhenNotHighlighted;
        private Transform towerHolder;
        private TowerElement elementOfCatalyst;
        private bool isCatalyseButton;
        private bool isFuseButton;

        public delegate void CatalyseButtonPressedCallback(Transform targetTower, TowerElement elementOfCatalyst);
        public static event CatalyseButtonPressedCallback OnCatalyseButtonPressed;

        public delegate void FuseButtonPressedCallback(Transform targetTower);
        public static FuseButtonPressedCallback OnFuseButtonPressed;

        

        // Update is called once per frame
        void Update()
        {
            //If the mouse cursor is over the button -> display highlighted values in the inspecotr
            if (IsHighlighted())
            {
                transform.GetComponent<Image>().color = highlightedColor;
            }
            else //If the mouse cursor is not over the button -> display not-highlighted values in the inspector
            {
                transform.GetComponent<Image>().color = normalColor;
            }
        }

        public void SetupCatalyseButton(Transform holder, TowerElement element, Color buttonColor)
        {
            highlightedColor = buttonColor;
            normalColor = highlightedColor;
            normalColor.a = 1;
            towerHolder = holder;
            elementOfCatalyst = element;
            GetComponentInChildren<TextMeshProUGUI>().text = "CATALYSE";
            isCatalyseButton = true;
            isFuseButton = false;
        
        }

        public void SetupFuseButton(Transform holder, TowerElement element, Color buttonColor)
        {
            highlightedColor = buttonColor;
            normalColor = highlightedColor;
            normalColor.a = alphaWhenNotHighlighted;
            towerHolder = holder;
            elementOfCatalyst = element;
            GetComponentInChildren<TextMeshProUGUI>().text = "FUSE";
            isCatalyseButton = false;
            isFuseButton = true;
        }

        public void PressButton()
        {
            if(isCatalyseButton)
            {
                OnCatalyseButtonPressed?.Invoke(towerHolder, elementOfCatalyst);
                
            }
            else if(isFuseButton)
            {
                OnFuseButtonPressed?.Invoke(towerHolder);
            }
        }

       

    }
}
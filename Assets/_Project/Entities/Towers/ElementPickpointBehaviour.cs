using Mono.Cecil.Cil;
using Mono.CompilerServices.SymbolWriter;
using System;
using System.Collections;
using System.Collections.Generic;
using TD.Entities.Towers;
using TD.MonetarySystem;
using TD.ShopSystem;
using TD.UI;
using UI;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UIElements;

namespace TD.ElementSystem
{
    public class ElementPickpointBehaviour : MonoBehaviour
    {

        [SerializeField] private Transform towerHolder;
        [SerializeField] private SpriteRenderer visualizer;
        [SerializeField] private float glowAnimationSpeed;
        private float glowAlphaMax = 1.0f;
        private float glowAlphaMin = 0f;
        private float glowDestinationAlpha = 0f;

        private void OnEnable()
        {
            UICatalystButtonBehaviour.OnCatalyseButtonPressed += DisableElementPickpoint;
        }

        private void OnDisable()
        {
            UICatalystButtonBehaviour.OnCatalyseButtonPressed -= DisableElementPickpoint;
        }

        private void DisableElementPickpoint(Transform targetTower, TowerElement elementOfCatalyst)
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            ElementsTracker elementBoostApplier = towerHolder.GetComponent<ElementsTracker>();
            if (!elementBoostApplier.IsReadyToGetAnElement)
            {
                visualizer.enabled = false;
                return;
            }
            RunAnim();
        }


        public void RunAnim()
        {
            visualizer.enabled = true;
            Color colorPreviousFrame = visualizer.color;
            float alphaPreviousFrame = colorPreviousFrame.a;
            Color colorNextFrame = colorPreviousFrame;
            bool isAnimAtMaxValue = alphaPreviousFrame >= glowAlphaMax;
            bool isAnimAtMinValue = alphaPreviousFrame <= glowAlphaMin;
            bool isAnimTargetValueIsMaxValue = glowDestinationAlpha == glowAlphaMax;
            bool isAnimTargetValueIsMinValue = glowDestinationAlpha == glowAlphaMin;
            if (isAnimAtMaxValue)
            {
                glowDestinationAlpha = glowAlphaMin;
            }
            if (isAnimAtMinValue)
            {
                glowDestinationAlpha = glowAlphaMax;
            }
            if (isAnimTargetValueIsMaxValue)
            {
                colorNextFrame.a = alphaPreviousFrame + glowAnimationSpeed * Time.deltaTime;
            }
            else if (isAnimTargetValueIsMinValue)
            {
                colorNextFrame.a = alphaPreviousFrame - glowAnimationSpeed * Time.deltaTime;
            }
            visualizer.color = colorNextFrame;           
        }
    }
}
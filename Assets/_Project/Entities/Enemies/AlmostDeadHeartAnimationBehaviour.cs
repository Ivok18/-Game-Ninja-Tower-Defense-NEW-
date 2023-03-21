using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TD.Entities.Enemies
{
    public class AlmostDeadHeartAnimationBehaviour : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer heartSpriteRenderer;
        [SerializeField] private SpriteRenderer headSkullIconSpriteRenderer;
        [SerializeField] private float glowAnimationSpeed;
        private float glowAlphaMax = 1.0f;
        private float glowAlphaMin = 0f;
        private float glowDestinationAlpha = 0f;
        private AlmostDeadSignaler almostDeadSignaler;

        private void Awake()
        {
            almostDeadSignaler = GetComponent<AlmostDeadSignaler>();
        }

        private void Update()
        {
            if (!almostDeadSignaler.IsAlmostDead)
            {
                headSkullIconSpriteRenderer.gameObject.SetActive(false);
                heartSpriteRenderer.gameObject.SetActive(true);
                heartSpriteRenderer.color = new Color(heartSpriteRenderer.color.r, heartSpriteRenderer.color.g, heartSpriteRenderer.color.b, glowAlphaMax);
                return;
            }

            headSkullIconSpriteRenderer.gameObject.SetActive(true);
            heartSpriteRenderer.gameObject.SetActive(false);
            Color colorPreviousFrame = headSkullIconSpriteRenderer.color;
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
            headSkullIconSpriteRenderer.color = colorNextFrame;


        }

    }
}

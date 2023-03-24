using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TD.Entities.Enemies
{
    public class AlmostDeadHeartAnimationBehaviour : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer healthIconUp;
        [SerializeField] private SpriteRenderer healthIconLeft;
        [SerializeField] private SpriteRenderer skullIcon;
        [SerializeField] private SpriteRenderer currentIconSpriteRenderer;
        [SerializeField] private float glowAnimationSpeed;
        private float glowAlphaMax = 1.0f;
        private float glowAlphaMin = 0f;
        private float glowDestinationAlpha = 0f;
        private AlmostDeadSignaler almostDeadSignaler;
        private EnemyMovement enemyMovement;

        private void Awake()
        {
            almostDeadSignaler = GetComponent<AlmostDeadSignaler>();
            enemyMovement = GetComponent<EnemyMovement>();
        }

        private void Update()
        {
            if (!almostDeadSignaler.IsAlmostDead)
            {
                //currentSkullIconSpriteRenderer.gameObject.SetActive(false);
                //currentHeartIconSpriteRenderer.gameObject.SetActive(true);
                // currentHeartIconSpriteRenderer.color = new Color(currentHeartIconSpriteRenderer.color.r, currentHeartIconSpriteRenderer.color.g, currentHeartIconSpriteRenderer.color.b, glowAlphaMax);
                if (enemyMovement.HasHorizontalDirection)
                {
                    SetCurrentIconSpriteRenderer(healthIconUp);            
                }
                else 
                {
                    SetCurrentIconSpriteRenderer(healthIconLeft);
                }
                currentIconSpriteRenderer.color = new Color(currentIconSpriteRenderer.color.r, currentIconSpriteRenderer.color.g, currentIconSpriteRenderer.color.b, glowAlphaMax);
                return;
            }


            if (enemyMovement.HasHorizontalDirection)
            {
                SetCurrentIconSpriteRenderer(skullIcon);
            }
            else
            {
                SetCurrentIconSpriteRenderer(skullIcon);
            }

            //currentIconRenderer.gameObject.SetActive(true);
            //currentHeartIconSpriteRenderer.gameObject.SetActive(false);
            Color colorPreviousFrame = currentIconSpriteRenderer.color;
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
            currentIconSpriteRenderer.color = colorNextFrame;


        }

        public void SetCurrentIconSpriteRenderer(SpriteRenderer spriteRenderer)
        {
            currentIconSpriteRenderer = spriteRenderer;

        }

    }
}

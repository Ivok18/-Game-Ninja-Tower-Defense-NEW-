using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TD.Entities.Enemies
{
    [Serializable]
    public class TargetedEffectData
    {
        public string name;
        public SpriteRenderer spriteRenderer;

        [Header("Data when targeted")]
        [Tooltip("alpha when targeted")]
        public float alphaWhenTargeted;
       

        [Header("Data when not targeted")]
        [Tooltip("alpha when targeted")]
        public float alphaWhenNotTargeted;
       
    }

    public class TargetedEffectBehaviour : MonoBehaviour
    {
        
        private TargetedBehaviour targetedBehaviour;
        [SerializeField] private List<TargetedEffectData> targetedEffectDatas;
        [SerializeField] private float alphaWhenTargeted;
        [SerializeField] private float alphaWhenNotTargeted;
     

        private void Awake()
        {
            targetedBehaviour = GetComponent<TargetedBehaviour>();
        }
        private void Update()
        {
            bool isTargeted = targetedBehaviour.IsTargeted() == true ? true : false;
            if (!isTargeted)
            {
                //false means apply effect corresponding to the situation in which enemy is not targeted
                ApplyEffect(false); 
                return;
            }

            //true means apply effect corresponding to the situation in which enemy is targeted
            ApplyEffect(true);
        }
            
        public void ApplyEffect(bool isTargeted)
        {
            //SetAlphas(isTargeted);
            //SetScale(isTargeted);
        }

        public void SetAlphas(bool isTargeted)
        {
            if(isTargeted)
            {
                foreach (var effectData in targetedEffectDatas)
                {
                    if (effectData.spriteRenderer.color.a == effectData.alphaWhenTargeted)
                        continue;

                    Color color = effectData.spriteRenderer.color;
                    color.a = effectData.alphaWhenTargeted;
                    effectData.spriteRenderer.color = color;
                }
            }
            else
            {
                foreach (var effectData in targetedEffectDatas)
                {
                    if (effectData.spriteRenderer.color.a == effectData.alphaWhenNotTargeted)
                        continue;

                    Color color = effectData.spriteRenderer.color;
                    color.a = effectData.alphaWhenNotTargeted;
                    effectData.spriteRenderer.color = color;              
                }
            }
        }

        public void SetScale(bool isTargeted)
        {
            if(isTargeted)
            {
                transform.localScale = Vector3.one * 0.7f;
            }
            else
            {
                transform.localScale = Vector3.one * 0.3f;
            }
        }
       
    }
}
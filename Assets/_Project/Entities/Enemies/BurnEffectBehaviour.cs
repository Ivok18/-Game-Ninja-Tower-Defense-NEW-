using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TD.Entities.Enemies
{
    public class BurnEffectBehaviour : MonoBehaviour
    {
        private BurnBehaviour burnBehaviour;
        private WindedBehaviour windedBehaviour;
        [SerializeField] private GameObject burnEffect;

        private void Awake()
        {
            burnBehaviour = GetComponent<BurnBehaviour>();
            windedBehaviour = GetComponent<WindedBehaviour>();
        }

        private void Update()
        {
            if (!burnBehaviour.IsBurning()
               || (burnBehaviour.IsBurning() && windedBehaviour.IsWinded()))
            {
                burnEffect.SetActive(false);
                return;
            }
            burnEffect.SetActive(true);
        }
    }
}

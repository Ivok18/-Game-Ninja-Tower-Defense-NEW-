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
            bool isBurning = burnBehaviour.IsBurning() ? true : false;
            bool isWinded = windedBehaviour.IsWinded() ? true : false;
            if (!isBurning || (isBurning && isWinded))
            {
                burnEffect.SetActive(false);
                return;
            }
            burnEffect.SetActive(true);
        }
    }
}

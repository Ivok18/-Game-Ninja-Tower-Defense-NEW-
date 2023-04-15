using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TD.Entities.Enemies
{
    public class WindedEffectBehaviour : MonoBehaviour
    {
        [SerializeField] private WindedBehaviour windedBehaviour;
        [SerializeField] private GameObject windedEffect;

        private void Update()
        {
            bool isWinded = windedBehaviour.IsWinded() ? true : false;
            if (!isWinded)
            {
                windedEffect.SetActive(false);
                return;
            }
            windedEffect.SetActive(true);
        }
    }
}

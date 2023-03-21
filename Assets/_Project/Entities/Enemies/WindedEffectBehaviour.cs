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
            if (!windedBehaviour.IsWinded[0])
            {
                windedEffect.SetActive(false);
                return;
            }
            windedEffect.SetActive(true);
        }
    }
}

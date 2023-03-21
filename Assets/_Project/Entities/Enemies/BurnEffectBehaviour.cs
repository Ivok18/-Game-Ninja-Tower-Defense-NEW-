using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TD.Entities.Enemies
{
    public class BurnEffectBehaviour : MonoBehaviour
    {
        [SerializeField] private BurnBehaviour burnBehaviour;
        [SerializeField] private GameObject burnEffect;

        private void Update()
        {
            if (!burnBehaviour.IsBurning[0])
            {
                burnEffect.SetActive(false);
                return;
            }
            burnEffect.SetActive(true);
        }
    }
}

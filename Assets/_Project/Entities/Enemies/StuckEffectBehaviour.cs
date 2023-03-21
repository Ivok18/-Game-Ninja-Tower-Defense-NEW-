using System.Collections;
using System.Collections.Generic;
using TD.Entities.Enemies;
using UnityEngine;


namespace TD.Entities.Enemies
{
    public class StuckEffectBehaviour : MonoBehaviour
    {

        [SerializeField] private StuckBehaviour stuckBehaviour;
        [SerializeField] private GameObject stuckEffect;

        private void Update()
        {
            if (!stuckBehaviour.IsStuck[0])
            {
                stuckEffect.SetActive(false);
                return;
            }
            stuckEffect.SetActive(true);
        }
    }
}

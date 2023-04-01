using System.Collections;
using System.Collections.Generic;
using TD.Entities.Enemies;
using UnityEngine;


namespace TD.Entities.Enemies
{
    public class StuckEffectBehaviour : MonoBehaviour
    {

        private StuckBehaviour stuckBehaviour;
        private WindedBehaviour windedBehaviour;
        [SerializeField] private GameObject stuckEffect;

        private void Awake()
        {
            stuckBehaviour = GetComponent<StuckBehaviour>();
            windedBehaviour = GetComponent<WindedBehaviour>();
        }
        private void Update()
        {
            if (!stuckBehaviour.IsStuck())
            {
                if (!stuckBehaviour.IsStuck()
                || (stuckBehaviour.IsStuck() && windedBehaviour.IsWinded()))
                {
                    stuckEffect.SetActive(false);
                    return;
                }
            }
            stuckEffect.SetActive(true);
        }
    }
}

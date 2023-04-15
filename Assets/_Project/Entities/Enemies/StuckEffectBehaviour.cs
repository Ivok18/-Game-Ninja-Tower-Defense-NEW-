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
            bool isStuck = stuckBehaviour.IsStuck() ? true : false;
            bool isWinded = windedBehaviour.IsWinded() ? true : false;
            if (!isStuck)
            {
                if (!isStuck || (isStuck && isWinded))
                {

                    stuckEffect.SetActive(false);
                    return;
                }
            }
            stuckEffect.SetActive(true);
        }
    }
}

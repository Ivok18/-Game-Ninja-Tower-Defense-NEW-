using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TD.Entities.Enemies
{
    public class DodgeEffectBehaviour : MonoBehaviour
    {
        [SerializeField] private DodgeBehaviour dodgeBehaviour;
        [SerializeField] private TrailRenderer[] trails;

        private void Update()
        {
            if (!dodgeBehaviour.CanStartDodge)
            {
                foreach (var trail in trails)
                {
                    trail.emitting = false;
                }

                return;

            }
           
            foreach (var trail in trails)
            {
                trail.emitting = true;
            }
        }
    }
}
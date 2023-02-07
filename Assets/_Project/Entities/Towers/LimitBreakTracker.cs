using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TD.Entities.Towers
{
    public class LimitBreakTracker : MonoBehaviour
    {
        public bool HasBrokeLimits;

        private void OnEnable()
        {
            LimitBreakActioner.OnTowerLimitBreak += UpdateTracker;
        }

        private void OnDisable()
        {
            LimitBreakActioner.OnTowerLimitBreak -= UpdateTracker;
        }

        private void UpdateTracker(Transform transform)
        {
            if(transform == this.transform) HasBrokeLimits = true;
        }

    }
}
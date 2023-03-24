using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using TD.Entities.Towers;
using TD.Entities.Towers.States;
using TD.StatusSystem;
using UnityEngine;


namespace TD.Entities.Enemies
{
    public class TargetedBehaviour : MonoBehaviour
    {
        public List<Transform> WaitingIncomingAttackers;
        public List<Transform> IncomingAttackers;

  
       
        public bool IsTargeted()
        {
            return (IsInRadiusOfAtLeastOneAttacker() || IsAffectedByStatus()) ? true : false;
        }

        public bool IsAffectedByStatus()
        {
            BurnBehaviour burnBehaviour = GetComponent<BurnBehaviour>();
            StuckBehaviour stuckBehaviour = GetComponent<StuckBehaviour>();
            WindedBehaviour windedBehaviour = GetComponent<WindedBehaviour>();

            bool isBurned = burnBehaviour.IsBurning();
            bool isStuck = stuckBehaviour.IsStuck();
            bool isWinded = windedBehaviour.IsWinded();
            if (isBurned || isStuck || isWinded)
            {
                return true;
            }

            return false; 
        }

        public bool IsInRadiusOfAtLeastOneAttacker()
        {
            foreach(Transform incomingAttacker in IncomingAttackers)
            {
                RadiusGetter radiusGetter = incomingAttacker.GetComponent<RadiusGetter>();
                if (radiusGetter == null)
                    continue;

                if (radiusGetter.RadiusTransform == null)
                    continue;


                RadiusDetectionBehaviour radiusDetect = radiusGetter.RadiusTransform.GetComponent<RadiusDetectionBehaviour>();
                if (radiusDetect == null)
                    continue;

                if (radiusDetect.IsInRadius(transform) == false)
                    continue;

                return true;
            }

            IncomingAttackers.Clear();
            return false;
        }

    }
}

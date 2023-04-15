using System.Collections;
using System.Collections.Generic;
using TD.Entities.Towers;
using UnityEngine;

namespace TD.StatusSystem
{
    public class StatusRollMaker : MonoBehaviour
    {
        private StatusToInflictTracker statusToInflictTracker;
        private ListOfTargets listOfTargets;
        public bool HasTriedToTriggerAllStatus;

        public delegate void RollPerformedCallback(Transform targetTower, int targetStatusId);
        public static event RollPerformedCallback OnRollPerformed;

        private void Awake()
        {
            listOfTargets = GetComponent<ListOfTargets>();
            statusToInflictTracker = GetComponent<StatusToInflictTracker>();
        }

        private void Update()
        {
            bool hasTargets = listOfTargets.EnemiesToAttack.Count > 0 ? true : false;
            if (!hasTargets)
                return;

            bool hasGotAtLeastOneStatus = statusToInflictTracker.CurrentStatusToInflict.Count > 0 ? true : false;
            if (!hasGotAtLeastOneStatus)
                return;


            if (HasTriedToTriggerAllStatus)
                return;

            foreach(var statusToInflict in statusToInflictTracker.CurrentStatusToInflict)
            {
                if (statusToInflict == null)
                    continue;

                statusToInflict.TryTrigger();
                OnRollPerformed?.Invoke(transform, statusToInflict.id);
            }
            HasTriedToTriggerAllStatus = true;
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TD.ElementSystem;
using TD.Entities.Enemies;
using TD.Entities.Towers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TD.StatusSystem
{
    public enum StatusType
    {
        None,
        Burned,
        Stuck,
        Winded
    }

    [Serializable]
    public class Status
    {
        private static int staticID = 0;
        public int id;
        public StatusType type;
        public RollOutcome rollOutcome;
        public float currentOddsForActivation;
        private float baseOddsForActivation = 0.03f;
        public float rollResult;
        public bool canInflict;

        public Status(StatusType _type, float _oddsForActivation)
        {
            type = _type;
            currentOddsForActivation = _oddsForActivation;
            id = staticID;
            staticID++;         
        }


        public Status(StatusType _statusType)
        {
            type = _statusType;
            currentOddsForActivation = baseOddsForActivation;
            id = staticID;
            staticID++;
        }

        public Status()
        {
            type = StatusType.None;
            currentOddsForActivation = 0;
            id = staticID;
            staticID++;
        }

        public bool TryTrigger()
        {
            rollResult = Random.Range(0f, 1f);
            //Debug.Log("probability of activation -> " + currentOddsForActivation);
            Debug.Log("result -> " + rollResult);
            if (rollResult <= currentOddsForActivation)
            {
                canInflict = true;
                rollOutcome = RollOutcome.SUCCESS;
                return canInflict;
            }
            else
            {
                canInflict = false;
                rollOutcome = RollOutcome.FAIL;
                return canInflict;
            } 
        }      
    }

    public class StatusManager : MonoBehaviour
    {
        private Dictionary<TowerElement, Status> statusTable;

        public delegate void StatusAddedOnTowerCallback(Transform targetTower, int idOfStatusAdded);
        public static event StatusAddedOnTowerCallback OnStatusAddedOnTower;

        public delegate void StatusRemoveFromTowerCallback(Transform targetTower, int idOfRemovedStatus);
        public static event StatusRemoveFromTowerCallback OnStatusRemovedFromTower;

        private void Start()
        {
            statusTable = new Dictionary<TowerElement, Status>();
            statusTable.Add(TowerElement.Fire, new Status(StatusType.Burned));
            statusTable.Add(TowerElement.Earth, new Status(StatusType.Stuck));
            statusTable.Add(TowerElement.Wind, new Status(StatusType.Winded));

        }
        private void OnEnable()
        {
            ElementDataApplier.OnElementDataAppliedOnTower += AddStatus;
            ElementDataApplier.OnElementDataUnappliedFromTower += RemoveStatus;

            EnemyHitDetection.OnEnemyHit += TryToInflictStatus;
        }

        private void OnDisable()
        {
            ElementDataApplier.OnElementDataAppliedOnTower -= AddStatus;
            ElementDataApplier.OnElementDataUnappliedFromTower -= RemoveStatus;

            EnemyHitDetection.OnEnemyHit -= TryToInflictStatus;
        }

        public void AddStatus(Transform tower, ElementScriptableObject dataOfElementApplied)
        {
            StatusToInflictTracker statusToInflictTracker = tower.GetComponent<StatusToInflictTracker>();
            StatusType statusType = statusTable[dataOfElementApplied.Element].type;
            Status status = new Status(statusType);
            statusToInflictTracker.CurrentStatusToInflict.Add(status);

            OnStatusAddedOnTower?.Invoke(tower, status.id);
        }

        public void RemoveStatus(Transform tower, TowerElement element)
        {
            StatusToInflictTracker statusToInflictTracker = tower.GetComponent<StatusToInflictTracker>();
            StatusType statusType = statusTable[element].type;
            Status statusToRemove = new Status(statusType);
            int indexOfStatusToRemove = 0;
            int idOfRemovedStatus = 0;
            for(int i = 0; i < statusToInflictTracker.CurrentStatusToInflict.Count; i++)
            {
                if (!(statusToInflictTracker.CurrentStatusToInflict[i].type == statusToRemove.type))
                    return;

                indexOfStatusToRemove = i;
                idOfRemovedStatus = statusToInflictTracker.CurrentStatusToInflict[i].id;
            }
            statusToInflictTracker.CurrentStatusToInflict.RemoveAt(indexOfStatusToRemove);
            OnStatusRemovedFromTower(tower, idOfRemovedStatus);
        }

        public void TryToInflictStatus(Transform enemy, Transform attackingTower, Vector3 hitPosition)
        {
            StatusToInflictTracker statusToInflictTracker = attackingTower.GetComponent<StatusToInflictTracker>();
            if (statusToInflictTracker == null)
                return;

            if(statusToInflictTracker.CurrentStatusToInflict.Count <= 0)            
                return;
            
           
            foreach(var status in statusToInflictTracker.CurrentStatusToInflict)
            {
                StatusRollMaker statusRollMaker = attackingTower.GetComponent<StatusRollMaker>();
                if (statusRollMaker == null)
                    return;

                if (statusRollMaker.HasTriedToTriggerAllStatus)
                    statusRollMaker.HasTriedToTriggerAllStatus = false;


                bool canInflictStatus = (status.rollOutcome == RollOutcome.SUCCESS) ? true : false;
                if (!canInflictStatus)
                    return;
      
                InflictedStatusActivator inflictedStatusActivator = enemy.GetComponent<InflictedStatusActivator>();
                inflictedStatusActivator.InflictStatus(status.type);
            }
        }
    }
}
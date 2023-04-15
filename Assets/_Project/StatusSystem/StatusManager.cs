using System;
using System.Collections.Generic;
using TD.ElementSystem;
using TD.Entities.Enemies;
using TD.Entities.Towers;
using TD.MonetarySystem;
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
        private static int staticID = 1;
        public int id;
        public StatusType type;
        public RollOutcome rollOutcome;
        public float currentOddsForActivation;
        private float baseOddsForActivation = 0.01f;
        public float rollResult;
        public bool canInflict;
        private float boostAmount = 0.04f;
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

        //This function is triggered each time a roll is performed by a tower
        public bool TryTrigger()
        {
            rollResult = Random.Range(0f, 1f);
            bool hasRollSucceded = rollResult <= currentOddsForActivation;
            if (hasRollSucceded)
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

        //This functions is triggered when player click on a boost in UIStatusUpgradeManager of the target tower
        public void BoostOddsForActivation()
        {
            currentOddsForActivation += boostAmount;
        }
    }

    public class StatusManager : MonoBehaviour
    {
        private Dictionary<TowerElement, Status> statusTable;

        public delegate void StatusAddedOnTowerCallback(Transform targetTower, int idOfStatusAdded);
        public static event StatusAddedOnTowerCallback OnStatusAddedOnTower;

        public delegate void StatusRemoveFromTowerCallback(Transform targetTower, int idOfRemovedStatus);
        public static event StatusRemoveFromTowerCallback OnStatusRemovedFromTower;

        public delegate void StatusOddsForActivationBoostCallback(Transform targetTower, int idOfBoostedStatus);
        public static event StatusOddsForActivationBoostCallback OnStatusOddsForActivationBoost;

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
            UIStatusUpgradeManager.OnUIRequestStatusUpgradeCallback += TryBoostStatusOddsForActivation;
        }

        private void OnDisable()
        {
            ElementDataApplier.OnElementDataAppliedOnTower -= AddStatus;
            ElementDataApplier.OnElementDataUnappliedFromTower -= RemoveStatus;

            EnemyHitDetection.OnEnemyHit -= TryToInflictStatus;
            UIStatusUpgradeManager.OnUIRequestStatusUpgradeCallback -= TryBoostStatusOddsForActivation;
        }

        //Gives status with base stats to the tower 
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
                bool hasFoundIndexOfStatusToRemove = statusToInflictTracker.CurrentStatusToInflict[i].type == statusToRemove.type;
                if (!hasFoundIndexOfStatusToRemove)
                    continue;

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

            StatusRollMaker statusRollMaker = attackingTower.GetComponent<StatusRollMaker>();
            if (statusRollMaker == null)
                return;

            bool doesAttackingTowerHaveAtLeastOneStatus = statusToInflictTracker.CurrentStatusToInflict.Count > 0 ? true : false;
            if (!doesAttackingTowerHaveAtLeastOneStatus)            
                return;      
           
            foreach(var status in statusToInflictTracker.CurrentStatusToInflict)
            {
                bool hasRollMakerAlreadyTriedToTriggerAllStatus = statusRollMaker.HasTriedToTriggerAllStatus ? true : false;
                if (hasRollMakerAlreadyTriedToTriggerAllStatus)
                {
                    //Reset value after enemy attack
                    statusRollMaker.HasTriedToTriggerAllStatus = false;
                }
                   
                bool canInflictStatus = (status.rollOutcome == RollOutcome.SUCCESS) ? true : false;
                if (!canInflictStatus)
                    continue;
      
                InflictedStatusActivator inflictedStatusActivator = enemy.GetComponent<InflictedStatusActivator>();
                inflictedStatusActivator.InflictStatus(status.type);
            }
        }

        public void TryBoostStatusOddsForActivation(Transform targetTower, int targetedStatusID)
        {
            StatusToInflictTracker statusToInflictTracker = targetTower.GetComponent<StatusToInflictTracker>();
            foreach(var status in statusToInflictTracker.CurrentStatusToInflict)
            {
                if (status.id != targetedStatusID)
                    continue;

                //TODO
                bool hasEnoughElementalPoints = MoneyManager.Instance.Money <= 1000 ? true : false;
                if (hasEnoughElementalPoints)
                    return;

                bool isStatusAlreadyMaxed = status.currentOddsForActivation >= 0.13f ? true : false;
                if (status.currentOddsForActivation >= 0.13f)
                    return;
               
                status.BoostOddsForActivation();
                OnStatusOddsForActivationBoost?.Invoke(targetTower, targetedStatusID);               
            }
        }
    }
}
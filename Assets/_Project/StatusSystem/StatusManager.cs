using System;
using System.Collections;
using System.Collections.Generic;
using TD.ElementSystem;
using TD.Entities.Enemies;
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
        public StatusType type;
        private float probabilityOfActivation;
        public float baseProbabilityOfActivation = 1f;

        public Status(StatusType _type, float _probabilityOfActivation)
        {
            type = _type;
            probabilityOfActivation = _probabilityOfActivation;
        }


        public Status(StatusType _statusType)
        {
            type = _statusType;
            probabilityOfActivation = baseProbabilityOfActivation;
        }

        public Status()
        {
            type = StatusType.None;
            probabilityOfActivation = 0;
        }

     

        public bool TryTrigger()
        {
            float result = Random.Range(0f, 1f);
            Debug.Log("probability of activation -> " + probabilityOfActivation);
            Debug.Log("result -> " + result);
            if (result <= probabilityOfActivation)
            {
                return true;
            }
            else
            {
                return false;
            } 
        }

    }

    public class StatusManager : MonoBehaviour
    {
        private Dictionary<TowerElement, Status> statusTable;

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
        }

        public void RemoveStatus(Transform tower, TowerElement element)
        {
            StatusToInflictTracker statusToInflictTracker = tower.GetComponent<StatusToInflictTracker>();
            StatusType statusType = statusTable[element].type;
            Status statusToRemove = new Status(statusType);
            int indexOfStatusToRemove = 0;
            for(int i = 0; i < statusToInflictTracker.CurrentStatusToInflict.Count; i++)
            {
                if (!(statusToInflictTracker.CurrentStatusToInflict[i].type == statusToRemove.type))
                    return;

                indexOfStatusToRemove = i;
                
            }
            statusToInflictTracker.CurrentStatusToInflict.RemoveAt(indexOfStatusToRemove);
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
                bool canInflictStatus = (status.TryTrigger() == true) ? true : false;
                if (!canInflictStatus)
                {
                    return;
                }
      
                InflictedStatusActivator inflictedStatusActivator = enemy.GetComponent<InflictedStatusActivator>();
                inflictedStatusActivator.InflictStatus(status.type);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using TD.Entities.Enemies;
using TD.Entities.Towers.States;
using TD.StatusSystem;
using UnityEngine;
using TD.Utilities;

namespace TD.Entities.Towers
{

    [Serializable]
    public class RollSuccessAnim
    {
        public GameObject rollSuccessAnimObj;
        public Animator animator;
    }



    [Serializable]
    public class LuckbarGoData
    {
        public GameObject wholeObj;
        public GameObject rollSuccessObj;
        public GameObject rollFailObj;
        public RollSuccessAnim fireAnim;
        public RollSuccessAnim earthAnim;
        public RollSuccessAnim windAnim;
        public bool wasThePreviousRollASuccess;

        public void SaveSuccess()
        {
            wasThePreviousRollASuccess = true;
        }
        public void PlayRollSuccessAnim(StatusType statusType)
        {
            rollSuccessObj.SetActive(true);
            
            switch (statusType)
            {
                case StatusType.Burned:
                    PlayRollSuccessFire();
                    break;
                case StatusType.Stuck:
                    PlayRollSuccessEarth();
                    break;
                case StatusType.Winded:
                    PlayRollSuccessWind();
                    break;
                default:
                    break;
            }
        }
        public void PlayRollSuccessFire()
        {
            fireAnim.rollSuccessAnimObj.SetActive(true);
            earthAnim.rollSuccessAnimObj.SetActive(false);
            windAnim.rollSuccessAnimObj.SetActive(false);

            fireAnim.animator.Play("Fire");
        }
        public void PlayRollSuccessEarth()
        {
            fireAnim.rollSuccessAnimObj.SetActive(false);
            earthAnim.rollSuccessAnimObj.SetActive(true);
            windAnim.rollSuccessAnimObj.SetActive(false);

            earthAnim.animator.Play("Earth");
        }
        public void PlayRollSuccessWind()
        {
            fireAnim.rollSuccessAnimObj.SetActive(false);
            earthAnim.rollSuccessAnimObj.SetActive(false);
            windAnim.rollSuccessAnimObj.SetActive(true);

            windAnim.animator.Play("Wind");
        }
    }

    public class LuckbarsManager : MonoBehaviour
    {
        [SerializeField] private Transform towerHolder;
        [SerializeField] private LuckbarGoData[] luckbars;

       
        
        [Header("Luckbars dice icon colors match table")]
        [SerializeField] private List<StatusType> statusTypeKeys;
        [SerializeField] private List<Color> colorsValues;

       

        private void OnEnable()
        {
            StatusManager.OnStatusAddedOnTower += TryActivateLuckbarAndLinkItToStatusAndManageSave;
            StatusManager.OnStatusRemovedFromTower += TryDesactivateLinkedLuckbar;
            StatusManager.OnStatusOddsForActivationBoost += TryUpdateAllActivationPointHeights;
            StatusRollMaker.OnRollPerformed += TryUpdateLuckbars;
            EnemyHitDetection.OnEnemyHit += TryReactivateLuckbar;
        }

       
        private void OnDisable()
        {
            StatusManager.OnStatusAddedOnTower -= TryActivateLuckbarAndLinkItToStatusAndManageSave;
            StatusManager.OnStatusRemovedFromTower -= TryDesactivateLinkedLuckbar;
            StatusManager.OnStatusOddsForActivationBoost -= TryUpdateAllActivationPointHeights;
            StatusRollMaker.OnRollPerformed -= TryUpdateLuckbars;
            EnemyHitDetection.OnEnemyHit -= TryReactivateLuckbar;
        }

        public void TryActivateLuckbarAndLinkItToStatusAndManageSave(Transform targetTower, int idOfAddedStatus)
        {
            if (targetTower != towerHolder)
                return;

            StatusToInflictTracker statusToInflictTracker = targetTower.GetComponent<StatusToInflictTracker>();
            if (statusToInflictTracker == null)
                return;
        
            foreach (var luckbar in luckbars)
            {
                //Goal is to find an inactive luckbar
                if (luckbar.wholeObj.activeSelf)
                    continue;

                //found !
                foreach (var status in statusToInflictTracker.CurrentStatusToInflict)
                {
                    //Goal is to get the data of added status using id in param
                    if (status.id != idOfAddedStatus)
                        continue;

                    //found!
                    //we activate the inactive luckbar
                    luckbar.wholeObj.SetActive(true);

                    //we get+set the height of activation that the tower needs to reach to activate its status
                    LuckbarBehaviour luckbarBehaviour = luckbar.wholeObj.GetComponent<LuckbarBehaviour>();
                    float percentageOfBarHeightToReach = 1 - status.currentOddsForActivation;
                    luckbarBehaviour.UpdateActivationPointHeight(percentageOfBarHeightToReach);
           
                    //we link the luckbar to the status 
                    luckbarBehaviour.Link(idOfAddedStatus);

                    //we set the color of the luckbar dice icon based on its linked status
                    Dictionary<StatusType, Color> luckbarDiceIconColorsMatchTable =
                        Utils.CreateDictionary(statusTypeKeys, colorsValues);
                    luckbarBehaviour.SetDiceIconColor(luckbarDiceIconColorsMatchTable[status.type]);

                    //we check if the tower (is charging its attack/is in attack state) while we give it the status..
                    //and we check if the roll that was made before the tower started to (charge its attack, dash on target) was a success..
                    //meaning there is a save for this succees that we must apply 
                    TowerStateSwitcher towerStateSwitcher = towerHolder.GetComponent<TowerStateSwitcher>();
                    bool isTowerInChargingAttackState = towerStateSwitcher.CurrentTowerState == TowerState.ChargingAttack;
                    bool isTowerInAttackState = towerStateSwitcher.CurrentTowerState == TowerState.Attacking;
                    bool wasThePreviousRollASuccess = luckbar.wasThePreviousRollASuccess == true ? true : false;
                    if ( (isTowerInChargingAttackState  || isTowerInAttackState) && wasThePreviousRollASuccess)
                    {
                        status.rollOutcome = RollOutcome.SUCCESS;
                        status.canInflict = true;
                        luckbar.PlayRollSuccessAnim(status.type);
                    }
                    break;
                }
                break;
            }
        }

       
        public void TryDesactivateLinkedLuckbar(Transform targetTower, int idOfRemovedStatus)
        {
            if (targetTower != towerHolder)
                return;

            foreach (var luckbar in luckbars)
            {
                LuckbarBehaviour luckbarBehaviour = luckbar.wholeObj.GetComponent<LuckbarBehaviour>();
                if (!(luckbarBehaviour.isActiveAndEnabled && luckbarBehaviour.IdOfLinkedStatus == idOfRemovedStatus))
                    continue;

                luckbar.wholeObj.SetActive(false);
                break;
            }
        }
        public void TryUpdateLuckbars(Transform targetTower, int targetStatusId)
        {
            if (targetTower != towerHolder)
                return;

            StatusToInflictTracker statusToInflictTracker = targetTower.GetComponent<StatusToInflictTracker>();
            if (statusToInflictTracker == null)
                return;

            //Goal is to find the luckbar linked to the target status id in function param
            foreach (var luckbar in luckbars)
            {
                LuckbarBehaviour luckbarBehaviour = luckbar.wholeObj.GetComponent<LuckbarBehaviour>();
                if (luckbarBehaviour.IdOfLinkedStatus != targetStatusId)
                    continue;

                //found!

                //Goal is to get the datas of the status using its Id
                foreach (var status in statusToInflictTracker.CurrentStatusToInflict)
                {
                    if (status.id != targetStatusId)
                        continue;

                    //found!

                    //we check if the roll was a success or a failure, and we act accordingly
                    switch (status.rollOutcome)
                    {
                        case RollOutcome.FAIL:
                            float rollResultGaugeConversion = 1 - status.rollResult;
                            luckbarBehaviour.UpdateBarGauge(rollResultGaugeConversion);
                            break;
                        case RollOutcome.SUCCESS:
                            luckbar.PlayRollSuccessAnim(status.type);
                            luckbar.rollFailObj.SetActive(false);
                            luckbar.SaveSuccess();
                            break;
                        default:
                            break;
                    }   
                }
            }

        }
        public void TryReactivateLuckbar(Transform enemy, Transform attackingTower, Vector3 hitPosition)
        {

            if (attackingTower != towerHolder)
                return;

            LockTargetState lockTargetState = attackingTower.GetComponent<LockTargetState>();
            if (lockTargetState == null)
                return;


            StatusToInflictTracker statusToInflictTracker = attackingTower.GetComponent<StatusToInflictTracker>();
            if (statusToInflictTracker == null)
                return;


            if (statusToInflictTracker.CurrentStatusToInflict.Count <= 0)
                return;

            foreach (var status in statusToInflictTracker.CurrentStatusToInflict)
            {
                foreach (var luckbar in luckbars)
                {
                    if (luckbar.rollSuccessObj == null && luckbar.rollFailObj == null)
                        continue;

                    LuckbarBehaviour luckbarBehaviour = luckbar.wholeObj.GetComponent<LuckbarBehaviour>();
                    bool isRollSuccessObjActive = (luckbar.rollSuccessObj.activeSelf == true) ? true : false;
                    bool isLuckbarLinkedToStatus = luckbarBehaviour.IdOfLinkedStatus == status.id;
                    if (isRollSuccessObjActive && isLuckbarLinkedToStatus)
                    {
                        luckbar.rollSuccessObj.SetActive(false);
                        luckbar.rollFailObj.SetActive(true);

                        //Reset roll success save after last dash
                        AttackState attackState = towerHolder.GetComponent<AttackState>();
                        if (attackState == null)
                            break;

                        bool hasTowerLandedAllItsDashes = attackState.NbOfHitLanded >= attackState.NbOfBonusDash + 1;
                        if (!hasTowerLandedAllItsDashes)
                            break;

                        luckbar.wasThePreviousRollASuccess = false;
                        break;
                    }
                }
                break;

            }
        }
        public void TryUpdateAllActivationPointHeights(Transform targetTower, int idOfBoostedStatus)
        {
            if (targetTower != towerHolder)
                return;

            StatusToInflictTracker statusToInflictTracker = targetTower.GetComponent<StatusToInflictTracker>();
            if (statusToInflictTracker == null)
                return;


            //Goal is to find the luckbar linked to te target status id in function param
            foreach (var luckbar in luckbars)
            {
                LuckbarBehaviour luckbarBehaviour = luckbar.wholeObj.GetComponent<LuckbarBehaviour>();
                if (luckbarBehaviour.IdOfLinkedStatus != idOfBoostedStatus)
                    continue;

                //found!
                foreach (var status in statusToInflictTracker.CurrentStatusToInflict)
                {
                    //Goal is to get the data of boosted status using targetTower in param
                    if (status.id != idOfBoostedStatus)
                        continue;

                    //found!
                    //we get+set the height of activation that the tower needs to reach to activate its status
                    float percentageOfBarHeightToReach = 1 - status.currentOddsForActivation;
                    luckbarBehaviour.UpdateActivationPointHeight(percentageOfBarHeightToReach);
                    break;
                }
                break;
            }
        }
    }
}
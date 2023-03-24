using System.Collections.Generic;
using TD.Entities.Enemies;
using TD.Entities.Towers;
using TD.Entities.Towers.States;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace TD.TowersManager.UselessAttackStopper
{
    public class UselessAttackStopper : MonoBehaviour
    {
        [SerializeField] private List<Transform> towersInAttackState; 
      
        private void OnEnable()
        {
            TowerStateSwitcher.OnTowerEnterState += UpdateTarget;
        }

        private void OnDisable()
        {
            TowerStateSwitcher.OnTowerEnterState -= UpdateTarget;
        }

        private void UpdateTarget(Transform tower, TowerState state)
        {
          
            bool doesTowerExist = tower != null;
            if (!doesTowerExist)
                return;

            bool isTowerAttacking = state == TowerState.Attacking;
            if (isTowerAttacking)
            {
                //Debug.Log("ok");
                Transform target = tower.GetComponent<LockTargetState>().Target;

                //make a list of towers that share the same target as tower
                List<Transform> towersWithSameTarget = FindTowersWithSameTargetAs(tower);

                //add tower
                towersWithSameTarget.Add(tower);
            

                //FindListOfNecessaryTowersToKillTarget(towersWithSameTarget, target);
                FindListOfNecessaryTowersToKillTargetV2(towersWithSameTarget, target, tower);
                towersInAttackState.Add(tower);
            }
            else 
                towersInAttackState.Remove(tower);


        }

        private List<Transform> FindTowersWithSameTargetAs(Transform tower)
        {
            List<Transform> towersWithSameTargetAsTower = new List<Transform>();

            Transform target = tower.GetComponent<LockTargetState>().Target;
            foreach (Transform mTower in towersInAttackState)
            {
                bool doesTowerExist = mTower != null;
                if (!doesTowerExist)
                    continue;
    
                Transform mTarget = mTower.GetComponent<LockTargetState>().Target;
                bool hasTheSameTargetAsTowerParameter = mTarget == target;
                if (!hasTheSameTargetAsTowerParameter)
                    continue;

                towersWithSameTargetAsTower.Add(mTower);
            }
            return towersWithSameTargetAsTower;
        }

        private void FindListOfNecessaryTowersToKillTarget(List<Transform> towersWithSameTarget, Transform target)
        {
            List<Transform> listOfNecessaryTowersToKillTarget = new List<Transform>();
            int targetHp = target.GetComponent<HealthBehaviour>().CurrentHealth;
            int towersDamageSum = 0;

           
            foreach(var tower in towersWithSameTarget)
            {
                bool isSumOfDamageOfAllTowerInListIsNotEnoughToKillTarget = towersDamageSum < targetHp;
                if (isSumOfDamageOfAllTowerInListIsNotEnoughToKillTarget)
                {
                    AttackState towerAttackState = tower.GetComponent<AttackState>();
                    towersDamageSum += towerAttackState.CurrentDamagePerDash + (towerAttackState.CurrentDamagePerDash * towerAttackState.NbOfBonusDashRemaining);
                    listOfNecessaryTowersToKillTarget.Add(tower);
                }
                else
                {
                    ListOfTargets listOfTargets = tower.GetComponent<ListOfTargets>();
                    listOfTargets.SwitchTargetFrom(target);
                }
            }
        }

        private void FindListOfNecessaryTowersToKillTargetV2(List<Transform> towersWithSameTarget, Transform target, Transform attackingTower)
        {
            List<Transform> listOfNecessaryTowersToKillTarget = new List<Transform>();
            int targetHp = target.GetComponent<HealthBehaviour>().CurrentHealth;
            int sumOfDamagesOfTowersAfterTheirNextDash = 0;
            int dashDecrement = 0;
            int noOfTowersChecked = 0;
            AlmostDeadSignaler almostDeadSignaler = target.GetComponent<AlmostDeadSignaler>();

            if (towersWithSameTarget.Count > 1 && almostDeadSignaler.IsAlmostDead)
            {
                Debug.Log("Wp, my help is not needed here");
                ListOfTargets listOfTargets = attackingTower.GetComponent<ListOfTargets>();
                listOfTargets.SwitchTargetFrom(target);
                return;
            }

            else if (towersWithSameTarget.Count > 1 && !almostDeadSignaler.IsAlmostDead)
            {

                for (int i = 0; i < towersWithSameTarget.Count; i++)
                {
                    if (towersWithSameTarget[i] == attackingTower)
                        continue;

                    AttackState towerAttackState = towersWithSameTarget[i].GetComponent<AttackState>();
                    sumOfDamagesOfTowersAfterTheirNextDash += towerAttackState.CurrentDamagePerDash;
                    noOfTowersChecked++;

                    if (sumOfDamagesOfTowersAfterTheirNextDash >= targetHp && noOfTowersChecked < towersWithSameTarget.Count - 1)
                    {
                        Debug.Log("Wp, my help is not needed here");
                        ListOfTargets listOfTargets = attackingTower.GetComponent<ListOfTargets>();
                        listOfTargets.SwitchTargetFrom(target);
                        return;

                    }

                    //Debug.Log("Without counting me, the other towers will make up for " + sumOfDamagesOfTowersAfterTheirNextDash +
                    //  " dmg to the enemy ");
                }
            }

            List<Transform> towersWithDashesLeft = new List<Transform>();
            int noOfTowersWithDashesLeft = 0;

  
            foreach (var tower in towersWithSameTarget)
            {
                if (tower == attackingTower)
                    continue;

                AttackState towerAttackState = tower.GetComponent<AttackState>();
                int noOfDashesLeft = towerAttackState.NbOfBonusDash;
                if (noOfDashesLeft > 0)
                {
                    towersWithDashesLeft.Add(tower);
                    noOfTowersWithDashesLeft++;
                }
            }

            if(noOfTowersWithDashesLeft <= 0) //if list is empty
            {
                //it means the attacking towers needs to attack target
                listOfNecessaryTowersToKillTarget.Add(attackingTower);
                return;
            }
            else //if list is not empty -> there are towers that got dashes left
            {
             
                int sumOfDamagesOfTowersThatGotDashesLeftAfterTheirNextDash = sumOfDamagesOfTowersAfterTheirNextDash;
                if(sumOfDamagesOfTowersAfterTheirNextDash >= targetHp)
                {
                  
                    ListOfTargets listOfTargets = attackingTower.GetComponent<ListOfTargets>();
                    listOfTargets.SwitchTargetFrom(target);
                    return;
                }

                while (towersWithDashesLeft.Count > 0)
                {                 
                    if (sumOfDamagesOfTowersAfterTheirNextDash >= targetHp)
                    {
                        ListOfTargets listOfTargets = attackingTower.GetComponent<ListOfTargets>();
                        listOfTargets.SwitchTargetFrom(target);
                        return;
                    }

                    foreach (var towerWithDashesLeft in towersWithDashesLeft.ToArray())
                    {
                        AttackState attackState = towerWithDashesLeft.GetComponent<AttackState>();
                        int currentDamagePerDash = attackState.CurrentDamagePerDash;
                        sumOfDamagesOfTowersThatGotDashesLeftAfterTheirNextDash += currentDamagePerDash;
                      
                    }

                    dashDecrement++;

                    foreach (var tower in towersWithDashesLeft.ToArray())
                    {
                        AttackState towerAttackState = tower.GetComponent<AttackState>();
                        int mNoOfDashLeft = towerAttackState.NbOfBonusDash - dashDecrement;
                        if (mNoOfDashLeft <= 0)
                        {
                            towersWithDashesLeft.Remove(tower);
                        }
                    }
                }

                listOfNecessaryTowersToKillTarget.Add(attackingTower);
            }

        }
    }
}

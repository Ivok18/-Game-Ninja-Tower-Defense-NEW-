using System.Collections.Generic;
using TD.Entities.Enemies;
using TD.Entities.Towers;
using TD.Entities.Towers.States;
using UnityEngine;

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
                Transform target = tower.GetComponent<LockTargetState>().Target;

                //make a list of towers that share the same target as tower
                List<Transform> towersWithSameTarget = FindTowersWithSameTargetAs(tower);

                //add tower
                towersWithSameTarget.Add(tower);

                FindListOfNecessaryTowersToKillTarget(towersWithSameTarget, target);
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
    }
}

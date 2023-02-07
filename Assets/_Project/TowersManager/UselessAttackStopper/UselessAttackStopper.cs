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
            if(tower!=null)
            {
                if (state == TowerState.Attacking)
                {
                    Transform target = tower.GetComponent<LockTargetState>().Target;

                    //make a list of towers that share the same target as tower
                    List<Transform> towersWithSameTarget = FindTowersWithSameTargetAs(tower);

                    //add tower
                    towersWithSameTarget.Add(tower);

                    FindListOfNecessaryTowersToKillTarget(towersWithSameTarget,target);
                    towersInAttackState.Add(tower);
                }
                else towersInAttackState.Remove(tower);
            }
            
        }

        private List<Transform> FindTowersWithSameTargetAs(Transform tower)
        {
            List<Transform> towersWithSameTargetAsTower = new List<Transform>();

            Transform target = tower.GetComponent<LockTargetState>().Target;
            foreach (Transform mTower in towersInAttackState)
            {
                if(mTower != null)
                {
                    Transform mTarget = mTower.GetComponent<LockTargetState>().Target;
                    if (mTarget == target) towersWithSameTargetAsTower.Add(mTower);
                }
                
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
                if(towersDamageSum < targetHp)
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

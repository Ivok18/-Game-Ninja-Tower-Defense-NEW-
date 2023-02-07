using TD.Entities.Towers.States;
using TD.TowersManager.Storer;
using UnityEngine;


//It gives a stat boost to a tower, depending on the element it receives
namespace TD.ElementSystem
{
    public class ElementBoostManager : MonoBehaviour
    {
        private void OnEnable()
        {
            ElementDataApplier.OnElementDataAppliedOnTower += BoostTower;
            ElementDataApplier.OnElementDataUnappliedFromTower += RemoveBoost;
        }

        private void OnDisable()
        {
            ElementDataApplier.OnElementDataAppliedOnTower -= BoostTower;
            ElementDataApplier.OnElementDataUnappliedFromTower -= RemoveBoost;
        }

        public void BoostTower(Transform towerToBoost, TowerElement elementBoost, int elementCost)
        {
            //Find boosted tower in list of deployed towers
            foreach (Transform tower in TowerStorer.Instance.DeployedTowers)
            {
                if(tower == towerToBoost)
                {
                    //Get the scripts holding all the stats
                    //We use one of them depending on the element boost we wanna give
                    AttackState attackState = towerToBoost.GetComponent<AttackState>();
                    ChargeAttackState chargeAttackState = towerToBoost.GetComponent<ChargeAttackState>();

                    if(elementBoost == TowerElement.Fire)
                    {
                        attackState.CurrentDashSpeed = attackState.BaseDashSpeed + attackState.ElementBonusDashSpeed;
                        chargeAttackState.CurrentTimeBetweenAttacks = chargeAttackState.BaseTimeBetweenAttacks 
                                                                    + chargeAttackState.ElementBonusTimeBetweenAttacks;
                        
                    }
                    else if(elementBoost == TowerElement.Earth)
                    {
                        attackState.CurrentDamagePerDash = attackState.BaseDamagePerDash 
                                                         + attackState.ElementBonusDamagePerDash;
                    } 
                }
            }
        }

        public void RemoveBoost(Transform towerToRemoveBoostFrom, TowerElement elementBoostToRemove)
        {
            //Find boosted tower on which we wanna remove an element boost in the list of deployed towers
            foreach (Transform tower in TowerStorer.Instance.DeployedTowers)
            {
                if (tower == towerToRemoveBoostFrom)
                {
                    //Get the scripts holding all the stats
                    //We use one of them depending on the element boost we wanna remove
                    AttackState attackState = towerToRemoveBoostFrom.GetComponent<AttackState>();
                    ChargeAttackState chargeAttackState = towerToRemoveBoostFrom.GetComponent<ChargeAttackState>();

                    if (elementBoostToRemove == TowerElement.Fire)
                    {
                        attackState.CurrentDashSpeed = attackState.BaseDashSpeed;
                        chargeAttackState.CurrentTimeBetweenAttacks = chargeAttackState.BaseTimeBetweenAttacks;
                    }
                    else if (elementBoostToRemove == TowerElement.Earth)
                    {
                        attackState.CurrentDamagePerDash = attackState.BaseDamagePerDash;                                                    
                    }
                }
            }
        }
    }
}
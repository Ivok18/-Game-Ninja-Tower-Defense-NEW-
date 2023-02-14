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

        public void BoostTower(Transform towerToBoost, ElementScriptableObject elementBoostData)
        {
            //Get all scripts implementing IElementBoost interface that are attached to tower 
            IElementBoost[] IelementBoostList = towerToBoost.GetComponents<IElementBoost>();

            //Call boost on each script
            //Elements that doesnt correspond to the one we want to apply on tower will do nothing
            //Target element will apply its boost
            foreach(var ielementBoost in IelementBoostList)
            {
                ielementBoost.Boost(elementBoostData);
            }

            #region old
            //Find boosted tower in list of deployed towers
            /*foreach (Transform tower in TowerStorer.Instance.DeployedTowers)
            {
                bool isItTheTowerToBoost = tower == towerToBoost;
                if (!isItTheTowerToBoost) 
                    continue;
             

                //Get the scripts holding all the stats
                //We use one of them depending on the element boost we wanna give
                AttackState attackState = towerToBoost.GetComponent<AttackState>();
                ChargeAttackState chargeAttackState = towerToBoost.GetComponent<ChargeAttackState>();

                if(elementBoostData.Element == TowerElement.Fire)
                {
                    attackState.CurrentDashSpeed = attackState.BaseDashSpeed + attackState.ElementBonusDashSpeed;
                    chargeAttackState.CurrentTimeBetweenAttacks = chargeAttackState.BaseTimeBetweenAttacks 
                                                                + chargeAttackState.ElementBonusTimeBetweenAttacks;
                        
                }
                else if(elementBoostData.Element == TowerElement.Earth)
                {
                    attackState.CurrentDamagePerDash = attackState.BaseDamagePerDash 
                                                        + attackState.ElementBonusDamagePerDash;
                }     */
            #endregion
        }
    

        public void RemoveBoost(Transform towerToRemoveBoostFrom, TowerElement elementBoostToRemove)
        {
            //Get all scripts implementing IElementBoost interface that are attached to tower 
            IElementBoost[] IelementBoostList = towerToRemoveBoostFrom.GetComponents<IElementBoost>();

            //Call boost on each script
            //Elements that doesnt correspond to the one we want to remove on tower will do nothing
            //Target element will remove its boost
            foreach (var ielementBoost in IelementBoostList)
            {
                ielementBoost.RemoveBoost(elementBoostToRemove);
            }

            #region old
            //Find boosted tower on which we wanna remove an element boost in the list of deployed towers
            /*foreach (Transform tower in TowerStorer.Instance.DeployedTowers)
            {
                if (tower != towerToRemoveBoostFrom)
                    continue;
                
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
            */
            #endregion
        }
    }
}
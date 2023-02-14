using UnityEngine;
using TD.Entities.Towers.States;
using TD.ElementSystem;

namespace TD.Entities.Towers
{
    public class TowerVarsSetter : MonoBehaviour
    {
        [SerializeField] private TowerScriptableObject towerScriptableObject;

        private ChargeAttackState chargeAttackState;
        private AttackState attackState;
        private RadiusDetectionBehaviour radiusDetectBehaviour;
        private TowerTypeAccessor towerTypeAccessor;

        private void Awake()
        {
            IFireBoost fireBoost = GetComponent<IFireBoost>();
            IEarthBoost earthBoost = GetComponent<IEarthBoost>();
            chargeAttackState = GetComponent<ChargeAttackState>();
            attackState = GetComponent<AttackState>();
            radiusDetectBehaviour = GetComponent<RadiusGetter>().RadiusTransform.GetComponent<RadiusDetectionBehaviour>();
            towerTypeAccessor = GetComponent<TowerTypeAccessor>();

            if(chargeAttackState!=null)
            {
                chargeAttackState.BaseTimeBetweenAttacks = towerScriptableObject.BaseAttackRate;
                //chargeAttackState.BoostTimeBetweenAttacks = towerScriptableObject.ElementBonusAttackRate;
            }

            if(fireBoost != null)
            {
                fireBoost.BoostTimeBetweenAttacks = towerScriptableObject.ElementBonusAttackRate;
                fireBoost.BoostDashSpeed = towerScriptableObject.ElementBonusDashSpeed;
            }

            if(earthBoost != null)
            {
                earthBoost.BoostDamagePerDash = towerScriptableObject.ElementBonusDamagePerDash;
            }

            
            
            if(attackState!=null)
            {
                attackState.BaseDamagePerDash = towerScriptableObject.BaseDamagePerDash;
                //attackState.BoostDamagePerDash = towerScriptableObject.ElementBonusDamagePerDash;

                attackState.BaseDashSpeed = towerScriptableObject.BaseDashSpeed;
                //attackState.BoostDashSpeed = towerScriptableObject.ElementBonusDashSpeed;

                attackState.NbOfBonusDash = towerScriptableObject.NoOfBonusDash;
            }

            if(radiusDetectBehaviour!=null)
            {
                radiusDetectBehaviour.Radius = towerScriptableObject.Radius;
            }

            if(towerTypeAccessor!=null)
            {
                towerTypeAccessor.TowerType = towerScriptableObject.TowerType;
            }
        }

       
    }
}

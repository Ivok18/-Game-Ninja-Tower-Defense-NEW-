using UnityEngine;
using TD.Entities.Towers.States;

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
            chargeAttackState = GetComponent<ChargeAttackState>();
            attackState = GetComponent<AttackState>();
            radiusDetectBehaviour = GetComponent<RadiusGetter>().RadiusTransform.GetComponent<RadiusDetectionBehaviour>();
            towerTypeAccessor = GetComponent<TowerTypeAccessor>();

            if(chargeAttackState!=null)
            {
                chargeAttackState.BaseTimeBetweenAttacks = towerScriptableObject.BaseAttackRate;
                chargeAttackState.ElementBonusTimeBetweenAttacks = towerScriptableObject.ElementBonusAttackRate;
            }
            
            if(attackState!=null)
            {
                attackState.BaseDamagePerDash = towerScriptableObject.BaseDamagePerDash;
                attackState.ElementBonusDamagePerDash = towerScriptableObject.ElementBonusDamagePerDash;

                attackState.BaseDashSpeed = towerScriptableObject.BaseDashSpeed;
                attackState.ElementBonusDashSpeed = towerScriptableObject.ElementBonusDashSpeed;

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

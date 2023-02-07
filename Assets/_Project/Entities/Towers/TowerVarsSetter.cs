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

            chargeAttackState.BaseTimeBetweenAttacks = towerScriptableObject.BaseAttackRate;
            chargeAttackState.ElementBonusTimeBetweenAttacks = towerScriptableObject.ElementBonusAttackRate;

            attackState.BaseDamagePerDash = towerScriptableObject.BaseDamagePerDash;
            attackState.ElementBonusDamagePerDash = towerScriptableObject.ElementBonusDamagePerDash;

            radiusDetectBehaviour.Radius = towerScriptableObject.Radius;

            attackState.BaseDashSpeed = towerScriptableObject.BaseDashSpeed;
            attackState.ElementBonusDashSpeed = towerScriptableObject.ElementBonusDashSpeed;

            attackState.NbOfBonusDash = towerScriptableObject.NoOfBonusDash;

            towerTypeAccessor.TowerType = towerScriptableObject.TowerType;
        }

       
    }
}

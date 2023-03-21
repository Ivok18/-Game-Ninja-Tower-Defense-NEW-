using System.Collections;
using System.Collections.Generic;
using TD.Entities.Towers.States;
using UnityEngine;

namespace TD.ElementSystem
{
    public class IFireBoost : MonoBehaviour, IElementBoost
    {
        private AttackState attackState;
        private ChargeAttackState chargeAttackState;
        public float BoostDashSpeed;
        public float BoostTimeBetweenAttacks;

        private void Awake()
        {
            attackState = GetComponent<AttackState>();
            chargeAttackState = GetComponent<ChargeAttackState>();
        }


        public void Boost(ElementScriptableObject elementData)
        {
            if (elementData.Element != TowerElement.Fire)
                return;

            attackState.BoostDashSpeed += BoostDashSpeed;
            chargeAttackState.BoostTimeBetweenAttacks += BoostTimeBetweenAttacks;


        }
        public void RemoveBoost(TowerElement element)
        {
            if (element != TowerElement.Fire)
                return;

            attackState.BoostDashSpeed -= BoostDashSpeed;
            chargeAttackState.BoostTimeBetweenAttacks -= BoostTimeBetweenAttacks;

        }
    }
}

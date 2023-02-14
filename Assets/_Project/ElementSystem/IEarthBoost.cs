using System.Collections;
using System.Collections.Generic;
using TD.Entities.Towers.States;
using UnityEngine;

namespace TD.ElementSystem
{
    public class IEarthBoost : MonoBehaviour, IElementBoost
    {
        private AttackState attackState;
        public int BoostDamagePerDash;

        private void Awake()
        {
            attackState = GetComponent<AttackState>();
        }

        public void Boost(ElementScriptableObject elementData)
        {
            if (elementData.Element != TowerElement.Earth)
                return;

            attackState.BoostDamagePerDash += BoostDamagePerDash;
        }

        public void RemoveBoost(TowerElement element)
        {
            if (element != TowerElement.Earth)
                return;

            attackState.BoostDamagePerDash -= attackState.BaseDamagePerDash;
        }
    }
}
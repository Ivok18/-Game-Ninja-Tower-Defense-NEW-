using System.Collections;
using System.Collections.Generic;
using TD.Entities.Towers.States;
using UnityEngine;

namespace TD.Entities.Towers
{
    public class DummyTowerBehaviour : MonoBehaviour
    {
        private AttackState attackState;

        private void Awake()
        {
            attackState = GetComponent<AttackState>();
        }

        private void Update()
        {
            attackState.NbOfHitLanded = -1;
            attackState.NbOfBonusDash = 10;
        }

        
    }
}
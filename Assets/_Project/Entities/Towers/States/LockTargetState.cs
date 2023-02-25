using TD.Entities.Enemies;
using UnityEngine;


namespace TD.Entities.Towers.States
{
    public class LockTargetState : MonoBehaviour
    {
        public Transform Target;
        public int TargetIndex;

        private TowerStateSwitcher towerStateSwitcher;

        private void Awake()
        {
            towerStateSwitcher = GetComponent<TowerStateSwitcher>();

        }
        void Update()
        {
            if (towerStateSwitcher.CurrentTowerState != TowerState.LockingTarget) 
                return;

            ListOfTargets listOfTargets = GetComponent<ListOfTargets>();
            bool areThereEnemiesToTarget = listOfTargets.EnemiesToAttack.Count > 0;
            if (!areThereEnemiesToTarget)
            {
                towerStateSwitcher.CurrentTowerState = TowerState.Stationary;
            }
            else
            {
                Target = listOfTargets.FindEnemy();
                TargetIndex = listOfTargets.EnemiesToAttack.IndexOf(Target);
               
                ChargeAttackState chargeAttackBehaviour = GetComponent<ChargeAttackState>();
                chargeAttackBehaviour.ChargeAttackBar.Bar.parent.gameObject.SetActive(true);
                towerStateSwitcher.SwitchTo(TowerState.ChargingAttack);
            }
        }

       
    }
}


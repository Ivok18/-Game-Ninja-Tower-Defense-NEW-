using TD.Entities.Enemies;
using UnityEngine;


namespace TD.Entities.Towers.States
{
    public class LockTargetState : MonoBehaviour
    {
        public Transform Target;
        public int TargetIndex;

        private TowerStateSwitcher towerStateSwitcher;
        private ListOfTargets listOfTargets;

        public delegate void TargetLockedCallback(Transform targetLocked, Transform attackingTower);
        public static event TargetLockedCallback OnTargetLock;

        private void Awake()
        {
            towerStateSwitcher = GetComponent<TowerStateSwitcher>();
            listOfTargets = GetComponent<ListOfTargets>();

        }
        void Update()
        {
            if (towerStateSwitcher.CurrentTowerState != TowerState.LockingTarget) 
                return;

         
            bool areThereEnemiesToTarget = listOfTargets.EnemiesToAttack.Count > 0;
            if (!areThereEnemiesToTarget)
            {
                towerStateSwitcher.CurrentTowerState = TowerState.Stationary;
            }
            else
            {
                LockTarget();
                TargetIndex = listOfTargets.EnemiesToAttack.IndexOf(Target);

                ChargeAttackState chargeAttackBehaviour = GetComponent<ChargeAttackState>();
                chargeAttackBehaviour.ChargeAttackBar.Bar.parent.gameObject.SetActive(true);
                towerStateSwitcher.SwitchTo(TowerState.ChargingAttack);
            }
        }

        public void LockTarget()
        {
            Target = listOfTargets.FindEnemy();
            OnTargetLock?.Invoke(Target, transform);
        }

       
    }
}


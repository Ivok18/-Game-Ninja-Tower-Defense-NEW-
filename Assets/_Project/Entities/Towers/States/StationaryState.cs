using TD.Entities.Towers.AttackPattern;
using UnityEngine;

namespace TD.Entities.Towers.States
{
    public class StationaryState : MonoBehaviour
    {
        private Vector2 startPosition;
        private TowerStateSwitcher towerStateSwitcher;
        public Vector2 StartPosition;

        private void OnEnable()
        {
            TowerStateSwitcher.OnTowerEnterState += Reset;
        }

        private void OnDisable()
        {
            TowerStateSwitcher.OnTowerEnterState -= Reset;
        }

        private void Reset(Transform tower, TowerState state)
        {
            bool areWeEnteringSationaryState = state == TowerState.Stationary;
            bool isItTheTowerEnteringStationatyState = tower == transform;
            if (areWeEnteringSationaryState && isItTheTowerEnteringStationatyState)
            {
                //Reset attack pattern
                PatternAttacher patternAttacher = GetComponent<PatternAttacher>();
                patternAttacher.AttachPaternsToParent();
                AttackPatternsStorer followAttackPatternBehaviour = GetComponent<AttackPatternsStorer>();
                followAttackPatternBehaviour.ResetPatternsState();
                AttackState attackState = GetComponent<AttackState>();
                attackState.NextPattern = null;

                //Reset nb of hits landed
                attackState.NbOfHitLanded = 0;


                //go back to start position
                transform.position = StartPosition;

                //only set tower to stationary after it has reached start position
                towerStateSwitcher.CurrentTowerState = TowerState.Stationary;

                

                ChargeAttackState chargeAttackState = GetComponent<ChargeAttackState>();

                //if this tower has let someone else kill an enemy before, show that next enemy will get attacked right back (full bar)
                if (chargeAttackState.TimeUntilNextAttack <= 0)
                {
                    chargeAttackState.ChargeAttackBar.gameObject.SetActive(true);
                }
                    
       
                //reset target 
                LockTargetState lockTargetState = GetComponent<LockTargetState>();
                bool isThereAStillATarget = lockTargetState.Target != null;
                if (isThereAStillATarget) 
                    lockTargetState.Target = null;
              
            }          
        }

        private void Awake()
        {
            towerStateSwitcher = GetComponent<TowerStateSwitcher>();

        }
        private void Start()
        {
            StartPosition = transform.position;
        }
        
        private void Update()
        {
            bool isTowerInStationaryState = towerStateSwitcher.CurrentTowerState == TowerState.Stationary;
            if (!isTowerInStationaryState) 
                return;

            ListOfTargets listOfTargets = GetComponent<ListOfTargets>();
            bool areThereEnemiesToAttack = listOfTargets.EnemiesToAttack.Count > 0;
            if (!areThereEnemiesToAttack) 
                return;

            towerStateSwitcher.SwitchTo(TowerState.LockingTarget);
        }
    }

}

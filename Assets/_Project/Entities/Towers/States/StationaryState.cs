using TD.Entities.Towers.AttackPattern;
using TD.TowersManager.TowerSelectionManager;
using UnityEngine;
using UnityEngine.UI;

namespace TD.Entities.Towers.States
{
    public class StationaryState : MonoBehaviour
    {
        private Vector2 startPosition;
        private TowerStateSwitcher towerStateSwitcher;
        public Vector2 StartPosition;
        private bool isCatalyst;

        public delegate void TargetResetCallback(Transform targetReseted, Transform attackingTower);
        public static event TargetResetCallback OnTargetReset;

        private void OnEnable()
        {
            TowerStateSwitcher.OnTowerEnterState += Reset;
            SelectionAreaBehaviour.OnTowerSelected += TryShowCatalystUI;
        }

        private void OnDisable()
        {
            TowerStateSwitcher.OnTowerEnterState -= Reset;
            SelectionAreaBehaviour.OnTowerSelected -= TryShowCatalystUI;
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
                attackState.IndexOfNextAttackPattern = -1;
                attackState.TotalDamage = 0;

                //Reset nb of hits landed
                attackState.NbOfHitLanded = 0;


                //go back to start position
                transform.position = StartPosition;

                //only set tower to stationary after it has reached start position
                towerStateSwitcher.CurrentTowerState = TowerState.Stationary;

                

                ChargeAttackState chargeAttackState = GetComponent<ChargeAttackState>();
                if(chargeAttackState != null)
                {
                    //if this tower has let someone else kill an enemy before, show that next enemy will get attacked right back (full bar)
                    if (chargeAttackState.TimeUntilNextAttack <= 0)
                    {
                        chargeAttackState.ChargeAttackBar.gameObject.SetActive(true);
                    }

                }



                //reset target 
                LockTargetState lockTargetState = GetComponent<LockTargetState>();
                bool isThereAStillATarget = lockTargetState.Target != null;
                if (isThereAStillATarget)
                {
                    OnTargetReset?.Invoke(lockTargetState.Target, transform);
                    lockTargetState.Target = null; 
                }
                    
              
            }          
        }

        private void TryShowCatalystUI(SelectionAreaBehaviour selection)
        {
            if (this.transform != selection.TowerHolder)
                return;

           //selection.
           

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

            if(!isCatalyst)
            {
                ListOfTargets listOfTargets = GetComponent<ListOfTargets>();
                if (listOfTargets == null)
                    return;

                bool areThereEnemiesToAttack = listOfTargets.EnemiesToAttack.Count > 0;
                if (!areThereEnemiesToAttack)
                    return;

                towerStateSwitcher.SwitchTo(TowerState.LockingTarget);
            }

            else
            {
                //DO NOTHING
            }
           
        }

        public void SetCatalystMode(bool boolean)
        {
            Debug.Log("OK?");
            isCatalyst = boolean;
        }

        
    }

}

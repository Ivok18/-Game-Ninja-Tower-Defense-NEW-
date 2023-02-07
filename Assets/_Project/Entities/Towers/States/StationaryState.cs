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
            if(state == TowerState.Stationary && tower == transform)
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
                if (lockTargetState.Target != null) lockTargetState.Target = null;
              
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
            if (towerStateSwitcher.CurrentTowerState != TowerState.Stationary) return;

            ListOfTargets listOfTargets = GetComponent<ListOfTargets>();
            if (listOfTargets.EnemiesToAttack.Count <= 0) return;
            towerStateSwitcher.SwitchTo(TowerState.LockingTarget);
        }
    }

}

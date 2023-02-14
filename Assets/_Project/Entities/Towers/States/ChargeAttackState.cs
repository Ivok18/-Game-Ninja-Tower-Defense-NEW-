using TD.Entities.Enemies;
using UnityEngine;

namespace TD.Entities.Towers.States
{
    public class ChargeAttackState : MonoBehaviour
    {
        public float TimeUntilNextAttack;
        public ChargeAttackBarBehaviour ChargeAttackBar;
        private TowerStateSwitcher towerStateSwitcher;
        private LockTargetState lockTargetState;
        private ListOfTargets listOfTargets;
        [HideInInspector]
        public float BaseTimeBetweenAttacks;
        [HideInInspector]
        public float BoostTimeBetweenAttacks;
        public float CurrentTimeBetweenAttacks;
    
       


        private void OnEnable()
        {
            TowerStateSwitcher.OnTowerEnterState += ResetNbOfBonusDash;
        }
        private void OnDisable()
        {
            
            TowerStateSwitcher.OnTowerEnterState -= ResetNbOfBonusDash;
        }


        private void ResetNbOfBonusDash(Transform tower, TowerState state)
        {
            if(transform == tower && state == TowerState.ChargingAttack)
            {
                AttackState attackState = GetComponent<AttackState>();
                attackState.NbOfBonusDashRemaining = attackState.NbOfBonusDash;

                //prevents bug
                StationaryState stationary = GetComponent<StationaryState>();
                transform.position = stationary.StartPosition;
            }
        }

        private void Start()
        {
            CurrentTimeBetweenAttacks = BaseTimeBetweenAttacks + BoostTimeBetweenAttacks;
            TimeUntilNextAttack = CurrentTimeBetweenAttacks;
        }
        private void Awake()
        {
            towerStateSwitcher = GetComponent<TowerStateSwitcher>();
            lockTargetState = GetComponent<LockTargetState>();
            listOfTargets = GetComponent<ListOfTargets>();
        }

        void Update()
        {
            CurrentTimeBetweenAttacks = BaseTimeBetweenAttacks + BoostTimeBetweenAttacks;
            if (towerStateSwitcher.CurrentTowerState != TowerState.ChargingAttack) 
                return;
            ChargeAttack();
    
        }

        private void ChargeAttack()
        {
           
            ChargeAttackBar.CurrentValue = TimeUntilNextAttack;
            ChargeAttackBar.MaxValue = CurrentTimeBetweenAttacks;

            bool isTowerStillChargingItsAttack = TimeUntilNextAttack > 0;
            if (isTowerStillChargingItsAttack)
            {
                TimeUntilNextAttack -= Time.deltaTime;

                //if the target is not null while the tower charges its attack ..
                if(lockTargetState.Target!=null)
                {
                    //also if the target is "Dead" while the tower charges its attack..
                    if (lockTargetState.Target.CompareTag("Dead"))
                    {
                        //get list of targets
                        ListOfTargets listOfTargets = GetComponent<ListOfTargets>();

                        //find another enemy to attack, and keep on charging attack
                        lockTargetState.Target = listOfTargets.FindEnemy();
                    }
                    else //if target is still alive during attack charge..
                    {
                        //get movement script ..
                        EnemyMovement targetMovement = lockTargetState.Target.GetComponent<EnemyMovement>();
                        if (targetMovement != null)
                        {
                            //verify if target has been affected by wind element..
                            if (targetMovement.IsWinded)
                            {
                                //if it is the case, do not lock him as target
                                //instead search for another target
                                ListOfTargets listOfTargets = GetComponent<ListOfTargets>();
                                listOfTargets.SwitchTargetFrom(targetMovement.transform);
                            }
                        }
                    }
                }

                //If all enemies in radius are killed during attack charge, do not reset charge and go back to stationary mode
                if(listOfTargets.EnemiesToAttack.Count <= 0)
                {
                    towerStateSwitcher.SwitchTo(TowerState.Stationary);
                }
            }
            else //when attack is ready to be launched ..
            {
                bool doesTargetOfTowerStillExist = lockTargetState.Target != null;
                if (doesTargetOfTowerStillExist)
                {
                    bool isTargetDead = lockTargetState.Target.CompareTag("Dead");
                    if (!isTargetDead) // if the target is still alive, swicth to attack state
                    {
                        TimeUntilNextAttack = CurrentTimeBetweenAttacks;
                        ChargeAttackBar.gameObject.SetActive(false);
                        towerStateSwitcher.SwitchTo(TowerState.Attacking);
                    }
                    else //if the target died, switch to sationary state                                    
                    {
                        towerStateSwitcher.SwitchTo(TowerState.Stationary);
                    }
                }
                else //if target is null when the tower has finished to charge its attack, go back to stationary state
                {
                    towerStateSwitcher.SwitchTo(TowerState.Stationary);
                }
                
            }
        }
    }

}

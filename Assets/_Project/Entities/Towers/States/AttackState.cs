using UnityEngine;
using TD.Entities.Towers.AttackPattern;
using TD.Entities.Enemies;

namespace TD.Entities.Towers.States
{
    public class AttackState : MonoBehaviour
    {
        public int NbOfBonusDashRemaining;
        private TowerStateSwitcher towerStateSwitcher;
        public PatternBehaviour NextPattern;

      
        public int NbOfHitLanded;

        [HideInInspector]
        public int BaseDamagePerDash;
        [HideInInspector]
        public int ElementBonusDamagePerDash;
        [HideInInspector]
        public int CurrentDamagePerDash
            ;
        [HideInInspector]
        public int NbOfBonusDash;

        [HideInInspector]
        public float BaseDashSpeed;
        [HideInInspector]
        public float ElementBonusDashSpeed;
        [HideInInspector]
        public float CurrentDashSpeed;



        private void Awake()
        {
            towerStateSwitcher = GetComponent<TowerStateSwitcher>();
        }

        void Start()
        {
            NbOfBonusDashRemaining = NbOfBonusDash;
            CurrentDashSpeed = BaseDashSpeed;
            CurrentDamagePerDash = BaseDamagePerDash;
        }

        private void OnEnable()
        {
            TowerStateSwitcher.OnTowerEnterState += DetachPatternChildren;
            DamageReceiver.OnEnemyHit += OperateNextAttackMove;
        }

        private void OnDisable()
        {
            TowerStateSwitcher.OnTowerEnterState -= DetachPatternChildren;
            DamageReceiver.OnEnemyHit -= OperateNextAttackMove;

        }

        //On entering attack state, detach attack pattern from player so that he can reach attack patterns 
        private void DetachPatternChildren(Transform tower, TowerState state)
        {
            if(transform == tower && state == TowerState.Attacking)
            {
          
                PatternDetacher patternDetacher = GetComponent<PatternDetacher>();
                patternDetacher.DetachPaternsFromParent(); 
            }
        }

        //Decide whether to continue attacking or go back to stationary state after hitting target
        private void OperateNextAttackMove(Transform enemy,Transform attackingTower)
        {
            if(transform == attackingTower)
            {
                //Avoiding unknow bug yet
                if(NbOfHitLanded < NbOfBonusDash + 1)
                {
                    NbOfHitLanded++;
                    if (NbOfHitLanded != NbOfBonusDash + 1)
                    {
                        //Find next attack pattern to teleport to
                        AttackPatternsStorer followAttackPatternBehaviour = GetComponent<AttackPatternsStorer>();
                        NextPattern = followAttackPatternBehaviour.FindNextPattern();
                        if (NextPattern != null)
                        {
                            //teleport to next pattern
                            transform.position = NextPattern.transform.position;
                            //Debug.Log("AH");

                            //if next pattern overlaps with target (fixed)
                            AttackPatternOverlapTracker attackPatternOverlapTracker = enemy.GetComponent<AttackPatternOverlapTracker>();
                            if (attackPatternOverlapTracker.FindOverlapingPattern(NextPattern) != null)
                            {
                                NextPattern.HasBeenReached = true;

                                //damage enemy
                                DamageReceiver damageReceiver = enemy.GetComponent<DamageReceiver>();
                                damageReceiver.ReceiveDamage(CurrentDamagePerDash, transform);

                                //go to next the next attack pattern if there is any
                                NextPattern = followAttackPatternBehaviour.FindNextPattern();
                                if (NextPattern != null) transform.position = NextPattern.transform.position;
                            }
                        }
                    }
                    else
                    {
                        ChargeAttackState chargeAttackState = GetComponent<ChargeAttackState>();
                        chargeAttackState.TimeUntilNextAttack = chargeAttackState.CurrentTimeBetweenAttacks;
                        towerStateSwitcher.SwitchTo(TowerState.Stationary);
                    }
                }
                else towerStateSwitcher.SwitchTo(TowerState.Stationary);

            }          
        }


        void Update()
        {
            if (towerStateSwitcher.CurrentTowerState != TowerState.Attacking) return;
            Attack();
        }

        private void Attack()
        {
            //Get the target
            LockTargetState lockTargetState = GetComponent<LockTargetState>();
            Transform target = lockTargetState.Target;

            //If target has been destroyed (it may happen when the wave ends)
            if (target == null)
            {
                towerStateSwitcher.SwitchTo(TowerState.Stationary);
                return;
            }
            
            //If target has not been destroyed (it happens when the wave ends)
            if (!target.CompareTag("Dead"))  //If the target is alive..
            {
                //Dash on target
                transform.position = Vector2.MoveTowards(transform.position, target.position, CurrentDashSpeed * Time.deltaTime);

                EnemyMovement enemyMovement = target.GetComponent<EnemyMovement>();
                ListOfTargets listOfTargets = GetComponent<ListOfTargets>();

                //Switch target only if it is not the only one affected by wind element among my list of target
                bool isTargetAffectedByWind = enemyMovement.IsWinded;
                bool hasMultipleTargets = !(listOfTargets.EnemiesToAttack.Count == 1);
                if (isTargetAffectedByWind && hasMultipleTargets)
                {    
                    listOfTargets.SwitchTargetFrom(target);
                }
            }
            else
            {
                ListOfTargets listOfTargets = GetComponent<ListOfTargets>();
                    
                //If tower didnt land all its dashes on target yet and for some reason its target died..
                if (NbOfHitLanded < NbOfBonusDash + 1)
                {
                    if (listOfTargets.EnemiesToAttack.Count > 0) //And if tower has other targets to attack..
                    {
                        //attack them
                        lockTargetState.Target = listOfTargets.FindEnemy();     

                    }
                    else //If there are no target available..
                    {
                        //Go back to staionary state
                        towerStateSwitcher.SwitchTo(TowerState.Stationary);    
                    }
                }
                else
                {
                    towerStateSwitcher.SwitchTo(TowerState.Stationary);
                }
            }
        }
            
    }

}

using UnityEngine;
using TD.Entities.Towers.AttackPattern;
using TD.Entities.Enemies;
using System;
using Unity.Mathematics;
using Random = UnityEngine.Random;

namespace TD.Entities.Towers.States
{
    public class AttackState : MonoBehaviour
    {
        private TowerStateSwitcher towerStateSwitcher;
     
        [Header("Attack pattern")]
        public PatternBehaviour NextPattern;
        public int IndexOfNextAttackPattern = -1;
        
        [Header("Dashes remaining")]
        public int NbOfBonusDashRemaining;

        [Header("Hit counter")]
        public int NbOfHitLanded;  
        
        [Header("Damage per dash")]
        public int CurrentDamagePerDash;
        public int TotalDamage = 0;

        [Header("Dash speed")]
        public float CurrentDashSpeed;


        [HideInInspector] public int BaseDamagePerDash;
        [HideInInspector] public float BaseDashSpeed;
        [HideInInspector] public float BoostDashSpeed;
        [HideInInspector] public int BoostDamagePerDash;
        [HideInInspector] public int NbOfBonusDash;



        private void Awake()
        {
            towerStateSwitcher = GetComponent<TowerStateSwitcher>();
        }

        void Start()
        {
            NbOfBonusDashRemaining = NbOfBonusDash;
            CurrentDashSpeed = BaseDashSpeed + BoostDashSpeed;
            CurrentDamagePerDash = BaseDamagePerDash + BoostDamagePerDash;
        }

        private void OnEnable()
        {
            TowerStateSwitcher.OnTowerEnterState += DetachPatternChildren;
            EnemyHitDetection.OnEnemyHit += OperateNextAttackMove;
        }

        private void OnDisable()
        {
            TowerStateSwitcher.OnTowerEnterState -= DetachPatternChildren;
            EnemyHitDetection.OnEnemyHit -= OperateNextAttackMove;

        }

        //On entering attack state, detach attack pattern from player so that he can reach attack patterns 
        private void DetachPatternChildren(Transform tower, TowerState state)
        {
            bool hastToDetachChildrenPattern = transform == tower;
            bool isAttacking = state == TowerState.Attacking;
            if (hastToDetachChildrenPattern && isAttacking)
            {
                PatternDetacher patternDetacher = GetComponent<PatternDetacher>();
                patternDetacher.DetachPaternsFromParent();
            }
        }

        //Decide whether to continue attacking or go back to stationary state after hitting target
        private void OperateNextAttackMove(Transform enemy, Transform attackingTower, Vector3 hitPosition)
        {
            bool hasToDetermineNextCourseOfAction = transform == attackingTower;
            if (!hasToDetermineNextCourseOfAction) 
                return;

           

            NbOfHitLanded++;
            TotalDamage += CurrentDamagePerDash;
            IndexOfNextAttackPattern++;
            bool
                towerHasCompletedAllItsDashesAfterIncrementOfCounterOfHitsOnTarget = NbOfHitLanded >= NbOfBonusDash + 1;

            if (towerHasCompletedAllItsDashesAfterIncrementOfCounterOfHitsOnTarget)
            {
                ChargeAttackState chargeAttackState = GetComponent<ChargeAttackState>();
                chargeAttackState.TimeUntilNextAttack = chargeAttackState.CurrentTimeBetweenAttacks;
                towerStateSwitcher.SwitchTo(TowerState.Stationary);    
                return;

            }

           
            AttackPatternsStorer attackPatternsStorer = GetComponent<AttackPatternsStorer>();         
            
            //If not,
            //check if next pattern overlaps with target (fixed)
            AttackPatternOverlapTracker attackPatternOverlapTracker = enemy.GetComponent<AttackPatternOverlapTracker>();

            
            bool nextPatternOverlapsWithAnEnemy =
            attackPatternOverlapTracker.FindOverlapingPattern(attackPatternsStorer.Patterns[IndexOfNextAttackPattern]) != null;

            if (!nextPatternOverlapsWithAnEnemy)
            {
                NextPattern = attackPatternsStorer.GetPatternAt(IndexOfNextAttackPattern);
                transform.position = NextPattern.transform.position;
                NextPattern.HasBeenReached = true;
                return;
            }

            //if there is overlap
            //.. directly inflict damage to enemy
            HealthBehaviour healthBehaviour = enemy.GetComponent<HealthBehaviour>();
            healthBehaviour.GetDamage(CurrentDamagePerDash, transform);


            //And go to next attack pattern, if there is any
            if (!attackPatternsStorer.HaveAllPatternsBeenReached())
            {
                //If next pattern after the one that enemy overlaps exists tp to it
                if (IndexOfNextAttackPattern + 1 < attackPatternsStorer.Patterns.Length)
                {
                    NextPattern = attackPatternsStorer.GetPatternAt(IndexOfNextAttackPattern + 1);
                    transform.position = NextPattern.transform.position;
                    NextPattern.HasBeenReached = true;
                }
                //if not tp to the one that enemy overlaps
                else
                {
                    NextPattern = attackPatternsStorer.GetPatternAt(IndexOfNextAttackPattern);
                    transform.position = NextPattern.transform.position;
                    NextPattern.HasBeenReached = true;
                }
            }

            else
            {
                ChargeAttackState chargeAttackState = GetComponent<ChargeAttackState>();
                chargeAttackState.TimeUntilNextAttack = chargeAttackState.CurrentTimeBetweenAttacks;
                towerStateSwitcher.SwitchTo(TowerState.Stationary);
            }

        }
    
     



        void Update()
        {
            CurrentDashSpeed = BaseDashSpeed + BoostDashSpeed;
            CurrentDamagePerDash = BaseDamagePerDash + BoostDamagePerDash;
            if (towerStateSwitcher.CurrentTowerState != TowerState.Attacking) 
                return;

            Attack();
        }

        private void Attack()
        {
            //Get the target
            LockTargetState lockTargetState = GetComponent<LockTargetState>();
            Transform target = lockTargetState.Target;

            //If target has been destroyed (it may happen when the wave ends)
            bool isTargetStillValid = target != null;
            if (!isTargetStillValid)
            {
                towerStateSwitcher.SwitchTo(TowerState.Stationary);
                return;
            }

            //If target has not been destroyed (it happens when the wave ends)
            bool isTargetDead = target.CompareTag("Dead");
            if (!isTargetDead)  //If the target is alive..
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
                bool hasTowerLandedAllItsDashes = NbOfHitLanded >= NbOfBonusDash + 1;
                if (hasTowerLandedAllItsDashes)
                {
                    //Debug.Log("Oh shoot, he died before ): -> " + TotalDamage + " dmg");
                    towerStateSwitcher.SwitchTo(TowerState.Stationary);
                }


                bool towerHasOtherTargets = listOfTargets.EnemiesToAttack.Count > 0;
                if (!towerHasOtherTargets)
                {
                    //Go back to staionary state
                    //Debug.Log("No target anymore?" + TotalDamage + " dmg");
                    towerStateSwitcher.SwitchTo(TowerState.Stationary);
                    return;
                }

                //if tower has other targets to attack..   
                //attack them
                lockTargetState.Target = listOfTargets.FindEnemy();
            }
        }

    }

}
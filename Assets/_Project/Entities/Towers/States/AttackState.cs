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
        public int BoostDamagePerDash;

        public int CurrentDamagePerDash;
        [HideInInspector]
        public int NbOfBonusDash;

        [HideInInspector]
        public float BaseDashSpeed;

        [HideInInspector]
        public float BoostDashSpeed;
    
        public float CurrentDashSpeed;



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
        private void OperateNextAttackMove(Transform enemy, Transform attackingTower)
        {
            bool hasToDetermineNextCourseOfAction = transform == attackingTower;
            if (!hasToDetermineNextCourseOfAction) 
                return;
            


            NbOfHitLanded++;
            bool
                towerHasCompletedAllItsDashesAfterIncrementOfCounterOfHitsOnTarget = NbOfHitLanded >= NbOfBonusDash + 1;

            if (towerHasCompletedAllItsDashesAfterIncrementOfCounterOfHitsOnTarget)
            {
                ChargeAttackState chargeAttackState = GetComponent<ChargeAttackState>();
                chargeAttackState.TimeUntilNextAttack = chargeAttackState.CurrentTimeBetweenAttacks;

                towerStateSwitcher.SwitchTo(TowerState.Stationary);
                return;

            }

            //Find next attack pattern to teleport to
            AttackPatternsStorer followAttackPatternBehaviour = GetComponent<AttackPatternsStorer>();
            NextPattern = followAttackPatternBehaviour.FindNextPattern();
            bool hasNextPattern = NextPattern != null;
            if (!hasNextPattern)
            {
                ChargeAttackState chargeAttackState = GetComponent<ChargeAttackState>();
                chargeAttackState.TimeUntilNextAttack = chargeAttackState.CurrentTimeBetweenAttacks;

                towerStateSwitcher.SwitchTo(TowerState.Stationary);
                return;
            }

            //teleport to next pattern
            transform.position = NextPattern.transform.position;
      

            //if next pattern overlaps with target (fixed)
            AttackPatternOverlapTracker attackPatternOverlapTracker = enemy.GetComponent<AttackPatternOverlapTracker>();

            bool nextPatternOverlapsWithAnEnemy =
                attackPatternOverlapTracker.FindOverlapingPattern(NextPattern) != null;

            if (!nextPatternOverlapsWithAnEnemy)
                return;
       
            NextPattern.HasBeenReached = true;

            //damage enemy
            HealthBehaviour healthBehaviour= enemy.GetComponent<HealthBehaviour>();
            healthBehaviour.GetDamage(CurrentDamagePerDash, transform);

            //go to next the next attack pattern if there is any
            bool thereIsOtherPatternToReachAfterOverlapingPattern =
                followAttackPatternBehaviour.FindNextPattern() != null;
            if (!thereIsOtherPatternToReachAfterOverlapingPattern)
            {
                ChargeAttackState chargeAttackState = GetComponent<ChargeAttackState>();
                chargeAttackState.TimeUntilNextAttack = chargeAttackState.CurrentTimeBetweenAttacks;


                towerStateSwitcher.SwitchTo(TowerState.Stationary);
                return;
            }

            NextPattern = followAttackPatternBehaviour.FindNextPattern();
            transform.position = NextPattern.transform.position;
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
                    towerStateSwitcher.SwitchTo(TowerState.Stationary);
                }


                bool towerHasOtherTargets = listOfTargets.EnemiesToAttack.Count > 0;
                if (!towerHasOtherTargets)
                {
                    //Go back to staionary state
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
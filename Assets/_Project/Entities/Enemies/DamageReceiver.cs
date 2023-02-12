using UnityEngine;
using TD.Entities.Towers.States;
using TD.Entities.Towers;

namespace TD.Entities.Enemies
{
    public class DamageReceiver : MonoBehaviour
    {
        private HealthBehaviour healthBehaviour;
        private RewardGetter rewardGetter;


        public delegate void EnemyHitCallback(Transform enemy, Transform attackingTower);
        public static event EnemyHitCallback OnEnemyHit;

        public delegate void EnemyDeadCallback(Transform enemy, Transform killerTower, float reward);
        public static event EnemyDeadCallback OnEnemyDead;

       

        private void Awake()
        {
            healthBehaviour = GetComponent<HealthBehaviour>();
            rewardGetter = GetComponent<RewardGetter>();
        }

        public void ReceiveDamage(int amount, Transform attacker)
        {
            //Avoids unwanted damages
            AttackState attackState = attacker.GetComponent<AttackState>();
            bool hasTowerNotCompletedAllItsDashesYet = attackState.NbOfHitLanded < attackState.NbOfBonusDash + 1;
            if (hasTowerNotCompletedAllItsDashesYet)
            {
                healthBehaviour.CurrentHealth -= amount;
            }

            //For some reason, putting this line of code here instead of line 38 "fixes" an unknown bug 
            //where the tower get stuck on its targets 
            OnEnemyHit?.Invoke(transform, attacker);

            bool isDead = healthBehaviour.CurrentHealth <= 0;
            if (isDead)
            {
                healthBehaviour.CurrentHealth = 0;

                transform.tag = "Dead";
                gameObject.SetActive(false);
                //spriteGetter.SpriteRenderer.enabled = false;

                OnEnemyDead?.Invoke(transform, attacker, rewardGetter.Reward);

            }
        }

       

        private void OnTriggerStay2D(Collider2D collision)
        {
            bool hasCollidedWithATower = collision.CompareTag("Tower");
            if (!hasCollidedWithATower) 
                return;

            LockTargetState lockTargetState = collision.GetComponent<LockTargetState>();

            //Receive damage from collision if collision is a tower and the enemy holding this script is its target
            //Also, the tower needs to be in attack state in order to inflict damage to its target
            bool isTargetOfTower = lockTargetState.Target == transform;
            if (!isTargetOfTower)
                return;
            
            TowerStateSwitcher towerStateSwitcher = collision.GetComponent<TowerStateSwitcher>();
            bool isTowerAttacking = towerStateSwitcher.CurrentTowerState == TowerState.Attacking;
            if (!isTowerAttacking)
                return;

            AttackState attackState = collision.GetComponent<AttackState>();
            ReceiveDamage(attackState.CurrentDamagePerDash, attackState.transform);

            ShakeBehaviour shakeBehaviour = GetComponent<ShakeBehaviour>();
            shakeBehaviour.StartShake();
        }
      
    }

}

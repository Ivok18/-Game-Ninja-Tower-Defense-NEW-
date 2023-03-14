using UnityEngine;
using TD.Entities.Towers.States;
using TD.Entities.Towers;

namespace TD.Entities.Enemies
{
    public class HealthBehaviour : MonoBehaviour
    {
        public int CurrentHealth;
        public int MaxHealth;
        private RewardGetter rewardGetter;

        private void Start()
        {
            CurrentHealth = MaxHealth;
        }

        public delegate void EnemyDeadCallback(Transform enemy, Transform killerTower, float reward);
        public static event EnemyDeadCallback OnEnemyDead;

        private void Awake()
        {
            rewardGetter = GetComponent<RewardGetter>();
        }

        public void GetDamage(int amount, Transform attacker)
        {
            //Avoids unwanted damages
            AttackState attackState = attacker.GetComponent<AttackState>();
            bool hasTowerNotCompletedAllItsDashesYet = attackState.NbOfHitLanded < attackState.NbOfBonusDash + 1;
            if (hasTowerNotCompletedAllItsDashesYet)
            {
              

                DodgeBehaviour dodgeBehaviour = GetComponent<DodgeBehaviour>();
                if (!dodgeBehaviour.IsDodging)
                {
                    Remove(amount);
                }

                ShakeBehaviour shakeBehaviour = GetComponent<ShakeBehaviour>();
                shakeBehaviour.StartShake();
            }

            bool isDead = CurrentHealth <= 0;
            if (!isDead)
                return;

            

            transform.tag = "Dead";
            gameObject.SetActive(false);
            OnEnemyDead?.Invoke(transform, attacker, rewardGetter.Reward);
        }

        public void Add(int amountToAdd)
        {
            CurrentHealth += amountToAdd;
            Set(CurrentHealth);         
        }

        public void Remove(int amountToRemove)
        {
            CurrentHealth -= amountToRemove;
            Set(CurrentHealth);
        }

        public void Set(int newHealth)
        {
            CurrentHealth = newHealth;
            CurrentHealth = System.Math.Clamp(newHealth, 0, 999999999);
        }
    }
}


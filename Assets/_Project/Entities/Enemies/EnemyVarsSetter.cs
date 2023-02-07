using UnityEngine;

namespace TD.Entities.Enemies
{
    public class EnemyVarsSetter : MonoBehaviour
    {
        [SerializeField] private EnemyScriptableObject enemyScriptableObject;

        private EnemyMovement enemyMovement;
        private HealthBehaviour healthBehaviour;
        private RewardGetter rewardGetter;

        private void Awake()
        {
            enemyMovement = GetComponent<EnemyMovement>();
            healthBehaviour = GetComponent<HealthBehaviour>();
            rewardGetter = GetComponent<RewardGetter>();

            enemyMovement.Speed = enemyScriptableObject.Speed;
            healthBehaviour.MaxHealth = enemyScriptableObject.MaxHealth;
            rewardGetter.Reward = enemyScriptableObject.Reward;
        }

    }
}

using UnityEngine;
using UnityEngine.UI;

namespace TD.PlayerLife
{
    public class LifeManager : MonoBehaviour
    {
        [SerializeField] private int life;
        [SerializeField] private Text lifeText;

        public delegate void PlayerLoseLifeCallback(int currentLife);
        public static event PlayerLoseLifeCallback OnPlayerLoseLife;

        private void Start()
        {
            lifeText.text = life.ToString();
        }

        private void OnEnable()
        {
            OnEnemyBaseCollisionBehaviour.OnEnemyReachBase += LoseLife;
        }
        private void OnDisable()
        {
            OnEnemyBaseCollisionBehaviour.OnEnemyReachBase -= LoseLife;
        }
        private void LoseLife(Transform enemy, int enemyCurrentHealth)
        {
            life -= enemyCurrentHealth;

            if (life <= 0)
                life = 0;

            lifeText.text = life.ToString();
            OnPlayerLoseLife?.Invoke(life);
        }
    }
}

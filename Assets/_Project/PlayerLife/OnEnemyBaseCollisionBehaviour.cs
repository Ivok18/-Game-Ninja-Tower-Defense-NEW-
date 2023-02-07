using TD.Entities.Enemies;
using UnityEngine;

namespace TD.PlayerLife
{
    public class OnEnemyBaseCollisionBehaviour : MonoBehaviour
    {
        public delegate void EnemyReachBaseCallback(Transform enemy, int enemyCurrentHealth);
        public static event EnemyReachBaseCallback OnEnemyReachBase;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Enemy")) return;

            collision.tag = "Dead";
            collision.gameObject.SetActive(false);

            HealthBehaviour enemyHealthComponent = collision.GetComponent<HealthBehaviour>();
            OnEnemyReachBase?.Invoke(collision.transform, enemyHealthComponent.CurrentHealth);

            //Destroy(collision.gameObject);
        }
    }
}

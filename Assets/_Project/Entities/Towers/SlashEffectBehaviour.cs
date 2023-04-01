using System.Collections;
using System.Collections.Generic;
using TD.Entities.Enemies;
using UnityEngine;


namespace TD.Entities.Towers
{
    public class SlashEffectBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject slashPrefab;

        private void OnEnable()
        {
            EnemyHitDetection.OnEnemyHit += PlaySlash;
        }
        private void OnDisable()
        {
            EnemyHitDetection.OnEnemyHit -= PlaySlash;
        }
        

        public void PlaySlash(Transform enemy, Transform attackingTower, Vector3 hitPosition)
        {
            if (attackingTower != transform)
                return;

            GameObject slashGo = Instantiate(slashPrefab, hitPosition, Quaternion.identity);
            ParticleSystem slashEffect = slashGo.GetComponentInChildren<ParticleSystem>();
            slashEffect.Play();
            Destroy(slashGo, 2f);
        }

      

    }
}

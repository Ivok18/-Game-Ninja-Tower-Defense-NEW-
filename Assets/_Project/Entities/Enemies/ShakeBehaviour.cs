using UnityEngine;

namespace TD.Entities.Enemies
{
    
    public class ShakeBehaviour : MonoBehaviour
    {
        [SerializeField] private float shakeDuration;
        [SerializeField] private float timeUntilEndOfShake;
        [SerializeField] private float shakeIntensity;
        [SerializeField] private float alphaWhenShake;
        public bool IsShaking;
        private Color shakeColor;
        private Vector2 startPos;
        private Color startColor;

        private void OnEnable()
        {
            EnemyHitDetection.OnEnemyHit += Shake;
        }

        private void OnDisable()
        {
            EnemyHitDetection.OnEnemyHit -= Shake;
        }

        private void Start()
        {
            startColor = GetComponent<SpriteGetter>().SpriteRenderer.color;
            shakeColor = new Color(startColor.r, startColor.g, startColor.b, alphaWhenShake);
        }

        void Update()
        {
            if (!IsShaking) 
                return;

            if(timeUntilEndOfShake > 0)
            {
                timeUntilEndOfShake -= Time.deltaTime;

                int shakeClamp = Random.Range(-1, 1);
                float xShake = startPos.x + (shakeClamp * shakeIntensity);
                float yShake = startPos.y + (shakeClamp * shakeIntensity);
                transform.position = new Vector2(xShake, yShake);

            }
            else
            {
                transform.position = startPos;
                SpriteRenderer spriteRenderer = GetComponent<SpriteGetter>().SpriteRenderer;
                spriteRenderer.color = startColor; 
                IsShaking = false;
            }
        }

        public void Shake(Transform enemyAttacked, Transform attackingTower, Vector3 hitPosition)
        {
            bool isTargetOfTower = transform == enemyAttacked;
            if (!isTargetOfTower)
                return;

            if (IsShaking)
                return;
            
            StartShake();
            
            
        }
        public void StartShake()
        {
            IsShaking = true;
            startPos = transform.position;
            SpriteRenderer spriteRenderer = GetComponent<SpriteGetter>().SpriteRenderer;
            spriteRenderer.color = shakeColor;
            timeUntilEndOfShake = shakeDuration;
        }
    }
}

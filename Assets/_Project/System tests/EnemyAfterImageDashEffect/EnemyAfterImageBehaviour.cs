using UnityEngine;

namespace TD.Entities.Enemies.AfterImageDashEffect
{
    public class EnemyAfterImageBehaviour : MonoBehaviour
    {
        private float activeTime = 1000f;
        private float timeActivated;
        private float currentAlpha;
        [SerializeField] private float startAlpha;
        [SerializeField] private float alphaDecreaseSpeed;
        [SerializeField] private SpriteRenderer afterImageSpriteRenderer;
        private Color afterImageColor;
        public Transform Holder;

        private void OnEnable()
        {
            bool hasEnemyHolder = Holder != null;

            if (!hasEnemyHolder)
                return;

            SpriteRenderer holderSpriteRenderer = Holder.GetComponent<SpriteGetter>().SpriteRenderer;
            afterImageColor = holderSpriteRenderer.color;
            afterImageSpriteRenderer.sprite = holderSpriteRenderer.sprite;
            currentAlpha = startAlpha;
            transform.position = Holder.position;
            transform.rotation = Holder.rotation;
            timeActivated = Time.time;

        }

        private void Update()
        {
            if(currentAlpha != 0)
            {
                currentAlpha -= Time.deltaTime * alphaDecreaseSpeed;
            }
            
            bool hasEnemyHolder = Holder != null;
            if (!hasEnemyHolder)
            {
                EnemyAfterImagePool.Instance.AddToPool(gameObject);
                return;
            }


            SpriteRenderer holderSpriteRenderer = Holder.GetComponent<SpriteGetter>().SpriteRenderer;
            afterImageColor = holderSpriteRenderer.color;


            afterImageColor = new Color(afterImageColor.r, afterImageColor.g, afterImageColor.b, currentAlpha);
            afterImageSpriteRenderer.color = afterImageColor;

            bool hasReachedEndOfItsLifetime = Time.time >= (timeActivated + activeTime);
            if (hasReachedEndOfItsLifetime)
            {
                EnemyAfterImagePool.Instance.AddToPool(gameObject);
                return;
            }
        }
    }
}


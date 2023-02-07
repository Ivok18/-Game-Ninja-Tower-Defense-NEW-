using UnityEngine;

namespace TD.Entities.Towers.AfterImageDashEffect
{
    public class AfterImageBehaviour : MonoBehaviour
    {
        private float activeTime = 0.1f;
        private float timeActivated;
        private float currentAlpha;
        [SerializeField] private float startAlpha;
        [SerializeField] private float alphaDecreaseSpeed;
        [SerializeField] private SpriteRenderer afterImageSpriteRenderer;
        private Color afterImageColor;
        public Transform Holder;

        private void OnEnable()
        {
            if(Holder!=null)
            {

                SpriteRenderer holderSpriteRenderer = Holder.GetComponent<SpriteGetter>().SpriteRenderer;
                afterImageColor = holderSpriteRenderer.color;
                afterImageSpriteRenderer.sprite = holderSpriteRenderer.sprite;
                currentAlpha = startAlpha;
                transform.position = Holder.position;
                transform.rotation = Holder.rotation;
                timeActivated = Time.time;
            }
           
        }

        private void Update()
        {
            currentAlpha -= Time.deltaTime * alphaDecreaseSpeed;
            if(Holder!=null)
            {
                SpriteRenderer holderSpriteRenderer = Holder.GetComponent<SpriteGetter>().SpriteRenderer;
                afterImageColor = holderSpriteRenderer.color;
            }
           
            afterImageColor = new Color(afterImageColor.r, afterImageColor.g, afterImageColor.b, currentAlpha);
            afterImageSpriteRenderer.color = afterImageColor;


            if (Time.time >= (timeActivated+activeTime))
            {
                //Add back to pool
                AfterImagePool.Instance.AddToPool(gameObject);
            }
        }



      

            
    }
}


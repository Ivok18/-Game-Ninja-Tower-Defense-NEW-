using UnityEngine;


namespace TD.Entities.Enemies.AfterImageDashEffect
{
    public class EnemyAfterImageDashEffect : MonoBehaviour
    {
        private DodgeBehaviour dodgeBehaviour;
        private Vector3 lastImagePos;
        [SerializeField] private float distanceBetweenImages = 0.2f;


        private void Awake()
        {
            dodgeBehaviour = GetComponent<DodgeBehaviour>();
        }

        private void Update()
        {

            bool enemyIsDodging = dodgeBehaviour.CanStartDodge;
            if (!enemyIsDodging)
                return;

            bool isFarFromLastAfterImage = Vector2.Distance(transform.position, lastImagePos) > distanceBetweenImages;
            if (!isFarFromLastAfterImage)
                return;

            GameObject afterImage = EnemyAfterImagePool.Instance.GetFromPool();
            afterImage.GetComponent<EnemyAfterImageBehaviour>().Holder = transform;
            lastImagePos = transform.position;
            afterImage.SetActive(true);

        }

    }
}


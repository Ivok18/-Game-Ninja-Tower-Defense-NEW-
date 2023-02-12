using UnityEngine;


namespace TD.Entities.Towers.AfterImageDashEffect
{
    public class AfterImageDashEffect : MonoBehaviour
    {
        private TowerStateSwitcher towerStateSwitcher;
        private Vector3 lastImagePos;
        [SerializeField] private float distanceBetweenImages = 0.2f;
 

        private void Awake()
        {
            towerStateSwitcher = GetComponent<TowerStateSwitcher>();
        }

        private void Update()
        {

            bool towerIsAttaking = towerStateSwitcher.CurrentTowerState == TowerState.Attacking;
            if (!towerIsAttaking)
                return;

            bool isFarFromLastAfterImage = Vector2.Distance(transform.position, lastImagePos) > distanceBetweenImages;
            if (!isFarFromLastAfterImage)
                return;

            GameObject afterImage = AfterImagePool.Instance.GetFromPool();
            afterImage.GetComponent<AfterImageBehaviour>().Holder = transform;
            lastImagePos = transform.position;
            afterImage.SetActive(true);
           
        }

    }
}


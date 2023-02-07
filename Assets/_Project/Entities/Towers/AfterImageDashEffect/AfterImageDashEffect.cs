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
           if (towerStateSwitcher.CurrentTowerState != TowerState.Attacking) return;

            if (Vector2.Distance(transform.position, lastImagePos) > distanceBetweenImages)
            {
                GameObject afterImage = AfterImagePool.Instance.GetFromPool();
                afterImage.GetComponent<AfterImageBehaviour>().Holder = transform;
                lastImagePos = transform.position;
                afterImage.SetActive(true);
            }
           
        }

    }
}


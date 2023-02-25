using System.Collections.Generic;
using UnityEngine;

namespace TD.Entities.Enemies.AfterImageDashEffect
{
    public class EnemyAfterImagePool : MonoBehaviour
    {
        [SerializeField] private GameObject[] afterImagePrefabs;

        [SerializeField] private Queue<GameObject> availableObjects = new Queue<GameObject>();

        public static EnemyAfterImagePool Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            GrowPool();
        }

        private void GrowPool()
        {
            foreach (GameObject prefab in afterImagePrefabs)
            {
                var newAfterImage = Instantiate(prefab);
                newAfterImage.transform.SetParent(transform);
                AddToPool(newAfterImage);
            }
        }

        public void AddToPool(GameObject newAfterImage)
        {
            newAfterImage.SetActive(false);
            availableObjects.Enqueue(newAfterImage);
        }

        public GameObject GetFromPool()
        {
            if (availableObjects.Count == 0)
            {
                GrowPool();
            }


            var afterImage = availableObjects.Dequeue();
            return afterImage;
        }
    }

}

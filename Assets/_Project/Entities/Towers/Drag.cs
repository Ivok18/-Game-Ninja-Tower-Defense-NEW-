using UnityEngine;

namespace TD.Entities.Towers
{
    public class Drag : MonoBehaviour
    {
        [SerializeField] private bool isTowerDragged;
        private TowerStateSwitcher towerStateSwitcher;

        private void Awake()
        {
            towerStateSwitcher = GetComponent<TowerStateSwitcher>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!isTowerDragged && towerStateSwitcher.CurrentTowerState !=TowerState.Undeployed) return;

            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.Translate(mousePosition);
        }
    }

}

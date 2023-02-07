using System.Collections.Generic;
using TD.Entities.Towers.States;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TD.Entities.Towers
{
    public class TowerPlacer : MonoBehaviour
    {
        private TowerStateSwitcher towerStateSwitcher;
        private StationaryState stationaryState;
        private SpriteRenderer radiusVizualizer;
        [SerializeField] private bool canPlace;
        [SerializeField] private Color cannotPlaceColor;
        [SerializeField] private Color canPlaceColor;
        [SerializeField] private List<GameObject> collisions;

        public delegate void TowerPlacedCallback(Transform tower);
        public static event TowerPlacedCallback OnTowerPlaced;

        private void Awake()
        {
            towerStateSwitcher = GetComponent<TowerStateSwitcher>();
            stationaryState = GetComponent<StationaryState>();
            radiusVizualizer = GetComponent<RadiusGetter>().RadiusTransform.GetComponent<RadiusDetectionBehaviour>().RadiusVizualizer;
        }

        private void Update()
        {
            if (towerStateSwitcher.CurrentTowerState != TowerState.Undeployed) return;

            
            if(CanPlace()) radiusVizualizer.color = canPlaceColor;
            else radiusVizualizer.color = cannotPlaceColor;   
        }

        private void PlaceTower()
        {          
            if (towerStateSwitcher.CurrentTowerState == TowerState.Undeployed)
            {
                stationaryState.StartPosition = transform.position;
                OnTowerPlaced?.Invoke(transform);
                canPlace = false;
            }
                  
        }


        private bool CanPlace()
        {
            if(collisions.Count > 1)
            {
                canPlace = false;
                return false;
            }
            else
            {
                if(collisions.Count == 1 && collisions[0].transform.CompareTag("PlacingZone"))
                {
                    canPlace = true;
                    return true;
                }
                else
                {
                    canPlace = false;
                    return false;
                }
            }

        }

        private void OnMouseDown()
        {
            //Do not place tower if it is over UI
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (canPlace) PlaceTower();
            }
                                  
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (towerStateSwitcher.CurrentTowerState != TowerState.Undeployed) return;

            TowerStateSwitcher collisionState = collision.GetComponent<TowerStateSwitcher>();

            if (!collision.CompareTag("AttackPattern") && (!collision.CompareTag("Tower")))
            {
                collisions.Add(collision.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (towerStateSwitcher.CurrentTowerState != TowerState.Undeployed) return;
           
            TowerStateSwitcher collisionState = collision.GetComponent<TowerStateSwitcher>();

            if (!collision.CompareTag("AttackPattern") && (!collision.CompareTag("Tower")))
            {
                collisions.Remove(collision.gameObject);
            }
        }

    }

   
}


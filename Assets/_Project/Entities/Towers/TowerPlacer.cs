using System.Collections.Generic;
using TD.Entities.Enemies;
using TD.Entities.Towers.States;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TD.Entities.Towers
{
    public class TowerPlacer : MonoBehaviour
    {
        private TowerStateSwitcher towerStateSwitcher;
        //private StationaryState stationaryState;
        private SpriteRenderer radiusVizualizer;
        [SerializeField] private Transform towerPrefab;
        [SerializeField] private bool canPlace;
        [SerializeField] private Color cannotPlaceColor;
        [SerializeField] private Color canPlaceColor;
        [SerializeField] private List<GameObject> collisions;

        public delegate void TowerPlacedCallback(Transform tower);
        public static event TowerPlacedCallback OnTowerPlaced;

        private void Awake()
        {
            towerStateSwitcher = GetComponent<TowerStateSwitcher>();
            //stationaryState = GetComponent<StationaryState>();
            radiusVizualizer = GetComponent<RadiusGetter>().RadiusTransform.GetComponent<RadiusDetectionBehaviour>().RadiusVizualizer;
        }

        private void Update()
        {
            if (towerStateSwitcher.CurrentTowerState != TowerState.Undeployed) 
                return;
   
            if(CanPlace()) 
                radiusVizualizer.color = canPlaceColor;
            else 
                radiusVizualizer.color = cannotPlaceColor;   
        }

        private void PlaceTower()
        {
            if (towerStateSwitcher.CurrentTowerState != TowerState.Undeployed)
                return;


            //Instantiate the tower at placing position
            Vector3 startPos = transform.position;
            startPos.z = 0;
            GameObject realTower = Instantiate(towerPrefab.gameObject, startPos, Quaternion.identity);
            
            //Set start position at placing position
            StationaryState stationaryState = realTower.GetComponent<StationaryState>();
            stationaryState.StartPosition = startPos;

            //Set z pos of selection area to -5 (it seems to fix the bug)
            SelectionAreaGetter getter = realTower.GetComponent<SelectionAreaGetter>();
            Vector3 selectionAreaPos = startPos;
            selectionAreaPos.z = -5;
            getter.SelectionAreaTransform.position = selectionAreaPos;
            
            
            OnTowerPlaced?.Invoke(realTower.transform);
            canPlace = false;

            Destroy(gameObject);
          
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
            /*if (EventSystem.current.IsPointerOverGameObject())
            {
   
                return;
            }*/
                

            if (canPlace)
                PlaceTower();

        }

        /*
            SPAGETTHI CODE, I MYSELF DONT KNOW HOW IT ACTUALLY WORKS MDRR
        */
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (towerStateSwitcher.CurrentTowerState != TowerState.Undeployed) 
                return;

            WindedBehaviour collWindedBehaviour = collision.GetComponent<WindedBehaviour>();
            bool isCollisionAnEnemy = collWindedBehaviour != null;
            if (isCollisionAnEnemy)
            {
                bool isEnemyAffectedByWind = collWindedBehaviour.ValueContainer[0];
                if(isEnemyAffectedByWind)
                {
                    if (!collision.CompareTag("AttackPattern") && (!collision.CompareTag("Tower"))
                        && (!collision.CompareTag("Enemy")))
                    {
                        collisions.Add(collision.gameObject);
                    }
                }
            }
            else if(!collision.CompareTag("AttackPattern") && (!collision.CompareTag("Tower")))
            {
                collisions.Add(collision.gameObject);
            }
        }


        private void OnTriggerExit2D(Collider2D collision)
        {
            if (towerStateSwitcher.CurrentTowerState != TowerState.Undeployed) 
                return;

            WindedBehaviour collWindedBehaviour = collision.GetComponent<WindedBehaviour>();
            bool isCollisionAnEnemy = collWindedBehaviour != null;
            if (isCollisionAnEnemy)
            {
                bool isEnemyAffectedByWind = collWindedBehaviour.ValueContainer[0];
                if (isEnemyAffectedByWind)
                {
                    if (!collision.CompareTag("AttackPattern") && (!collision.CompareTag("Tower"))
                        && (!collision.CompareTag("Enemy")))
                    {
                        collisions.Remove(collision.gameObject);
                    }
                }
            }
            else if (!collision.CompareTag("AttackPattern") && (!collision.CompareTag("Tower")))
            {
                collisions.Remove(collision.gameObject);
            }
        }

    }

   
}


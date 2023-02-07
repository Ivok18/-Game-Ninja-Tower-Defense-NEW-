using UnityEngine;
using TD.WaypointSystem;
using TD.Entities.Towers;
using TD.ElementSystem;

namespace TD.Entities.Enemies
{ 
    public class EnemyMovement : MonoBehaviour
    {
        [HideInInspector]
        [SerializeField] private WaypointStorer waypointStorer;
        private int nextWaypointIndex = 0;
        public float Speed;
        [SerializeField] private float currentSpeed;
        private float windedSpeed = 6f;
        public bool IsWinded;
        public bool CanMove;
        

        private void OnEnable()
        {
            DamageReceiver.OnEnemyHit += CheckWindElementEffect;
        }
        private void OnDisable()
        {
            DamageReceiver.OnEnemyHit -= CheckWindElementEffect;
        }

        private void Start()
        {
            currentSpeed = Speed;
        }

        public void CheckWindElementEffect(Transform enemy, Transform attackingTower)
        {
            if(enemy == transform && enemy!=null)
            {
                
                ElementsTracker elementsTracker = attackingTower.GetComponent<ElementsTracker>();
                if (elementsTracker != null)
                {
                    bool findWind = false;

                    //checks if attacker got wind element
                    foreach (TowerElement element in elementsTracker.CurrTowerElements)
                    {
                        if (!findWind)
                        {
                            if (element == TowerElement.Wind)
                            {
                                findWind = true;
                   
                            }
                        }
                    }

                    //if attacker got wind element, enemy goes goes back to the start of the road
                    if (findWind)
                    {
                        nextWaypointIndex = 0;
                        currentSpeed = windedSpeed;
                        IsWinded = true;
                    }
                }
            }
        }

        void FixedUpdate()
        {
            //before the wave starts, all enemies are loaded first
            //to prevent any of them to move during the loading process, i use the boolean value "CanMove"
            if(CanMove)
            {
                MoveToNextWaypoint();
            }
            
        }

        private void MoveToNextWaypoint()
        {
            if (nextWaypointIndex < waypointStorer.Waypoints.Length)
            {
                transform.position = Vector2.MoveTowards(transform.position, waypointStorer.Waypoints[nextWaypointIndex].position,
                Time.fixedDeltaTime * currentSpeed);

                if (Vector2.Distance(transform.position, waypointStorer.Waypoints[nextWaypointIndex].position) < 0.1f)
                {
                    if(IsWinded && nextWaypointIndex == 0)
                    {
                        IsWinded = false;
                        currentSpeed = Speed;

                    }
                    nextWaypointIndex++;
                }
            }
        }
    }

}

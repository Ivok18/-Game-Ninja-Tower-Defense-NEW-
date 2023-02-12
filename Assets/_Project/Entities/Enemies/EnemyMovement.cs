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
            EnemyHitDetection.OnEnemyHit += CheckWindElementEffect;
        }
        private void OnDisable()
        {
            EnemyHitDetection.OnEnemyHit -= CheckWindElementEffect;
        }

        private void Start()
        {
            currentSpeed = Speed;
        }

        public void CheckWindElementEffect(Transform enemy, Transform attackingTower)
        {
            bool isTargetOfTower = enemy == transform;
            bool isExisting = enemy != null;

            if (!isExisting)
                return;

            if(!isTargetOfTower)
                return;

            ElementsTracker elementsTracker = attackingTower.GetComponent<ElementsTracker>();
            bool hasElementParams = elementsTracker !=null;
            if (!hasElementParams)
                return;


            //checks if attacker got wind element
            bool findWind = false;
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
            bool hasReachedLastWaypoint = nextWaypointIndex >= waypointStorer.Waypoints.Length;
            if (hasReachedLastWaypoint)
                return;

            transform.position = Vector2.MoveTowards(transform.position, waypointStorer.Waypoints[nextWaypointIndex].position,
                Time.fixedDeltaTime * currentSpeed);

            bool hasReachedNextWaypoint = Vector2.Distance(transform.position, waypointStorer.Waypoints[nextWaypointIndex].position) < 0.1f;


            if (hasReachedNextWaypoint)
            {
                bool hasBeenAffectedByWind = IsWinded;
                bool hasReachedFirstWaypoint = nextWaypointIndex == 0;
                if (hasBeenAffectedByWind && hasReachedFirstWaypoint)
                {
                    IsWinded = false;
                    currentSpeed = Speed;
                }
                nextWaypointIndex++;
            }
        }
    }

}

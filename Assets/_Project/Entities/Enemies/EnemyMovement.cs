using UnityEngine;
using TD.WaypointSystem;
using TD.Entities.Towers;
using TD.ElementSystem;

namespace TD.Entities.Enemies
{
    public class EnemyMovement : MonoBehaviour
    {

        public WaypointStorer WaypointStorer;
        private WindedBehaviour windedBehaviour;
        public int NextWaypointIndex;
        public float Speed;
        public float CurrentSpeed;
        public bool CanMove;


        private void OnEnable()
        {
            EnemyHitDetection.OnEnemyHit += CheckWindElementEffect;
        }
        private void OnDisable()
        {
            EnemyHitDetection.OnEnemyHit -= CheckWindElementEffect;
        }

        private void Awake()
        {
            windedBehaviour = GetComponent<WindedBehaviour>();
        }

        private void Start()
        {
            CurrentSpeed = Speed;
        }

        public void CheckWindElementEffect(Transform enemy, Transform attackingTower, Vector3 hitPosition)
        {
            bool isTargetOfTower = enemy == transform;
            bool isExisting = enemy != null;

            if (!isExisting)
                return;

            if (!isTargetOfTower)
                return;

            ElementsTracker elementsTracker = attackingTower.GetComponent<ElementsTracker>();
            bool hasElementParams = elementsTracker != null;
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
            /*if (findWind)
            {
                NextWaypointIndex = 0;
                CurrentSpeed = windedSpeed;
                IsWinded = true;
            }*/


        }

        void FixedUpdate()
        {
            //before the wave starts, all enemies are loaded first
            //to prevent any of them to move during the loading process, i use the boolean value "CanMove"
            if (CanMove)
            {
                MoveToNextWaypoint();
            }
        }

        private void MoveToNextWaypoint()
        {
            if (!windedBehaviour.IsWinded[0])
            {
                bool hasReachedLastWaypoint = NextWaypointIndex >= WaypointStorer.Waypoints.Length;
                if (hasReachedLastWaypoint)
                    return;

                //Debug.Log("In enemy movement -> " + transform.position);

                transform.position = Vector2.MoveTowards(transform.position, WaypointStorer.Waypoints[NextWaypointIndex].position,
                    Time.fixedDeltaTime * CurrentSpeed);

                bool hasReachedNextWaypoint = Vector2.Distance(transform.position, WaypointStorer.Waypoints[NextWaypointIndex].position) < 0.1f;

                if (hasReachedNextWaypoint)
                {
                    NextWaypointIndex++;
                }
            }



            /*if (hasReachedNextWaypoint)
            {
                bool hasBeenAffectedByWind = IsWinded;
                bool hasReachedFirstWaypoint = NextWaypointIndex == 0;
                if (hasBeenAffectedByWind && hasReachedFirstWaypoint)
                {
                    IsWinded = false;
                    CurrentSpeed = Speed;
                }
                NextWaypointIndex++;
            }*/

        }
    }
}

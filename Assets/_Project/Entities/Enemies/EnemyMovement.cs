using UnityEngine;
using TD.WaypointSystem;

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
        public Vector2 CurrentDirection;
        public bool IsMovingHorizontally;
        public bool IsMovingVertically;


        public WaypointData NextWaypoint => WaypointStorer.Waypoints[NextWaypointIndex];

        public WaypointData PrevioustWaypoint => WaypointStorer.Waypoints[NextWaypointIndex-1];

        public WaypointData[] Waypoints => WaypointStorer.Waypoints;

      

        private void Awake()
        {
            windedBehaviour = GetComponent<WindedBehaviour>();
        }

        private void Start()
        {
            CurrentSpeed = Speed;
        }


        void FixedUpdate()
        {
            //before the wave starts, all enemies are loaded first
            //to prevent any of them to move during the loading process, i use the boolean value "CanMove"
            if (!CanMove)
                return;

            MoveTowardsNextWaypoint();
        }

        private void MoveTowardsNextWaypoint()
        {
            if (windedBehaviour.IsWinded())
                return;
   
            bool hasReachedLastWaypoint = NextWaypointIndex >= Waypoints.Length;
            if (hasReachedLastWaypoint)
                return;

            ExecuteMovement();

            bool hasReachedNextWaypoint = Vector2.Distance(transform.position, NextWaypoint.transform.position) < 0.1f;

            if (!hasReachedNextWaypoint)
                return;

            
            NextWaypointIndex++;
            CurrentDirection = PrevioustWaypoint.nextDirection;
            bool isGoingLeft = CurrentDirection.x < 0;
            bool isGoingRight = CurrentDirection.x > 0;
            bool isGoingUp = CurrentDirection.y > 0;
            bool isGoingDown = CurrentDirection.y < 0;

            if (isGoingLeft || isGoingRight)
            {
                IsMovingHorizontally = true;
                IsMovingVertically = false;
            }

            if(isGoingUp || isGoingDown)
            {
                IsMovingHorizontally = false;
                IsMovingVertically = true;
            }

            if(IsMovingHorizontally)
            {
                IsMovingVertically = false;
            }
            
            else if(IsMovingVertically)
            {
                IsMovingHorizontally = false;
            }
        }


        private void ExecuteMovement()
        {
            transform.position = Vector2.MoveTowards(transform.position, NextWaypoint.transform.position,
               Time.fixedDeltaTime * CurrentSpeed);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)CurrentDirection);
        }
    }
}

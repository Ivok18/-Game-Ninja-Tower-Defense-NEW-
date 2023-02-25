using System.Collections;
using System.Collections.Generic;
using TD.Map;
using TD.WaypointSystem;
using UnityEngine;

namespace TD.Entities.Enemies
{
    public class DodgeBehaviour : MonoBehaviour
    {

        public float DodgeDistance;
        public float DodgeDistanceRemaining;
        public float DodgeSpeed;
        public bool CanDodge;
        public bool CanStartDodge;
        public int NoOfAdditionalDodge;
        public int NoOfAdditionalDodgeRemaining;
        private EnemyMovement enemyMovement;
        private Vector2 exactPositionAfterDodge;


        private void Awake()
        {
            enemyMovement = GetComponent<EnemyMovement>();
        }

        private void OnEnable()
        {
            EnemyAssaultDetectionBehaviour.OnEnemyDetectAssault += TryDodge;
        }

       
        private void OnDisable()
        {
            EnemyAssaultDetectionBehaviour.OnEnemyDetectAssault -= TryDodge;
        }

        private void Start()
        {
            DodgeDistanceRemaining = DodgeDistance;
            NoOfAdditionalDodgeRemaining = NoOfAdditionalDodge;
        }

        private void Update()
        {
            if (!CanStartDodge)
                return;

            DodgeAttack();
        }
        public void DodgeAttack()
        {
            #region dodge1
            /*EnemyMovement enemyMovement = GetComponent<EnemyMovement>();
            WaypointStorer waypointStorer = enemyMovement.WaypointStorer;
            Transform[] waypoints = waypointStorer.Waypoints;
            int nextWaypointIndex = enemyMovement.NextWaypointIndex;


            transform.position = waypoints[nextWaypointIndex].position;
            CanStartDodge = false;
            */
            // transform.position = currPos;
            //
            #endregion

            #region dodge2

            if (DodgeDistanceRemaining > 0)
            {
                DodgeDistanceRemaining -= DodgeSpeed * Time.deltaTime;
            }
            else
            {
                transform.position = exactPositionAfterDodge;
                enemyMovement.CurrentSpeed = enemyMovement.Speed;
                enemyMovement.NextWaypointIndex = NodeManager.Instance.GetNodeAtPosition(transform.position).nextWaypointIndex;
                DodgeDistanceRemaining = DodgeDistance;
                CanStartDodge = false;
                NoOfAdditionalDodgeRemaining = NoOfAdditionalDodge;
            }
            #endregion
        }

        public void TryDodge(Transform enemy)
        {
            if (this.transform != enemy)
                return;

            if (!CanDodge)
                return;

            enemyMovement.CurrentSpeed = DodgeSpeed;
            CanStartDodge = true;

            
            //Get current node 
            Node currNode = NodeManager.Instance.GetNodeAtPosition(transform.position);

            //(Debug) 
            //Debug.Log("dodge distance -> " + (int)DodgeDistance);


            //(Debug) 
            //Debug.Log("start node -> " + currNode.center);

            //x > 0 means currentXPosition is to the left of current node
            //x < 0 means currentXPosition is to the right of current node
            //float xDiffFromCenterOfNode = currNode.center.x - transform.position.x;

            //y > 0 means currentYPosition is near the bottom of current node
            //y < 0 means currentYPosition is near the top of current node
            //float yDiffFromCenterOfNode = currNode.center.y - transform.position.y;

            //Get index of destination node
            Node destinationNode = null;
            int indexOfDestinationNode = currNode.nodeIndex + (int)DodgeDistance;
            if(indexOfDestinationNode <= NodeManager.Instance.Nodes.Count - 1)
            {
                destinationNode = NodeManager.Instance.GetNodeAtIndex(indexOfDestinationNode);
            }
            else
            {
                destinationNode = NodeManager.Instance.GetNodeBeforeVillageDoors();
            }


            //Adjust position in destination node using offset -> to know the exact final dodge position
            exactPositionAfterDodge = destinationNode.center;

            

            //(Debug) 
            //Debug.Log("destination node -> " + destinationNode.center);
       

        }
    }
}
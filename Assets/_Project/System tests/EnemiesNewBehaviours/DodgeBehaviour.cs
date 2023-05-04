using TD.Entities.Towers.States;
using TD.NodeSystem;
using UnityEngine;

namespace TD.Entities.Enemies
{
    public class DodgeBehaviour : MonoBehaviour
    {

        public float DodgeDistance;
        public float DodgeDistanceRemaining;
        public float DodgeSpeed;
        public bool CanDodge;
        public bool IsDodging;
        public int NoOfAdditionalDodge;
        public int NoOfAdditionalDodgeRemaining;
        private EnemyMovement enemyMovement;
        private Vector2 exactPositionAfterDodge;
        [SerializeField] private Transform dummyEnemyPrefab;


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
            if (!IsDodging)
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
                
                IsDodging = false;
            }
            #endregion
        }

        public void TryDodge(Transform targetedEnemy, Transform attackingTower)
        {
            if (this.transform != targetedEnemy)
                return;

            if (!CanDodge)
                return;

            if (IsDodging)
                return;

            if (NoOfAdditionalDodgeRemaining <= 0)
                return;

     
            //Make the attacking tower believe it has touched its target
            GameObject dummyEnemyGo = Instantiate(dummyEnemyPrefab.gameObject, transform.position, Quaternion.identity);
            DummyDetector dummyDetector = dummyEnemyGo.GetComponent<DummyDetector>();
            dummyDetector.Origin = transform;
            LockTargetState lockTargetState = attackingTower.GetComponent<LockTargetState>();
            lockTargetState.Target = dummyEnemyGo.transform;
            NoOfAdditionalDodgeRemaining--;
            enemyMovement.CurrentSpeed = DodgeSpeed;
            DodgeDistanceRemaining = DodgeDistance;
            IsDodging = true;

            
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
                destinationNode = NodeManager.Instance.GetNodeJustBeforeLastNode();
            }


            //Adjust position in destination node using offset -> to know the exact final dodge position
            exactPositionAfterDodge = destinationNode.center;

            

            //(Debug) 
            //Debug.Log("destination node -> " + destinationNode.center);
       

        }
    }
}
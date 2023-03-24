using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TD.Map;
using TD.WaypointSystem;
using UnityEngine;

namespace TD.Entities.Enemies
{
    public class WindedBehaviour : MonoBehaviour
    {
        public bool[] ValueContainer;
        private bool hasCalculatedDestinationNode;
        private EnemyMovement enemyMovement;
        private float windedSpeed = 6f;
        [SerializeField] private Node destinationNode;
        [SerializeField] private int attackerPushPower;
        [SerializeField] private int resistance;
       

        private void Awake()
        {
            enemyMovement = GetComponent<EnemyMovement>();
        }

        private void Start()
        {
            ValueContainer = new bool[1];
            destinationNode = new Node();
        }

        private void Update()
        {
            if (!IsWinded())
                return;


            if (!hasCalculatedDestinationNode)
            {
                if (resistance > 0)
                {
                    if(attackerPushPower > resistance)
                    {
                        int diff = attackerPushPower - resistance;
                        Node currentNode = NodeManager.Instance.GetNodeAtPosition(transform.position);
                        int destionationNodeIndex = currentNode.nodeIndex - diff;
                        
                        if(destionationNodeIndex <= 0)
                        {
                            destinationNode = NodeManager.Instance.GetNodeAtIndex(0);
                        }
                        else
                        {
                            destinationNode = NodeManager.Instance.GetNodeAtIndex(currentNode.nodeIndex - diff);
                        }
                       

                    }
                    else
                    {
                        ValueContainer[0] = false;
                        ShakeBehaviour shakeBehaviour = GetComponent<ShakeBehaviour>();
                        shakeBehaviour.StartShake();
                        return;
                    }
                }
                else
                {
                    destinationNode = NodeManager.Instance.GetNodeAtIndex(0);
                }

                hasCalculatedDestinationNode = true;
               
            }
        }

        private void FixedUpdate()
        {
            if (!IsWinded())
                return;

            if (!hasCalculatedDestinationNode)
                return;

            MoveToDestinationNode();
        }


        public bool IsWinded()
        {
            return ValueContainer[0] == true ? true : false;
        }

        private void MoveToDestinationNode()
        {
            transform.position = Vector2.MoveTowards(transform.position, destinationNode.center,
                   Time.fixedDeltaTime * windedSpeed);


            bool hasReachedDestinationNode = Vector2.Distance(transform.position, destinationNode.center) < 0.1f;
            if (hasReachedDestinationNode)
            {
                transform.position = destinationNode.center;
                enemyMovement.NextWaypointIndex = destinationNode.nextWaypointIndex;
                enemyMovement.CurrentSpeed = enemyMovement.Speed;
                ValueContainer[0] = false;
                hasCalculatedDestinationNode = false;
            }
        }
    }
}

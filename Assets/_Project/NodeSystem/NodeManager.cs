using System;
using System.Collections.Generic;
using UnityEngine;


namespace TD.NodeSystem
{
    [Serializable]
    public class Node
    {
        public Vector2 center;
        public int nodeIndex;

        //index of the waypoint after this node
        public int nextWaypointIndex;

        public Node(Vector2 _center, int _index)
        {
            center = _center;
            nodeIndex = _index;
        }

        public Node()
        {
            center = Vector2.zero;
            nodeIndex = 0;
        }
    }

    public class NodeManager : MonoBehaviour
    {
        public List<Node> Nodes;
        public static NodeManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Setup();
        }

        public Node GetNodeAtPosition(Vector2 position)
        {
            Node closestNode = null;
            float closestDistance = Mathf.Infinity;
            foreach(Node node in Nodes)
            {
                float distanceBetweenParamPosAndCurrNode = Vector2.Distance(position, node.center);
                if (distanceBetweenParamPosAndCurrNode < closestDistance)
                {
                    closestNode = node;
                    closestDistance = distanceBetweenParamPosAndCurrNode;
                }
            }

            return closestNode;
        }

        public Node GetNodeAtIndex(int index)
        {
            foreach (Node node in Nodes)
            {
                if (node.nodeIndex != index)
                    continue;
                
                return node;              
            }
            return null;
        }

        public Node GetNodeJustBeforeLastNode()
        {
            int indexOfNodeBeforeLastNode = Nodes.Count - 2;
            return Nodes[indexOfNodeBeforeLastNode];
        }

        public void Setup()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Vector2 center = transform.GetChild(i).position;
                int index = i;
                Nodes[i].center = center;
                Nodes[i].nodeIndex = index;
            }
        }
    }
}

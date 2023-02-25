using System;
using System.Collections;
using System.Collections.Generic;
using TD.WaypointSystem;
using UnityEngine;
using UnityEngine.UIElements;


namespace TD.Map
{
    [Serializable]
    public class Node
    {
        public Vector2 center;
        public int nodeIndex;
        public int nextWaypointIndex;

        public Node(Vector2 _center, int _index)
        {
            center = _center;
            nodeIndex = _index;
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
            for(int i = 0; i < transform.childCount; i++)
            {
                Vector2 center = transform.GetChild(i).position;
                int index = i;
                Nodes[i].center = center;
                Nodes[i].nodeIndex = index;
            }
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
                if(node.nodeIndex == index)
                {
                    return node;
                }
            }

            return null;
        }

        public Node GetNodeBeforeVillageDoors()
        {
            return Nodes[Nodes.Count - 2];
        }

       
    }
}

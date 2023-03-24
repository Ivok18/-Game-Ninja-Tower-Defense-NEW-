using System;
using UnityEngine;

namespace TD.WaypointSystem
{

    [Serializable]
    public class WaypointData
    {
        public Transform transform;
        public Vector2 nextDirection;
    }

    public class WaypointStorer : MonoBehaviour
    {
        [SerializeField] private WaypointData[] waypoints;

        public WaypointData[] Waypoints => waypoints;
    }

}

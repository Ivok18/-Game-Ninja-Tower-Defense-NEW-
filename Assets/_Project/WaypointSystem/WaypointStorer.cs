using UnityEngine;

namespace TD.WaypointSystem
{
    public class WaypointStorer : MonoBehaviour
    {
        [SerializeField] private Transform[] waypoints;

        public Transform[] Waypoints => waypoints;
    }

}

using System.Collections;
using System.Collections.Generic;
using TD.ElementSystem;
using UI;
using UnityEngine;


namespace TD.Entities.Towers
{
    public class CatalystBehaviour : MonoBehaviour
    {
        [Header("Animation (Mark Rotation)")]
        [SerializeField] private Transform markTransform;
        [SerializeField] private float animSpeed;


        [Header("Radius")]
        [SerializeField] private float radius;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        private void Update()
        {
            //anim
            Vector3 newRot = markTransform.rotation.eulerAngles;
            newRot.z -= animSpeed * Time.deltaTime;
            markTransform.eulerAngles = newRot;
        }


    }
}
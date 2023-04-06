using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TD.Entities.Towers
{
    public class LuckbarBehaviour : MonoBehaviour
    {
        public int IdOfLinkedStatus;
        [SerializeField] private Transform bar;
        [SerializeField] private Transform activationPoint;
        [SerializeField] private Transform activationPointStartMarker;
        [SerializeField] private Transform activationPointEndMarker;
        //public GameObject RollSuccessObj;
        //public GameObject RollFailObj;

        public void Link(int index)
        {
            IdOfLinkedStatus = index;
        }
        public void UpdateBarGauge(float newValue)
        {
            bar.localScale = new Vector3(bar.localScale.x, newValue, bar.localScale.z);
        }
        public void UpdateActivationPointHeight(float percentageOfBarHeightToReach)
        {
            float newHeightToReach = activationPointStartMarker.localPosition.y + (percentageOfBarHeightToReach * GetBarHeight()); 
            activationPoint.localPosition = new Vector3(activationPoint.localPosition.x, newHeightToReach, activationPoint.localPosition.z);
        }
        public float GetBarHeight()
        {
            return Mathf.Abs(activationPointEndMarker.localPosition.y - activationPointStartMarker.localPosition.y);
        }
       
    }
}
using System.Collections;
using System.Collections.Generic;
using TD.StatusSystem;
using UnityEngine;

namespace TD.Entities.Enemies
{
    public class InflictedStatusActivator : MonoBehaviour
    {
        private BurnBehaviour burnBehaviour;
        private StuckBehaviour stuckBehaviour;
        private WindedBehaviour windedBehaviour;
        private Dictionary<StatusType, bool[]> booleansToActivateOnStatusInflicted;

      

        private void Awake()
        {
            burnBehaviour = GetComponent<BurnBehaviour>();
            stuckBehaviour = GetComponent<StuckBehaviour>();
            windedBehaviour = GetComponent<WindedBehaviour>();
        }
        // Start is called before the first frame update
        void Start()
        {
            booleansToActivateOnStatusInflicted = new Dictionary<StatusType, bool[]>();
            booleansToActivateOnStatusInflicted.Add(StatusType.Burned, burnBehaviour.IsBurning);
            booleansToActivateOnStatusInflicted.Add(StatusType.Stuck, stuckBehaviour.IsStuck);
            booleansToActivateOnStatusInflicted.Add(StatusType.Winded, windedBehaviour.IsWinded);
        }

        public void InflictStatus(StatusType status)
        {
            booleansToActivateOnStatusInflicted[status][0] = true;
        }
    }
}
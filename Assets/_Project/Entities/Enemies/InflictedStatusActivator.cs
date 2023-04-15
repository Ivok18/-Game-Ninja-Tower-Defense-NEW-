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
            booleansToActivateOnStatusInflicted.Add(StatusType.Burned, burnBehaviour.ValueContainer);
            booleansToActivateOnStatusInflicted.Add(StatusType.Stuck, stuckBehaviour.ValueContainer);
            booleansToActivateOnStatusInflicted.Add(StatusType.Winded, windedBehaviour.ValueContainer);
        }

        public void InflictStatus(StatusType status)
        {
            booleansToActivateOnStatusInflicted[status][0] = true;
        }
    }
}
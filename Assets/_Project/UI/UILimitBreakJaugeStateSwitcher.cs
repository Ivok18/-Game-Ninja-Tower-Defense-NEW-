using UnityEngine;

namespace TD.UI
{
    public class UILimitBreakJaugeStateSwitcher : MonoBehaviour
    {
        [SerializeField] private GameObject jaugeFillingState;
        [SerializeField] private GameObject jaugeFilledState;


        public GameObject JaugeFilledState => jaugeFilledState;

        private void OnEnable()
        {
            UILimitBreakTrigger.OnLimitBreakButtonTriggered += SwitchToFillingState;
        }

        private void OnDisable()
        {
            UILimitBreakTrigger.OnLimitBreakButtonTriggered -= SwitchToFillingState;
        }

        //Connected to callback above
        private void SwitchToFillingState()
        {
            if (!jaugeFillingState.activeSelf)
            {
                jaugeFilledState.SetActive(false);
                jaugeFillingState.SetActive(true);
            }
        }

        public void SwichToFilledState()
        {
            if(!jaugeFilledState.activeSelf)
            {
                jaugeFilledState.SetActive(true);
                jaugeFillingState.SetActive(false);
            }
            
        }

        

    }
}

using TD.UI;
using UnityEngine;


namespace TD.TowersManager.LimitBreak
{
    public class LimitBreakAvailableSignaler : MonoBehaviour
    {
        public int NoOfKillsForSignal;
        public bool CanLimitBreak;
        [SerializeField] private UILimitBreakJaugeStateSwitcher uILimitBreakJaugeStateSwitcher;

        public static LimitBreakAvailableSignaler Instance;

        private void OnEnable()
        {
            UILimitBreakTrigger.OnLimitBreakButtonTriggered += UpdateSignaler;
        }
        private void OnDisable()
        {
            UILimitBreakTrigger.OnLimitBreakButtonTriggered -= UpdateSignaler;
        }

        private void UpdateSignaler()
        {
            CanLimitBreak = false;
            NoOfKillsForSignal *= 4;
        }

        private void Awake()
        {
            Instance = this;
        }

      
        private void Update()
        {
            bool haveTheTraineesEnoughKillToGetALimitBreak
                = TowersKillsManager.TowersKillsManager.Instance.NoOfTraineeTowerKills < NoOfKillsForSignal;

            if (haveTheTraineesEnoughKillToGetALimitBreak) 
                return;

            CanLimitBreak = true;
            if(CanLimitBreak)
            {
                uILimitBreakJaugeStateSwitcher.SwichToFilledState();
            }
            
        }
    }
}

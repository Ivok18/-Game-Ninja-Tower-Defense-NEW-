using TD.TowersManager.LimitBreak;
using TD.TowersManager.TowersKillsManager;
using UnityEngine;

namespace TD.UI
{
    public class UILimitBreakJaugeFillingBehaviour : MonoBehaviour
    {
        [SerializeField] private RectTransform jauge;

        void Update()
        {
            float currentNoOfKills = TowersKillsManager.Instance.NoOfTraineeTowerKills;
            float noOfKillsForSignal = LimitBreakAvailableSignaler.Instance.NoOfKillsForSignal;
            float ratio = currentNoOfKills / noOfKillsForSignal;
            if(noOfKillsForSignal == 0)
            {
                jauge.localScale = new Vector3(jauge.localScale.x, jauge.localScale.y, jauge.localScale.z);               
            }
            else
            {
                jauge.localScale = new Vector3(jauge.localScale.x, ratio, jauge.localScale.z);
            }
            


        }
    }
}

using UnityEngine;
using TD.Entities.Towers;
using TD.TowersManager.LimitBreak;
using TD.TowersManager.Storer;

namespace TD.UI
{
    public class UILimitBreakTrigger : MonoBehaviour
    {
        public delegate void LimitBreakButtonTriggeredCallback();
        public static event LimitBreakButtonTriggeredCallback OnLimitBreakButtonTriggered;

        //Limit break all trainees towers which haven't limit break (if there are)
        public void LimitBreakTrainees()
        {
            if(LimitBreakAvailableSignaler.Instance.CanLimitBreak)
            {
                if(FindLimitBreakableTower())
                {
                    foreach (Transform tower in TowerStorer.Instance.DeployedTowers)
                    {
                        TowerTypeAccessor towerTypeAccessor = tower.GetComponent<TowerTypeAccessor>();
                        LimitBreakTracker limitBreakTracker = tower.GetComponent<LimitBreakTracker>();
                        if (towerTypeAccessor.TowerType == TowerType.Trainee && !limitBreakTracker.HasBrokeLimits)
                        {
                            LimitBreakActioner limitBreakActioner = tower.GetComponent<LimitBreakActioner>();
                            limitBreakActioner.LimitBreak();
                        }

                    }

                    OnLimitBreakButtonTriggered?.Invoke();
                }   
            }          
        }

        //Check if there is at least one tower that can be limit break
        public bool FindLimitBreakableTower()
        {
            foreach(Transform tower in TowerStorer.Instance.DeployedTowers)
            {
                TowerTypeAccessor towerTypeAccessor = tower.GetComponent<TowerTypeAccessor>();
                if (towerTypeAccessor.TowerType == TowerType.Trainee)
                {
                    LimitBreakTracker limitBreakTracker = tower.GetComponent<LimitBreakTracker>();
                    if(!limitBreakTracker.HasBrokeLimits)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}

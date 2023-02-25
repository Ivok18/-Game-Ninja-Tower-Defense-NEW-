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
            bool isLimitBreakBarFull = LimitBreakAvailableSignaler.Instance.CanLimitBreak == true;
            if (!isLimitBreakBarFull)
                return;

            if (!FindLimitBreakableTower())
                return;

            foreach (Transform tower in TowerStorer.Instance.DeployedTowers)
            {
                TowerTypeAccessor towerTypeAccessor = tower.GetComponent<TowerTypeAccessor>();
                LimitBreakTracker limitBreakTracker = tower.GetComponent<LimitBreakTracker>();
                DodgeShadowCounterBehaviour towerAutoDestructionAfterHit = tower.GetComponent<DodgeShadowCounterBehaviour>();
                bool isTowerATrainee = towerTypeAccessor.TowerType == TowerType.Trainee;
                if (isTowerATrainee && !limitBreakTracker.HasBrokeLimits &&
                    !towerAutoDestructionAfterHit.IsActive)
                {
                    LimitBreakActioner limitBreakActioner = tower.GetComponent<LimitBreakActioner>();
                    limitBreakActioner.LimitBreak();
                }

            }

            OnLimitBreakButtonTriggered?.Invoke();
        }

        //Check if there is at least one tower that can be limit break
        public bool FindLimitBreakableTower()
        {
            foreach (Transform tower in TowerStorer.Instance.DeployedTowers)
            {
                TowerTypeAccessor towerTypeAccessor = tower.GetComponent<TowerTypeAccessor>();
                bool isTowerATrainee = towerTypeAccessor.TowerType == TowerType.Trainee;
                if (!isTowerATrainee)
                    continue;

                LimitBreakTracker limitBreakTracker = tower.GetComponent<LimitBreakTracker>();
                bool hasTowerAlreadyBrokeItsLimit = limitBreakTracker.HasBrokeLimits == true;
                if (hasTowerAlreadyBrokeItsLimit)
                    continue;

                return true;
            }

            return false;
        }
    }
}

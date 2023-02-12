using TD.Entities.Enemies;
using TD.Entities.Towers;
using TD.UI;
using UnityEngine;

namespace TD.TowersManager.TowersKillsManager
{
    public class TowersKillsManager : MonoBehaviour
    {
        public static TowersKillsManager Instance;
        public int NoOfKills;
        public int NoOfTraineeTowerKills;

        private void OnEnable()
        {
            HealthBehaviour.OnEnemyDead += UpdateTotalKillCount;
            UILimitBreakTrigger.OnLimitBreakButtonTriggered += ResetNoOfTraineeTowersKills;
        }

        private void OnDisable()
        {
            HealthBehaviour.OnEnemyDead -= UpdateTotalKillCount;
            UILimitBreakTrigger.OnLimitBreakButtonTriggered -= ResetNoOfTraineeTowersKills;
        }

        private void UpdateTotalKillCount(Transform enemy, Transform attacker, float reward)
        {
            TowerTypeAccessor towerTypeAccessor = attacker.GetComponent<TowerTypeAccessor>();
            if(towerTypeAccessor.TowerType == TowerType.Trainee) NoOfTraineeTowerKills++;

            NoOfKills++;
        }

        public void Awake()
        {
            Instance = this;
        }

        private void ResetNoOfTraineeTowersKills()
        {
            NoOfTraineeTowerKills = 0;
        }



    }
}

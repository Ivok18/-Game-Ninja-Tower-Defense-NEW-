using UnityEngine;

namespace TD.Entities.Towers
{
    public class TowerStateSwitcher : MonoBehaviour
    {
        public TowerState CurrentTowerState;

        public delegate void EnterStateCallback(Transform tower, TowerState state);
        public static event EnterStateCallback OnTowerEnterState;

        private void OnEnable()
        {
            TowerPlacer.OnTowerPlaced += Deploy;
        }
        private void OnDisable()
        {
            TowerPlacer.OnTowerPlaced -= Deploy;
        }

        private void Deploy(Transform tower)
        {
            bool isItTheTowerToDeployOnMap = transform == tower;
            if (!isItTheTowerToDeployOnMap)
                return;

            CurrentTowerState = TowerState.Stationary;
        }

        public void SwitchTo(TowerState newState)
        {      
            CurrentTowerState = newState;
            bool doesTransformStillExist = transform != null;
            if (!doesTransformStillExist)
                return;

            OnTowerEnterState?.Invoke(transform, newState);
        }
    }
}


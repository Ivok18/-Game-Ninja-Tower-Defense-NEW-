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
            if(transform == tower)
            {
                OnTowerEnterState?.Invoke(transform, TowerState.Stationary);
            }
        }

        public void SwitchTo(TowerState newState)
        {      
            CurrentTowerState = newState;
            if (transform != null) OnTowerEnterState?.Invoke(transform, newState);
        }
    }
}


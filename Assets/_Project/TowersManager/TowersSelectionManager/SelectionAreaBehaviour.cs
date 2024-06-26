using System.Collections;
using System.Collections.Generic;
using TD.ElementSystem;
using TD.Entities.Towers;
using UI;
using UnityEngine;

namespace TD.TowersManager.TowerSelectionManager
{
    public class SelectionAreaBehaviour : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer radiusVizualizer;
        [SerializeField] private TowerStateSwitcher TowerStateSwitcher;
        public Transform TowerHolder;
        public bool IsSelected;

        public delegate void TowerSelectedCallback(SelectionAreaBehaviour selection);
        public static event TowerSelectedCallback OnTowerSelected;


        private void OnEnable()
        {
            UICatalystButtonBehaviour.OnCatalyseButtonPressed += DisableSelectionArea;
        }

        private void OnDisable()
        {
            UICatalystButtonBehaviour.OnCatalyseButtonPressed -= DisableSelectionArea;
        }

        private void DisableSelectionArea(Transform targetTower, TowerElement elementOfCatalyst)
        {
            if (targetTower != TowerHolder)
                return;

            gameObject.SetActive(false);
        }

        private void Update()
        {
            bool isTowerInUndeployedState = TowerStateSwitcher.CurrentTowerState == TowerState.Undeployed;
            if (isTowerInUndeployedState) 
                return;

            if (!IsSelected)
            {
                radiusVizualizer.enabled = false;
            }
            else
            {
                radiusVizualizer.enabled = true;
            }        
        }

        private void OnMouseDown()
        {
            bool isTowerInUndeployedState = TowerStateSwitcher.CurrentTowerState == TowerState.Undeployed;
            if (isTowerInUndeployedState) 
                return;

            SelectionAreaBehaviour selectionAreaBehaviour = this;
            OnTowerSelected?.Invoke(selectionAreaBehaviour);
        }

        

    }
}

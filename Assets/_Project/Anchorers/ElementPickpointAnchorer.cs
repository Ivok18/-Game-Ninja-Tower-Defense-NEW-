using System.Collections;
using System.Collections.Generic;
using TD.Entities.Towers;
using UnityEngine;


namespace TD.Anchorers
{
    public class ElementPickpointAnchorer : MonoBehaviour
    {
        private void OnEnable()
        {
            TowerPlacer.OnTowerPlaced += AnchorElementPickPoint;
        }

        private void OnDisable()
        {
            TowerPlacer.OnTowerPlaced -= AnchorElementPickPoint;
        }

        private void AnchorElementPickPoint(Transform towerHoldingPickpoint)
        {
            TowerType towerType = towerHoldingPickpoint.GetComponent<TowerTypeAccessor>().TowerType;

            bool isTowerATrainee = towerType == TowerType.Trainee;
            if (isTowerATrainee)
                return;

            Transform pickpointToAnchor = towerHoldingPickpoint.GetComponent<ElementPickpointGetter>().ElementPickpointTransform;
            pickpointToAnchor.parent = transform;
            
            
        }
    }
}

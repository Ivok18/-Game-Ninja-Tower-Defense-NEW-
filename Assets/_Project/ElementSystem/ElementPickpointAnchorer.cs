using System.Collections;
using System.Collections.Generic;
using TD.Entities.Towers;
using UnityEngine;


namespace TD.ElementSystem
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

            if(towerType!=TowerType.Trainee)
            {
                Transform pickpointToAnchor = towerHoldingPickpoint.GetComponent<ElementPickpointGetter>().ElementPickpointTransform;
                pickpointToAnchor.parent = transform;
            }
            
        }
    }
}

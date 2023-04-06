using System.Collections;
using System.Collections.Generic;
using TD.Entities.Towers;
using UnityEngine;

namespace TD.Anchorers
{
    public class TowerSelectionAreaAnchorer : MonoBehaviour
    {   
        private void OnEnable()
        {
            TowerPlacer.OnTowerPlaced += AnchorSelectionArea;
        }

        private void OnDisable()
        {
            TowerPlacer.OnTowerPlaced -= AnchorSelectionArea;
        }

        private void AnchorSelectionArea(Transform tower)
        {
            Transform selectionAreaToAnchor = tower.GetComponent<SelectionAreaGetter>().SelectionAreaTransform;
            selectionAreaToAnchor.parent = transform;

            CircleCollider2D areaCollider = selectionAreaToAnchor.GetComponent<CircleCollider2D>();
            areaCollider.enabled = true;
          
        }

    }
}
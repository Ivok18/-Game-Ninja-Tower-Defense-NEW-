using System.Collections;
using System.Collections.Generic;
using TD.Entities.Towers;
using UnityEngine;


namespace TD.Anchorers
{
    public class LuckbarsAnchorer : MonoBehaviour
    {
        private void OnEnable()
        {
            TowerPlacer.OnTowerPlaced += AnchorLuckbars;
        }
        private void OnDisable()
        {
            TowerPlacer.OnTowerPlaced -= AnchorLuckbars;
        }

        private void AnchorLuckbars(Transform tower)
        {
            Transform luckbarsToAnchor = tower.GetComponent<LuckbarsGetter>().LuckbarsContainerTransform;
            luckbarsToAnchor.parent = transform;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using TD.Entities.Towers;
using UnityEngine;

namespace TD.TowersManager
{
    public class CatalystSelectorAnchorer : MonoBehaviour
    {
        private void OnEnable()
        {
            TowerPlacer.OnTowerPlaced += AnchorCatalyst;
        }
        private void OnDisable()
        {
            TowerPlacer.OnTowerPlaced -= AnchorCatalyst;
        }

        private void AnchorCatalyst(Transform tower)
        {
            CatalystSelectorGetter catalystSelectorGetter = tower.GetComponent<CatalystSelectorGetter>();
            if (catalystSelectorGetter == null)
                return;


            Transform catalystToAnchor = catalystSelectorGetter.CatalystSelectorTransform;
            catalystToAnchor.parent = transform;
        }
    }
}
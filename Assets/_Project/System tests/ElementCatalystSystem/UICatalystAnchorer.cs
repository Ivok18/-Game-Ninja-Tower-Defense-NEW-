using System.Collections;
using System.Collections.Generic;
using TD.Entities.Towers;
using UnityEngine;

namespace TD.TowersManager
{
    public class UICatalystAnchorer : MonoBehaviour
    {
        private void OnEnable()
        {
            TowerPlacer.OnTowerPlaced += AnchorUICatalyst;
        }
        private void OnDisable()
        {
            TowerPlacer.OnTowerPlaced -= AnchorUICatalyst;
        }

        private void AnchorUICatalyst(Transform tower)
        {
            Transform catalystToAnchor = tower.GetComponent<UICatalystGetter>().UICatalystTransform;
            catalystToAnchor.SetParent(transform);
        }
    }
}
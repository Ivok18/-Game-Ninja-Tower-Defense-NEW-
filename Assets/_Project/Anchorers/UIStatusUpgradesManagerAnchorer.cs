using System.Collections;
using System.Collections.Generic;
using TD.Entities.Towers;
using UnityEngine;

namespace TD.Anchorers
{
    public class UIStatusUpgradesManagerAnchorer : MonoBehaviour
    {
        private void OnEnable()
        {
            TowerPlacer.OnTowerPlaced += AnchorUIStatusUpgradesManager;
        }
        private void OnDisable()
        {
            TowerPlacer.OnTowerPlaced -= AnchorUIStatusUpgradesManager;
        }

        private void AnchorUIStatusUpgradesManager(Transform tower)
        {
            Transform uiStatusUpgradeManagerToAnchor = tower.GetComponent<UIStatusUpgradesManagerGetter>().UIStatusUpgradesManagerTransform;
            uiStatusUpgradeManagerToAnchor.SetParent(transform);
        }
    }
}

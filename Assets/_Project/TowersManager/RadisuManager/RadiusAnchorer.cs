using TD.Entities.Towers;
using UnityEngine;

namespace TD.TowersManager.RadiusManager
{
    public class RadiusAnchorer : MonoBehaviour
    { 
        private void OnEnable()
        {
            TowerPlacer.OnTowerPlaced += AnchorRadius;       
        }
        private void OnDisable()
        {
            TowerPlacer.OnTowerPlaced -= AnchorRadius;
        }

        private void AnchorRadius(Transform tower)
        {
            Transform radiusToAnchor = tower.GetComponent<RadiusGetter>().RadiusTransform;
            radiusToAnchor.parent = transform;
        }
    }
}
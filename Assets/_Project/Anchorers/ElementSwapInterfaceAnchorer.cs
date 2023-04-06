using System.Collections;
using System.Collections.Generic;
using TD.Entities.Towers;
using UnityEngine;


namespace TD.Anchorers
{
    public class ElementSwapInterfaceAnchorer : MonoBehaviour
    {
        private void OnEnable()
        {
            TowerPlacer.OnTowerPlaced += AnchorElementSwapInterface;
        }

        private void OnDisable()
        {
            TowerPlacer.OnTowerPlaced -= AnchorElementSwapInterface;
        }

        private void AnchorElementSwapInterface(Transform towerHoldingElementSwapInterface)
        {
            ElementSwapInterfaceGetter getter = towerHoldingElementSwapInterface.GetComponent<ElementSwapInterfaceGetter>();
            bool doesGetterExist = getter != null;
            if (!doesGetterExist)
                return;

            Transform elementSwapInterfaceToAnchor = getter.SwapInterfaceTransform;
            elementSwapInterfaceToAnchor.SetParent(transform);
        }



    }
}



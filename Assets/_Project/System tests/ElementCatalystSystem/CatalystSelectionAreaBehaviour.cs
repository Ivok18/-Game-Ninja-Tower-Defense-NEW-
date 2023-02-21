using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TD.Entities.Towers
{
    public class CatalystSelectionAreaBehaviour : MonoBehaviour
    {
        private CircleCollider2D selectionArea;

        public delegate void CatalystSelectedCallback(Transform targetCatalyst);
        public static event CatalystSelectedCallback OnCatalystSelected;

        private void Awake()
        {
            selectionArea = GetComponent<CircleCollider2D>();
        }

        private void OnEnable()
        {
            selectionArea.enabled = true;
        }

        private void OnMouseDown()
        {
            CatalystSelector catalystSelector = GetComponentInParent<CatalystSelector>();
            OnCatalystSelected?.Invoke(catalystSelector.TowerHolder);
        }
    }
}
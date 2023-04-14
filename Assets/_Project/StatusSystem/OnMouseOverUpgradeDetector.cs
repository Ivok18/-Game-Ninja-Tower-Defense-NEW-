using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

namespace TD.StatusSystem
{
    public class OnMouseOverUpgradeDetector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [SerializeField] private int upgradeSlotIndex;
        [SerializeField] private List<OnMouseOverAnyUpgradeDetector> onMouseOverAnyUpgradeDetectorLinks;
        private Transform towerHolder;

        public delegate void MouseOverUpgradeCallback(Transform towerHolder, int targetedSlotIndex, List<OnMouseOverAnyUpgradeDetector> onMouseOverAnyUpgradeDetectorLinks);
        public static event MouseOverUpgradeCallback OnMouseOverUpgrade;

        public delegate void MouseDownUpgradeCallback(Transform towerHolder, int targetedSlotIndex);
        public static event MouseDownUpgradeCallback OnMouseDownUpgrade;


        public void OnPointerEnter(PointerEventData pointerEventData)
        {         
            OnMouseOverUpgrade?.Invoke(towerHolder,upgradeSlotIndex, onMouseOverAnyUpgradeDetectorLinks);
            foreach(var onMouseUpgradeDetectorLink in onMouseOverAnyUpgradeDetectorLinks)
            {
                onMouseUpgradeDetectorLink.IsTriggered = true;
            }
        }

        public void OnPointerExit(PointerEventData pointerEventData)
        {
            foreach (var onMouseUpgradeDetectorLink in onMouseOverAnyUpgradeDetectorLinks)
            {
                onMouseUpgradeDetectorLink.IsTriggered = false;
            }
        }

        public void OnPointerDown(PointerEventData pointerEventData)
        {
            OnMouseDownUpgrade?.Invoke(towerHolder, upgradeSlotIndex);
        }

        public void SetTowerHolder(Transform _towerHolder)
        {
            towerHolder = _towerHolder;
        }


    }
}

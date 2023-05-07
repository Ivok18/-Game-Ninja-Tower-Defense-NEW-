using UnityEngine.EventSystems;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace TD.StatusSystem
{
    public class OnMouseOverAnyUpgradeDetector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        public bool IsTriggered;
        private Transform towerHolder;

        public delegate void MouseDownAnyUpgradeCallback(Transform towerHolder);
        public static event MouseDownAnyUpgradeCallback OnMouseDownAnyUpgrade;

        private void OnEnable()
        {
            OnMouseOverUpgradeDetector.OnMouseOverUpgrade += UpdateTriggerOnMouseOverUpgrade;
        }

        private void OnDisable()
        {
            OnMouseOverUpgradeDetector.OnMouseOverUpgrade -= UpdateTriggerOnMouseOverUpgrade;
        }

        public void UpdateTriggerOnMouseOverUpgrade(Transform towerHolder, int targetedSlotIndex, List<OnMouseOverAnyUpgradeDetector> onMouseOverAnyUpgradeDetectorLinks)
        {

            foreach(var onMouseOverAnyUpgradeDetectorLink in onMouseOverAnyUpgradeDetectorLinks)
            {
                if(onMouseOverAnyUpgradeDetectorLink == this)
                {
                    IsTriggered = true;
                    return;
                }
            }

            IsTriggered = false;
        }

        public void OnPointerEnter(PointerEventData pointerEventData)
        {
            IsTriggered = true;
        }

        public void OnPointerExit(PointerEventData pointerEventData)
        {
            IsTriggered = false;
        }

        public void OnPointerDown(PointerEventData pointerEventData)
        {
            OnMouseDownAnyUpgrade(towerHolder);
        }

        public void SetTowerHolder(Transform _towerHolder)
        {
            towerHolder = _towerHolder;
        }

    }
}
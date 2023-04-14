using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TD.Map
{
    public class ClickOnMap : MonoBehaviour
    {
        public delegate void OnMapClickCallback();
        public static event OnMapClickCallback OnMapClick;

        private void OnMouseDown()
        {
            if (IsPointerOverUIObject())
                return;
            
            OnMapClick?.Invoke();
        }

      
        private bool IsPointerOverUIObject()
        {
            // the ray cast appears to require only eventData.position.
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 1;
        }
    }
}


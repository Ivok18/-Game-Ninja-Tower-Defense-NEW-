using UnityEngine;

namespace TD.Map
{
    public class ClickOnMap : MonoBehaviour
    {
        public delegate void OnMapClickCallback();
        public static event OnMapClickCallback OnMapClick;

        private void OnMouseDown()
        {
            OnMapClick?.Invoke();
        }
    }
}


using System.Collections.Generic;
using TD.Map;
using TD.UI;
using UnityEngine;

namespace TD.ElementSystem
{
    //It stores all the UIElementSelector script on each element in the UI located on the right of the screen
    //It is notified when an element button is pressed in the UI located on the right of the screen
    //It manages all the UIElementSelector script on each element in the UI located on the right of the screen
    public class UIElementSelectorManager : MonoBehaviour
    {
        [SerializeField] List<UIElementSelector> selectorList;

        public delegate void UIElementSelectedCallback(TowerElement element);
        public static event UIElementSelectedCallback OnUIElementSelected;

        public delegate void UIElementUnselectedCallback(TowerElement element);
        public static event UIElementUnselectedCallback OnUIElementUnselected;

        private void OnEnable()
        {
            UIElementSelector.OnUIElementClick += UpdateList;
            ClickOnMap.OnMapClick += UnselectAll;
        }
        private void OnDisable()
        {
            UIElementSelector.OnUIElementClick -= UpdateList;
            ClickOnMap.OnMapClick -= UnselectAll;
        }


        private void UpdateList(TowerElement elementOfButtonPressed)
        {
            foreach (UIElementSelector selector in selectorList)
            {
                if (elementOfButtonPressed != selector.Element)
                {
                    selector.UIElementIsSelected = false;
                    continue;
                }
                   
                //Unselect if it was already selected
                //Select if it was not selected
                selector.UIElementIsSelected = !selector.UIElementIsSelected;

                if (selector.UIElementIsSelected == true)
                {
                    //Notify everyone that it was selected
                    OnUIElementSelected?.Invoke(selector.Element);
                }
                else
                {
                    //Notify everyone that it was unselected
                    OnUIElementUnselected?.Invoke(selector.Element);
                }
            }
        }

        private void UnselectAll()
        {
            foreach (var selector in selectorList)
            {
                selector.UIElementIsSelected = false;
            }
        }
    }
}

using System.Collections.Generic;
using TD.Entities.Towers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TD.ElementSystem
{
    //This class we created assuming that element swaps will alwyas be performed on a ..
    //"Talented Tower", but it could be extended in the future
    public class ElementSwapInterfaceBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform towerHolder;
        [SerializeField] private Button[] swapButtons; //elements are assigned manually
        [SerializeField] private Dictionary<TowerElement, Color> elementColors;
        [SerializeField] private Color fire;
        [SerializeField] private Color earth;
        [SerializeField] private Color wind;
        private TowerElement elementToGive;
        private Canvas canvas;
        private List<Transform> targetTowerForSwap;
        private bool swapHasBeenPerformed;

        public delegate void ElementSwapButtonTriggerCallback
            (Transform targetTower, TowerElement elementToBeReplaced, TowerElement elementToGive);

        public static event ElementSwapButtonTriggerCallback OnElementSwapButtonTrigger;

        private void OnEnable()
        {
            ElementDataApplier.OnElementSwapRequest += TryActivateTowerElementSwapInterface;
            SwapButtonBehaviour.OnElementSwapButtonPressed += TriggerElementSwap;
            ElementInQueueManager.OnQueueClear += TryDeactivateElementSwapInterface;
        }

        private void OnDisable()
        {
            ElementDataApplier.OnElementSwapRequest -= TryActivateTowerElementSwapInterface;
            SwapButtonBehaviour.OnElementSwapButtonPressed -= TriggerElementSwap;
            ElementInQueueManager.OnQueueClear -= TryDeactivateElementSwapInterface;
        }

        private void Awake()
        {
            canvas = GetComponent<Canvas>();
        }

        private void Start()
        {
            targetTowerForSwap = new List<Transform>();
            elementColors = new Dictionary<TowerElement, Color>();

            elementColors.Add(TowerElement.Fire, fire);
            elementColors.Add(TowerElement.Earth, earth);
            elementColors.Add(TowerElement.Wind, wind);
        }

        private void Update()
        {
            bool isThereATowerRequestedForElementSwap = targetTowerForSwap.Count > 0;
            if (swapHasBeenPerformed && isThereATowerRequestedForElementSwap)
            {
                targetTowerForSwap.Clear();
                canvas.enabled = false;
                swapHasBeenPerformed = false;
            }
        }

        private void TryActivateTowerElementSwapInterface(Transform targetTower, TowerElement elementToGive)
        {
            //We use this function only if the target tower for swap is the same as the tower this
            //element swap interface is linked to
            bool isItTheTowerRequestedForElementSwap = targetTower == towerHolder;
            if (!isItTheTowerRequestedForElementSwap)
                return;
                    
            canvas.enabled = true;

            //We store the tower that will get an element swap in a list
            targetTowerForSwap.Add(towerHolder);

            //We store the swap element
            this.elementToGive = elementToGive;

            //Using the elements that that the tower already has..
            List<TowerElement> towerCurrElements = targetTower.GetComponent<ElementsTracker>().CurrTowerElements;

            //We create a new tab that will only contain elements =/= from TowerElement.None..
            TowerElement[] swapButtonsElements = new TowerElement[swapButtons.Length];
            for(int i = 0; i < towerCurrElements.Count; i++)
            {
  
                bool isElementSlotAvailable = towerCurrElements[i] == TowerElement.None;
                if (isElementSlotAvailable)
                    continue;

                swapButtonsElements[i] = towerCurrElements[i];
            }

            //Using the new tab..
            //We give a name and a color to each swap button in the list declared in the inspector
            //Name and color depends on the elements the tower already has
            for (int i = 0; i < swapButtons.Length; i++)
            {
                string swapButtonName = swapButtonsElements[i].ToString().ToUpper();
                Color swapButtonColor = elementColors[swapButtonsElements[i]];

                swapButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = swapButtonName;
                swapButtons[i].transform.GetComponent<Image>().color = swapButtonColor;

                SwapButtonBehaviour swapButtonBehaviour = swapButtons[i].GetComponent<SwapButtonBehaviour>();
                swapButtonBehaviour.SetNormalColor(swapButtonColor);
                swapButtonBehaviour.SetNormalName(swapButtonName);
                swapButtonBehaviour.SetHighlightedColor(elementColors[elementToGive]);
                swapButtonBehaviour.SetHighlightedName(elementToGive.ToString().ToUpper());
                swapButtonBehaviour.SetAttachedElement(swapButtonsElements[i]);
            }         
        }
        private void TryDeactivateElementSwapInterface(TowerElement element)
        {
            if(canvas.isActiveAndEnabled)
            {
                canvas.enabled = false;
            }
        }
        private void TriggerElementSwap(TowerElement elementToBeReplaced)
        {
            bool isThereATowerRequestedForElementSwap = targetTowerForSwap.Count > 0;
            if (!isThereATowerRequestedForElementSwap)
                return;

            OnElementSwapButtonTrigger?.Invoke(targetTowerForSwap[0], elementToBeReplaced, elementToGive);
            swapHasBeenPerformed = true;
        }

    }
}
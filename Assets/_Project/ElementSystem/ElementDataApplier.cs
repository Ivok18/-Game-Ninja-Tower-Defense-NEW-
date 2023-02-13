using TD.Entities.Towers;
using TD.MonetarySystem;
using TD.TowersManager.Storer;
using TD.TowersManager.TowerSelectionManager;
using UnityEngine;

namespace TD.ElementSystem
{
    public class ElementDataApplier : MonoBehaviour
    {
        [SerializeField] private ElementScriptableObject dataOfElementToApply;

        public delegate void ElementDataAppliedOnToweCallback(Transform tower, ElementScriptableObject dataOfElementApplied);
        public static event ElementDataAppliedOnToweCallback OnElementDataAppliedOnTower;

        public delegate void ElementDataUnappliedFromTowerCallback(Transform tower, TowerElement element);
        public static event ElementDataUnappliedFromTowerCallback OnElementDataUnappliedFromTower;

        public delegate void ElementSwapRequestCallback(Transform targetTower, TowerElement elementToGive);
        public static event ElementSwapRequestCallback OnElementSwapRequest;


        private void OnEnable()
        {
            ElementInQueueManager.OnElementAddedToQueue += AddElementToApply;
            ElementInQueueManager.OnQueueClear += RemoveElementToApply;
            SelectionAreaBehaviour.OnTowerSelected += TryApplyElement;
            ElementSwapInterfaceBehaviour.OnElementSwapButtonTrigger += Swap;

        }
        private void OnDisable()
        {
            ElementInQueueManager.OnElementAddedToQueue -= AddElementToApply;
            ElementInQueueManager.OnQueueClear -= RemoveElementToApply;
            SelectionAreaBehaviour.OnTowerSelected -= TryApplyElement;
            ElementSwapInterfaceBehaviour.OnElementSwapButtonTrigger -= Swap;
        }

        private void Update()
        {
            //If there is no element in queue to apply we do nothing
            bool isThereAnElementToApply = dataOfElementToApply != null;
            if (!isThereAnElementToApply) 
                return;

            //Checks the list of deployed towers
            foreach (var tower in TowerStorer.Instance.DeployedTowers)
            {
                ElementsTracker elementsTracker = tower.GetComponent<ElementsTracker>();
                bool doesTowerHaveAnElementTracker = elementsTracker != null;
                if (!doesTowerHaveAnElementTracker)
                    continue;

                //If a tower does not have the element in queue..
                bool doesTowerAlreadyHaveTheElementToApply = CheckIfTowerHasElement(tower, dataOfElementToApply.Element);
                if (doesTowerAlreadyHaveTheElementToApply)
                {
                    elementsTracker.IsReadyToGetAnElement = false;
                    continue;
                }

                //And we have enough money, we can give the element in queu to the tower
                bool hasEnoughMoney = MoneyManager.Instance.Money >= dataOfElementToApply.Cost;
                if (hasEnoughMoney)
                {
                    elementsTracker.IsReadyToGetAnElement = true;
                }
                else
                {
                    elementsTracker.IsReadyToGetAnElement = false;
                }
            }
        }
        

        private void AddElementToApply(ElementScriptableObject dataOfElementToApply)
        {
            this.dataOfElementToApply = dataOfElementToApply;   
        }

        private void RemoveElementToApply(TowerElement towerElement)
        {
            dataOfElementToApply = null;
     
            foreach (var tower in TowerStorer.Instance.DeployedTowers)
            {
                ElementsTracker elementsTracker = tower.GetComponent<ElementsTracker>();
                bool doesTowerHaveAnElementTracker = elementsTracker != null;
                if (!doesTowerHaveAnElementTracker)
                    continue;

                elementsTracker.IsReadyToGetAnElement = false;               
            }
        }

        private bool CheckIfTowerHasElement(Transform tower, TowerElement element)
        {
            ElementsTracker elementsTracker = tower.GetComponent<ElementsTracker>();
            bool doesTowerHaveAnElementTracker = elementsTracker != null;
            if (!doesTowerHaveAnElementTracker)
                return true;

            foreach (var towerElement in elementsTracker.CurrTowerElements)
            {
                bool doesTowerAlreadyHaveTheElementInParameter = towerElement == element;
                if (!doesTowerAlreadyHaveTheElementInParameter)
                    continue;

                return true;
            }
           
            return false;
        }

        private void TryApplyElement(SelectionAreaBehaviour selection)
        {
            //We get the tower that we clicked on
            Transform towerSelected = selection.TowerHolder;

            ElementsTracker elementsTracker = towerSelected.GetComponent<ElementsTracker>();
            bool doesTowerHaveAnElementTracker = elementsTracker != null;
            if (!doesTowerHaveAnElementTracker)
                return;

            
            //We check if the bool value that tells us if we can apply the element in queue to the tower is true
            bool canApplyElementOnSelectedTower = elementsTracker.IsReadyToGetAnElement;

            if (!canApplyElementOnSelectedTower)
                return;

            //Basically, the number of slots available indicates how many element left the tower can still get
            //We assume first that it's the max value (max value is the maximum number of slots)
            int noOfSlotAvailable = elementsTracker.CurrTowerElements.Count;

            //For each slot that is =/= from "TowerElement.None", it means the slot is not available
            foreach (var currElement in elementsTracker.CurrTowerElements)
            {
                bool isElementSlotAvailable = currElement == TowerElement.None;
                if (isElementSlotAvailable)
                    continue;

                //And we decrease the number of slots available
                noOfSlotAvailable--;
            }

            //If there is at least a slot available for an element, we assign it the element in quueu
            bool isThereAtLeastASlotAvailable = noOfSlotAvailable > 0;
            if (isThereAtLeastASlotAvailable)
            {
                ApplyElementInQueueOnTower(towerSelected);
            }
            else //The tower we clicked on cannot receive more element
            {
                //Depending of the type of tower, there will be =/= outcomes
                int maxElementCapacity = elementsTracker.CurrTowerElements.Count;
                bool isMaxNumberPossibleForTowerIsOne = maxElementCapacity == 1;
                if (maxElementCapacity == 1) //Max element capacity = 1 means it's a trainee tower
                {
                    //For a common tower, we unapply its current element, and we replace it wth the element in quueue
                    UnapplyElementOnTower(towerSelected, elementsTracker.CurrTowerElements[0]);
                    ApplyElementInQueueOnTower(towerSelected);
                }
                else
                {
                    //This part of code was designed to handle when it is a talented tower
                    //For a talented tower, we resquest an element swap
                    OnElementSwapRequest?.Invoke(towerSelected, dataOfElementToApply.Element);
                }

            }   
        }

        private void ApplyElementInQueueOnTower(Transform towerToApplyElementOn)
        {
            ElementsTracker elementsTracker = towerToApplyElementOn.GetComponent<ElementsTracker>();
            for (int i = 0; i < elementsTracker.CurrTowerElements.Count; i++)
            {
                bool isElementSlotAvailable = elementsTracker.CurrTowerElements[i] == TowerElement.None;

                if (!isElementSlotAvailable)
                    continue;

                elementsTracker.CurrTowerElements[i] = dataOfElementToApply.Element;
                OnElementDataAppliedOnTower?.Invoke(towerToApplyElementOn, dataOfElementToApply);
                return;
            }
        }

        private void UnapplyElementOnTower(Transform towerToUnapplyElementFrom, TowerElement elementToUnapply)
        {
            ElementsTracker elementsTracker = towerToUnapplyElementFrom.GetComponent<ElementsTracker>();
            for (int i = 0; i < elementsTracker.CurrTowerElements.Count; i++)
            {
                bool doesElementSlotContainElementToRemove = elementsTracker.CurrTowerElements[i] == elementToUnapply;
                if (!doesElementSlotContainElementToRemove)
                    continue;

                elementsTracker.CurrTowerElements[i] = TowerElement.None;
                OnElementDataUnappliedFromTower?.Invoke(towerToUnapplyElementFrom, elementToUnapply);
                return;
            }
        }

        private void Swap(Transform targetTower, TowerElement elementToBeReplaced, TowerElement elementToGive)
        {        
            foreach (Transform tower in TowerStorer.Instance.DeployedTowers)
            {
                bool isItTheTowerToPerformSwapElementOn = tower == targetTower;
                if (!isItTheTowerToPerformSwapElementOn)
                    continue;

                UnapplyElementOnTower(tower, elementToBeReplaced);
                ApplyElementInQueueOnTower(tower);
                return;
            }
        }
        
        
    }
}
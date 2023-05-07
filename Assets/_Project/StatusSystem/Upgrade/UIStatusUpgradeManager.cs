using System;
using System.Collections.Generic;
using System.Linq;
using TD.Map;
using TD.MonetarySystem;
using TD.TowersManager.TowerSelectionManager;
using UnityEngine;
using UnityEngine.UI;


//Every tower on the scene has a "UIStatusUpgradeManager" script to it.
//It is attached to the tower subobject  called "Canvas (status upgrade UI)"
//and as its name implies, it manages everything related to the upgrade process of the tower holder
namespace TD.StatusSystem
{

    //A slot is the location in UI reserved in order to upgrade a status
    //For example, when you double click on a tower, the upgrade UI appears
    //And depending on the number of status the tower got, the UI shows the slots
    //A slot can be in different states, described by the following enum
    public enum SlotState
    {
        NOT_AVAILABLE, // -> can't upgrade the status contained in the slot, hence the color of the sprite is grey -ish
        AVAILABLE,  // -> can upgrade the status contained in the slot, hence the original color of the sprite is displayed
        SLOT_MAXED // -> can't upgrade anymore because the status has attained its full potential.
                   // "MAX" is written near the sprite and the sprite got its original color
    }

    [Serializable]
    public class UIStatusUpgradeSlot
    {
        public string name;
        public Transform towerHolder;
        public bool canClickToUpgrade;

        [Header("Parent")]
        public GameObject wholeObj;

        //Each state has its own subobject !

        [Header("Subobject state NOT AVAILABLE parameters")]
        public GameObject notAvailableStateSubObj;
        public Image notAvailableIcon;

        [Header("Subobject state AVAILABLE parameters")]
        public GameObject availableStateSubObj;
        public Image availableIcon;
        public Image outline;

        [Header("Subobject state MAXES parameters")]
        public GameObject slotMaxedStateSubObj;
        public Image maxedIcon;

        [Header("On mouse over upgrade obj")]
        public GameObject onMouseOverUpgrade;

        [Header("Other")]
        public int targetStatusID;
        public SlotState state;
        
        /// <summary>
        /// Returns the current state of the slot
        /// </summary>
        /// <returns></returns>
        public SlotState CheckState()
        {
            SlotState slotState = SlotState.AVAILABLE;

            StatusToInflictTracker statusToInflictTracker = towerHolder.GetComponent<StatusToInflictTracker>();
            if (statusToInflictTracker == null)
                return SlotState.NOT_AVAILABLE;

            foreach(var status in statusToInflictTracker.CurrentStatusToInflict)
            {
                bool hasFoundTargetStatusIdOnTowerHolder = status.id == targetStatusID;
                if (!hasFoundTargetStatusIdOnTowerHolder)
                    continue;

                bool isStatusMaxed = status.currentOddsForActivation >= 0.13f;
                if (isStatusMaxed)
                {
                    slotState = SlotState.SLOT_MAXED;
                    break;
                }
                else
                {
                    bool hasEnoughMoney = MoneyManager.Instance.Money >= 10;
                    if (hasEnoughMoney)
                    {
                        slotState = SlotState.AVAILABLE;
                        break;
                    }
                    else
                    {
                        slotState = SlotState.NOT_AVAILABLE;
                        break;
                    }
                }
            }

            return slotState;
        }

    }

    //This class contains the upgrade animation parameters
    [Serializable]
    public class UpgradeAnimation
    {
        public string name;
        public GameObject obj; //game object used to perform the animation
        public Animator animator; //animator attached to obj above
        public Image image; //image component used to store all the images that make the animation
    }


    public class UIStatusUpgradeManager : MonoBehaviour
    {
        private bool isUiOpen;
        private Canvas uiCanvas;
        private int noOfUpgradeSlotsToSetup;
        [SerializeField] private Transform towerHolder;

        
        //it is used to detect when the mouse is over a specifc upgrade slot (we know which slot)
        [SerializeField] private List<Transform> onMouseOverUpgradeDetectorsContainers;

        //it is used to detect when the mouse is over any upgrade slot (we don't know which slot)
        [SerializeField] private Transform onMouseOverAnyUpgradeDetectorContainer;

        [SerializeField] private List<UIStatusUpgradeSlot> uiStatusUpgradeSlots;
        private List<StatusType> statusTypes;
        [Tooltip("Index of the last upgrade slot which was hovered by the cursor of the mouse")]
        [SerializeField] private int indexOfLastUpgradeSlotHoveredByMouse;

        //I used a dictionary to link a status stype to a specific sprite when the slot is not available
        private Dictionary<StatusType, Sprite> slotNotAvailableIconTable;
        [SerializeField] private List<Sprite> slotNotAvailableIcons;

        //I used a dictionary to link a status stype to a specific sprite when the slot is available
        private Dictionary<StatusType, Sprite> slotAvailableIconTable;
        [SerializeField] private List<Sprite> slotAvailableIcons;

        //I used a dictionary to link a status stype to a specific outline when the slot is available
        private Dictionary<StatusType, Sprite> slotAvailableOutlineTable;
        [SerializeField] private List<Sprite> slotAvailableOutlines;

        [SerializeField] private List<UpgradeAnimation> upgradeAnimations;
        [SerializeField] private Transform upgradeAnimationsObj;

        public delegate void UIRequestStatusUpgradeCallback(Transform targetTower, int targetedStatusID);
        public static event UIRequestStatusUpgradeCallback OnUIRequestStatusUpgradeCallback;

        private void OnEnable()
        {
            DoubleClickOnSelectionAreaDetector.OnDoubleClickOnSelectionArea += TryShowUIStatusUpgrades;
            OnMouseOverUpgradeDetector.OnMouseOverUpgrade += ActivateOrDesactivateSlotButton;
            OnMouseOverUpgradeDetector.OnMouseDownUpgrade += UpgradeStatusLinkedToUpgradeSlotButton;
            OnMouseOverAnyUpgradeDetector.OnMouseDownAnyUpgrade += TryFindUpgradableStatusAndUpgradeIt;
            StatusManager.OnStatusOddsForActivationBoost += TryPlayUpgradeAnimation;
            ClickOnMap.OnMapClick += TryCloseUI;
        }

        private void OnDisable()
        {
            DoubleClickOnSelectionAreaDetector.OnDoubleClickOnSelectionArea -= TryShowUIStatusUpgrades;
            OnMouseOverUpgradeDetector.OnMouseOverUpgrade -= ActivateOrDesactivateSlotButton;
            OnMouseOverUpgradeDetector.OnMouseDownUpgrade -= UpgradeStatusLinkedToUpgradeSlotButton;
            OnMouseOverAnyUpgradeDetector.OnMouseDownAnyUpgrade -= TryFindUpgradableStatusAndUpgradeIt;
            StatusManager.OnStatusOddsForActivationBoost -= TryPlayUpgradeAnimation;
            ClickOnMap.OnMapClick -= TryCloseUI;
        }

        private void Awake()
        {
            uiCanvas = GetComponent<Canvas>();
        }

        private void Start()
        {
            uiCanvas.enabled = false;
            indexOfLastUpgradeSlotHoveredByMouse = -1;

            foreach(var detectorContainer in onMouseOverUpgradeDetectorsContainers)
            {
                for(int i = 0; i < detectorContainer.childCount; i++)
                {
                    OnMouseOverUpgradeDetector onMouseOverUpgradeDetector = 
                       detectorContainer.GetChild(i).GetComponent<OnMouseOverUpgradeDetector>();
                    onMouseOverUpgradeDetector.SetTowerHolder(towerHolder);
                }
            }

            for(int i = 0; i < onMouseOverAnyUpgradeDetectorContainer.childCount; i++)
            {
                OnMouseOverAnyUpgradeDetector onMouseOverAnyUpgradeDetector =
                    onMouseOverAnyUpgradeDetectorContainer.GetChild(i).GetComponent<OnMouseOverAnyUpgradeDetector>();
                onMouseOverAnyUpgradeDetector.SetTowerHolder(towerHolder);
            }

            statusTypes = new List<StatusType>();
            statusTypes.Add(StatusType.Burned);
            statusTypes.Add(StatusType.Stuck);
            statusTypes.Add(StatusType.Winded);

            //Link every status to its specific icon when the slot is not available
            slotNotAvailableIconTable = new Dictionary<StatusType, Sprite>();
            int indexInListToLinkToAStatus = 0;
            for(int i = 0; i < statusTypes.Count; i++)
            {
                int j = 0;

                bool hasReachedEndOfIconList = j >= slotNotAvailableIcons.Count;
                while (!hasReachedEndOfIconList)
                {
                    bool hasFoundIndexOfIconInListToLinkToStatus = j == indexInListToLinkToAStatus == true ? true : false;
                    if (!hasFoundIndexOfIconInListToLinkToStatus)
                    {
                        j++;
                        continue;
                    }
                      
                    slotNotAvailableIconTable.Add(statusTypes[i], slotNotAvailableIcons[indexInListToLinkToAStatus]);
                    indexInListToLinkToAStatus++;
                    break;
                }
            }

            //Link every status to its specific icon when the slot is available
            slotAvailableIconTable = new Dictionary<StatusType, Sprite>();
            indexInListToLinkToAStatus = 0;
            for (int i = 0; i < statusTypes.Count; i++)
            {
                int j = 0;

                bool hasReachedEndOfIconList = j >= slotAvailableIcons.Count;
                while (!hasReachedEndOfIconList)
                {
                    if (j != indexInListToLinkToAStatus)
                    {
                        j++;
                        continue;
                    }

                    slotAvailableIconTable.Add(statusTypes[i], slotAvailableIcons[indexInListToLinkToAStatus]);
                    indexInListToLinkToAStatus++;
                    break;
                }
            }

            //Link every status to its specific outline when the slot is available
            indexInListToLinkToAStatus = 0;
            slotAvailableOutlineTable = new Dictionary<StatusType, Sprite>();
            for (int i = 0; i < statusTypes.Count; i++)
            {
                int j = 0;

                bool hasReachedEndOfOutlineList = j >= slotAvailableOutlines.Count;
                while (!hasReachedEndOfOutlineList)
                {
                    if (j != indexInListToLinkToAStatus)
                    {
                        j++;
                        continue;
                    }

                    slotAvailableOutlineTable.Add(statusTypes[i], slotAvailableOutlines[indexInListToLinkToAStatus]);
                    indexInListToLinkToAStatus++;
                    break;
                }
            }
        }


        private void Update()
        {
            if (!isUiOpen)
                return;

            if (CheckMouseLeftUI())
            {
                foreach (var uiStatusUpgradeSlot in uiStatusUpgradeSlots)
                {
                    uiStatusUpgradeSlot.canClickToUpgrade = false;
                }
            }

            foreach (var uiStatusUpgradeSlot in uiStatusUpgradeSlots)
            {
                bool isSlotActive = uiStatusUpgradeSlot.wholeObj.activeSelf == true ? true : false;
                if (!isSlotActive)
                    continue;

                uiStatusUpgradeSlot.state = uiStatusUpgradeSlot.CheckState();

                StatusToInflictTracker statusToInflictTracker = uiStatusUpgradeSlot.towerHolder.GetComponent<StatusToInflictTracker>();

                foreach (var status in statusToInflictTracker.CurrentStatusToInflict)
                {
                    bool hasFoundIdOfStatusInSlot = status.id == uiStatusUpgradeSlot.targetStatusID;
                    if (!hasFoundIdOfStatusInSlot)
                        continue;

                    bool canClickOnSlotToUpgradeStatus = uiStatusUpgradeSlot.canClickToUpgrade == true ? true : false;
                    if (canClickOnSlotToUpgradeStatus)
                    {
                        uiStatusUpgradeSlot.outline.gameObject.SetActive(true);
                    }
                    else
                    {
                        uiStatusUpgradeSlot.outline.gameObject.SetActive(false);
                    }

                    switch (uiStatusUpgradeSlot.state)
                    {
                        case SlotState.NOT_AVAILABLE:
                            uiStatusUpgradeSlot.notAvailableStateSubObj.SetActive(true);
                            uiStatusUpgradeSlot.availableStateSubObj.SetActive(false);
                            uiStatusUpgradeSlot.slotMaxedStateSubObj.SetActive(false);
                            uiStatusUpgradeSlot.notAvailableIcon.sprite = slotNotAvailableIconTable[status.type];
                            break;

                        case SlotState.AVAILABLE:
                            uiStatusUpgradeSlot.notAvailableStateSubObj.SetActive(false);
                            uiStatusUpgradeSlot.availableStateSubObj.SetActive(true);
                            uiStatusUpgradeSlot.slotMaxedStateSubObj.SetActive(false);
                            uiStatusUpgradeSlot.availableIcon.sprite = slotAvailableIconTable[status.type];
                            uiStatusUpgradeSlot.outline.sprite = slotAvailableOutlineTable[status.type];
                            break;
                        case SlotState.SLOT_MAXED:
                            uiStatusUpgradeSlot.notAvailableStateSubObj.SetActive(false);
                            uiStatusUpgradeSlot.availableStateSubObj.SetActive(false);
                            uiStatusUpgradeSlot.slotMaxedStateSubObj.SetActive(true);
                            uiStatusUpgradeSlot.maxedIcon.sprite = slotAvailableIconTable[status.type];
                            uiStatusUpgradeSlot.outline.sprite = slotAvailableOutlineTable[status.type];
                            break;
                        default:
                            break;
                    }
                }
            }

        }


        public void TryShowUIStatusUpgrades(Transform selectionAreaTowerHolder)
        {
            if (selectionAreaTowerHolder != towerHolder)
            {
                //cLose UI
                foreach (var uiUpgradeSlot in uiStatusUpgradeSlots)
                {
                    uiUpgradeSlot.wholeObj.SetActive(false);
                }
                uiCanvas.enabled = false;
                isUiOpen = false;
                onMouseOverAnyUpgradeDetectorContainer.gameObject.SetActive(false);
                onMouseOverUpgradeDetectorsContainers.First().parent.gameObject.SetActive(false);
                upgradeAnimationsObj.gameObject.SetActive(false);               
                return;
            }
               

            StatusToInflictTracker statusToInflictTracker = selectionAreaTowerHolder.GetComponent<StatusToInflictTracker>();
            if (statusToInflictTracker == null)
                return;

            if (statusToInflictTracker.CurrentStatusToInflict.Count <= 0)
                return;

            if (isUiOpen)
                return;

            //open ui
            uiCanvas.enabled = true;
            isUiOpen = true;
            onMouseOverAnyUpgradeDetectorContainer.gameObject.SetActive(true);
            onMouseOverUpgradeDetectorsContainers.First().parent.gameObject.SetActive(true);
            upgradeAnimationsObj.gameObject.SetActive(true);
            noOfUpgradeSlotsToSetup = statusToInflictTracker.CurrentStatusToInflict.Count;
            List<int> idsUsed = new List<int>();

            if (noOfUpgradeSlotsToSetup <= 0)
                return;

            foreach (var uiStatusUpgradeSlot in uiStatusUpgradeSlots)
            {
                if (noOfUpgradeSlotsToSetup <= 0)
                    break;

                uiStatusUpgradeSlot.wholeObj.SetActive(true);
                uiStatusUpgradeSlot.onMouseOverUpgrade.SetActive(true);
                uiStatusUpgradeSlot.towerHolder = selectionAreaTowerHolder;
                foreach(var status in statusToInflictTracker.CurrentStatusToInflict)
                {

                    bool isStatusAlreadyUsed = (idsUsed.Find(x => x == status.id) != 0) ? true : false;
                    if (isStatusAlreadyUsed)
                        continue;

                    uiStatusUpgradeSlot.targetStatusID = status.id;
                    idsUsed.Add(status.id);
                    break;
                }

                noOfUpgradeSlotsToSetup--;
            }
        }

        /// <summary>
        /// Decides whether the slot buttons are clickable or not when the mouse is hovering this UI
        /// </summary>
        /// <param name="_towerHolder"></param>
        /// <param name="targetedSlotIndex"></param>
        /// <param name="onMouseOverAnyUpgradeDetectorLinks"></param>
        public void ActivateOrDesactivateSlotButton(Transform _towerHolder, int targetedSlotIndex, List<OnMouseOverAnyUpgradeDetector> onMouseOverAnyUpgradeDetectorLinks)
        {
            bool isMouseOverThisTower = _towerHolder == towerHolder;
            if (!isMouseOverThisTower)
                return;
            
            bool isMouseStillOverSlot = indexOfLastUpgradeSlotHoveredByMouse == targetedSlotIndex;
            bool hasMouseJustEnteredThisSlot = indexOfLastUpgradeSlotHoveredByMouse == -1 
                || indexOfLastUpgradeSlotHoveredByMouse != targetedSlotIndex;
            if (isMouseStillOverSlot)
            {
                uiStatusUpgradeSlots[targetedSlotIndex].canClickToUpgrade = true;
                return;
            }
            else if(hasMouseJustEnteredThisSlot)
            {
                indexOfLastUpgradeSlotHoveredByMouse = targetedSlotIndex;
                uiStatusUpgradeSlots[targetedSlotIndex].canClickToUpgrade = true;

                //Do not allow other slots to be clicked on while this slot is clickable
                for (int i = 0; i < uiStatusUpgradeSlots.Count; i++)
                {
                    bool hasFoundTargetedSlotIndex = i == targetedSlotIndex;
                    if (hasFoundTargetedSlotIndex)
                        continue;

                    uiStatusUpgradeSlots[i].canClickToUpgrade = false;
                }
            } 
        }


        /// <summary>
        /// Upgrades the status linked to the slot button when the slot button has been pressed
        /// </summary>
        /// <param name="_towerHolder"></param>
        /// <param name="targetedSlotIndex"></param>
        public void UpgradeStatusLinkedToUpgradeSlotButton(Transform _towerHolder, int targetedSlotIndex)
        {
            bool doesTheSlotButtonPressedBelongToTheUpgradeUIOfThisTower = _towerHolder == towerHolder;
            if (!doesTheSlotButtonPressedBelongToTheUpgradeUIOfThisTower)
                return;


            StatusToInflictTracker statusToInflictTracker = towerHolder.GetComponent<StatusToInflictTracker>();
            bool hasStatusToInflictTracker = statusToInflictTracker != null;
            if (!hasStatusToInflictTracker)
                return;

            bool hasStatusToInflict = statusToInflictTracker.CurrentStatusToInflict.Count > 0;
            if (!hasStatusToInflict)
                return;


            foreach (var status in statusToInflictTracker.CurrentStatusToInflict)
            {
                bool hasFoundIDOfStatusToUpgrade = status.id == uiStatusUpgradeSlots[targetedSlotIndex].targetStatusID;
                if (!hasFoundIDOfStatusToUpgrade)
                    continue;

                OnUIRequestStatusUpgradeCallback?.Invoke(towerHolder, status.id);
                break;
            }
        }

        /// <summary>
        /// Upgrades the status linked to the slot button when the slot button has been pressed
        /// </summary>
        /// <param name="_towerHolder"></param>
        public void TryFindUpgradableStatusAndUpgradeIt(Transform _towerHolder)
        {
            bool doesTheSlotButtonPressedBelongToTheUpgradeUIOfThisTower = _towerHolder == towerHolder;
            if (!doesTheSlotButtonPressedBelongToTheUpgradeUIOfThisTower)
                return;

            StatusToInflictTracker statusToInflictTracker = towerHolder.GetComponent<StatusToInflictTracker>();
            bool hasStatusToInflictTracker = statusToInflictTracker != null;
            if (!hasStatusToInflictTracker)
                return;


            bool hasStatusToInflict = statusToInflictTracker.CurrentStatusToInflict.Count > 0;
            if (!hasStatusToInflict)
                return;

            foreach (var uiStatusUpgradeSlot in uiStatusUpgradeSlots)
            {
                bool hasFoundSlotThatContainsTheStatusToUpgrade = uiStatusUpgradeSlot.canClickToUpgrade == true ? true : false;
                if (!hasFoundSlotThatContainsTheStatusToUpgrade)
                    continue;

                foreach (var status in statusToInflictTracker.CurrentStatusToInflict)
                {
                    bool hasFoundIDOfStatusToUpgrade = status.id == uiStatusUpgradeSlot.targetStatusID;
                    if (!hasFoundIDOfStatusToUpgrade)
                        continue;

                    OnUIRequestStatusUpgradeCallback?.Invoke(towerHolder, status.id);
                    break;
                }
            }
        }

        /// <summary>
        /// Play the upgrade animation on the target UI
        /// </summary>
        /// <param name="targetTower"></param>
        /// <param name="idOfBoostedStatus"></param>
        public void TryPlayUpgradeAnimation(Transform targetTower, int idOfBoostedStatus)
        {
            bool hasBeenAskedForAnAnimation = targetTower == towerHolder;
            if (!hasBeenAskedForAnAnimation)
                return;

            PlayUpgradeAnimation();
        }

        public void TryCloseUI()
        {
            //close UI
            foreach(var uiUpgradeSlot in uiStatusUpgradeSlots)
            {
                uiUpgradeSlot.wholeObj.SetActive(false);
            }
            uiCanvas.enabled = false;
            isUiOpen = false;
            onMouseOverAnyUpgradeDetectorContainer.gameObject.SetActive(false);
            onMouseOverUpgradeDetectorsContainers.First().parent.gameObject.SetActive(false);
            upgradeAnimationsObj.gameObject.SetActive(false);
        }

        /// <summary>
        /// Returns true if the mouse fully leaves the upgrade UI and returns false if it is still inside it
        /// </summary>
        /// <returns></returns>
        public bool CheckMouseLeftUI()
        {
            for(int i = 0; i < onMouseOverAnyUpgradeDetectorContainer.childCount; i++)
            {
                OnMouseOverAnyUpgradeDetector onMouseOverAnyUpgradeDetector = onMouseOverAnyUpgradeDetectorContainer.GetChild(i).GetComponent<OnMouseOverAnyUpgradeDetector>();

                bool isMouseStillInsideUI = onMouseOverAnyUpgradeDetector.IsTriggered == true ? true : false;
                if (isMouseStillInsideUI)
                    return false;
            }

            return true;
        }

        public void PlayUpgradeAnimation()
        {
            foreach(var upgradeAnim in upgradeAnimations)
            {
                upgradeAnim.obj.SetActive(true);
                upgradeAnim.image.enabled = true;
                upgradeAnim.animator.Play(upgradeAnim.name);
            }
        }
    }
    
}
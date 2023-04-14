using Newtonsoft.Json.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TD.Map;
using TD.MonetarySystem;
using TD.TowersManager.TowerSelectionManager;
using UnityEngine;
using UnityEngine.UI;

namespace TD.StatusSystem
{

    public enum SlotState
    {
        NOT_AVAILABLE,
        AVAILABLE,
        SLOT_MAXED
    }

    [Serializable]
    public class UIStatusUpgradeSlot
    {
        public string name;
        public Transform towerHolder;
        public bool canClickToUpgrade;

        [Header("Parent")]
        public GameObject wholeObj;

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
        

       
        public SlotState CheckState()
        {
            SlotState slotState = SlotState.AVAILABLE;

            StatusToInflictTracker statusToInflictTracker = towerHolder.GetComponent<StatusToInflictTracker>();
            foreach(var status in statusToInflictTracker.CurrentStatusToInflict)
            {
                if (status.id != targetStatusID)
                    continue;

                if(status.currentOddsForActivation >= 0.13f)
                {
                    slotState = SlotState.SLOT_MAXED;
                    break;
                }
                else
                {
                    if (MoneyManager.Instance.Money >= 10)
                    {
                        slotState = SlotState.AVAILABLE;
                        break;
                    }
                    else if(MoneyManager.Instance.Money < 10)
                    {
                        slotState = SlotState.NOT_AVAILABLE;
                        break;
                    }
                }
            }

            return slotState;
        }

    }

    [Serializable]
    public class UpgradeAnimation
    {
        public string name;
        public GameObject obj;
        public Animator animator;
        public Image image;
    }


    public class UIStatusUpgradeManager : MonoBehaviour
    {
        private bool isUiOpen;
        private Canvas uiCanvas;
        private int noOfUpgradeSlotsToSetup;
        [SerializeField] private Transform towerHolder;
        [SerializeField] private List<Transform> onMouseOverUpgradeDetectorsContainers;
        [SerializeField] private Transform onMouseOverAnyUpgradeDetectorContainer;
        [SerializeField] private List<UIStatusUpgradeSlot> uiStatusUpgradeSlots;
        private List<StatusType> statusTypes;
        [Tooltip("Index of the last upgrade slot which was hovered by the cursor of the mouse")]
        [SerializeField] private int indexOfLastUpgradeSlotHoveredByMouse;


        private Dictionary<StatusType, Sprite> slotNotAvailableIconTable;
        [SerializeField] private List<Sprite> slotNotAvailableIcons;

        private Dictionary<StatusType, Sprite> slotAvailableIconTable;
        [SerializeField] private List<Sprite> slotAvailableIcons;

        private Dictionary<StatusType, Sprite> slotAvailableOutlineTable;
        [SerializeField] private List<Sprite> slotAvailableOutlines;

        [SerializeField] private List<UpgradeAnimation> upgradeAnimations;
        [SerializeField] private Transform upgradeAnimationsObj;

        public delegate void UIRequestStatusUpgradeCallback(Transform targetTower, int targetedStatusID);
        public static event UIRequestStatusUpgradeCallback OnUIRequestStatusUpgradeCallback;

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

            slotNotAvailableIconTable = new Dictionary<StatusType, Sprite>();
            int indexInListToLinkToAStatus = 0;
            for(int i = 0; i < statusTypes.Count; i++)
            {
                int j = 0;

                while (j < slotNotAvailableIcons.Count)
                {
                    if (j != indexInListToLinkToAStatus)
                    {
                        j++;
                        continue;
                    }
                      
                    slotNotAvailableIconTable.Add(statusTypes[i], slotNotAvailableIcons[indexInListToLinkToAStatus]);
                    indexInListToLinkToAStatus++;
                    break;
                }
            }

            slotAvailableIconTable = new Dictionary<StatusType, Sprite>();
            indexInListToLinkToAStatus = 0;
            for (int i = 0; i < statusTypes.Count; i++)
            {
                int j = 0;

                while (j < slotAvailableIcons.Count)
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

            indexInListToLinkToAStatus = 0;
            slotAvailableOutlineTable = new Dictionary<StatusType, Sprite>();
            for (int i = 0; i < statusTypes.Count; i++)
            {
                int j = 0;

                while (j < slotAvailableOutlines.Count)
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

        public void ActivateOrDesactivateSlotButton(Transform _towerHolder, int targetedSlotIndex, List<OnMouseOverAnyUpgradeDetector> onMouseOverAnyUpgradeDetectorLinks)
        {
            if (_towerHolder != towerHolder)
                return;
            
            if (indexOfLastUpgradeSlotHoveredByMouse == targetedSlotIndex)
            {
                uiStatusUpgradeSlots[targetedSlotIndex].canClickToUpgrade = true;
                return;
            }
            else if(indexOfLastUpgradeSlotHoveredByMouse == -1 || indexOfLastUpgradeSlotHoveredByMouse != targetedSlotIndex)
            {
                indexOfLastUpgradeSlotHoveredByMouse = targetedSlotIndex;
                uiStatusUpgradeSlots[targetedSlotIndex].canClickToUpgrade = true;
                for (int i = 0; i < uiStatusUpgradeSlots.Count; i++)
                {
                    if (i == targetedSlotIndex)
                        continue;

                    uiStatusUpgradeSlots[i].canClickToUpgrade = false;
                    //uiStatusUpgradeSlots[i].Test();
                }
            } 
        }

        public void UpgradeStatusLinkedToUpgradeSlotButton(Transform _towerHolder, int targetedSlotIndex)
        {
            if (_towerHolder != towerHolder)
                return;


            StatusToInflictTracker statusToInflictTracker = towerHolder.GetComponent<StatusToInflictTracker>();
            if (statusToInflictTracker == null)
                return;


            if (statusToInflictTracker.CurrentStatusToInflict.Count <= 0)
                return;


            foreach (var status in statusToInflictTracker.CurrentStatusToInflict)
            {
                if (status.id != uiStatusUpgradeSlots[targetedSlotIndex].targetStatusID)
                    continue;

                OnUIRequestStatusUpgradeCallback?.Invoke(towerHolder, status.id);
                break;
            }
        }

        public void TryFindUpgradableStatusAndUpgradeIt(Transform _towerHolder)
        {
            if (_towerHolder != towerHolder)
                return;

            StatusToInflictTracker statusToInflictTracker = towerHolder.GetComponent<StatusToInflictTracker>();
            if (statusToInflictTracker == null)
                return;


            if (statusToInflictTracker.CurrentStatusToInflict.Count <= 0)
                return;

            foreach (var uiStatusUpgradeSlot in uiStatusUpgradeSlots)
            {
                if (!uiStatusUpgradeSlot.canClickToUpgrade)
                    continue;

                foreach (var status in statusToInflictTracker.CurrentStatusToInflict)
                {
                    if (status.id != uiStatusUpgradeSlot.targetStatusID)
                        continue;

                    OnUIRequestStatusUpgradeCallback?.Invoke(towerHolder, status.id);
                    break;
                }
            }
        }

        public void TryPlayUpgradeAnimation(Transform targetTower, int idOfBoostedStatus)
        {
            if (targetTower != towerHolder)
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

        private void Update()
        {
            if (!isUiOpen)
                return;

            


            if(CheckMouseLeftUI())
            {
                foreach (var uiStatusUpgradeSlot in uiStatusUpgradeSlots)
                {
                    uiStatusUpgradeSlot.canClickToUpgrade = false;
                }
            }
            
            foreach(var uiStatusUpgradeSlot in uiStatusUpgradeSlots)
            {
                if (uiStatusUpgradeSlot.wholeObj.activeSelf == false)
                    continue;

                uiStatusUpgradeSlot.state = uiStatusUpgradeSlot.CheckState();
            
                StatusToInflictTracker statusToInflictTracker = uiStatusUpgradeSlot.towerHolder.GetComponent<StatusToInflictTracker>();

                foreach (var status in statusToInflictTracker.CurrentStatusToInflict)
                {
                    if (status.id != uiStatusUpgradeSlot.targetStatusID)
                        continue;

                    if (uiStatusUpgradeSlot.canClickToUpgrade)
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

        public bool CheckMouseLeftUI()
        {
            for(int i = 0; i < onMouseOverAnyUpgradeDetectorContainer.childCount; i++)
            {
                OnMouseOverAnyUpgradeDetector onMouseAnyUpgradeDetector = onMouseOverAnyUpgradeDetectorContainer.GetChild(i).GetComponent<OnMouseOverAnyUpgradeDetector>();

                if (onMouseAnyUpgradeDetector.IsTriggered)
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
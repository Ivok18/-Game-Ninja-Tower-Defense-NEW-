using TD.Entities.Towers;
using TD.Map;
using TD.TowersManager.Storer;
using UnityEngine;
using TD.ElementSystem;
using TD.TowersManager.Spawner;

namespace TD.TowersManager.TowerSelectionManager
{
    public class TowerSelectionManager : MonoBehaviour
    {
        [SerializeField] private SelectionAreaBehaviour currentSelection;
        private bool someTowerJustGotAnElement;

        public delegate void SelectionSellCallback(Transform selection);
        public static event SelectionSellCallback OnTowerSelectedSell;


        private void OnEnable()
        {
            SelectionAreaBehaviour.OnTowerSelected += UpdateTowerSelected;
            ClickOnMap.OnMapClick += TryUndoCurrentSelection;
            ElementDataApplier.OnElementDataAppliedOnTower += NotifySomeTowerJustGotElement;
            TowerSpawner.OnTowerSpawn += TryUndoCurrentSelection;

        }

        private void OnDisable()
        {
            SelectionAreaBehaviour.OnTowerSelected -= UpdateTowerSelected;
            ClickOnMap.OnMapClick -= TryUndoCurrentSelection;
            ElementDataApplier.OnElementDataAppliedOnTower -= NotifySomeTowerJustGotElement;
            TowerSpawner.OnTowerSpawn -= TryUndoCurrentSelection;
            
        }

        
        
        //Happens when a tower is selected
        private void UpdateTowerSelected(SelectionAreaBehaviour selection)
        {
            if(currentSelection == selection)
            {
                currentSelection = null;
                selection.IsSelected = false;
            }
            else
            {
                currentSelection = selection;

                //Undo selection for all towers except current selection
                foreach (Transform tower in TowerStorer.Instance.DeployedTowers)
                {
                    if (tower != currentSelection.transform)
                    {
                        SelectionAreaGetter selectionAreaGetter = tower.GetComponent<SelectionAreaGetter>();
                        SelectionAreaBehaviour towerSelection = selectionAreaGetter.SelectionAreaTransform.GetComponent<SelectionAreaBehaviour>();
                        towerSelection.IsSelected = false;
                    }
                }
            }
           
        }

        //Happens when a left click is performed on the map 
        private void TryUndoCurrentSelection()
        {
            //Undo current selection
            if(currentSelection!=null) currentSelection.IsSelected = false;
            currentSelection = null;
        }

        //Happens when a tower gets an element 
        private void NotifySomeTowerJustGotElement(Transform tower, TowerElement element, int elementCost)
        {
            someTowerJustGotAnElement = true;
        }

       

        //Happens a tower is spawned from shop
        private void TryUndoCurrentSelection(Transform tower, int cost)
        {
            if(currentSelection!=null)
            {
                currentSelection.IsSelected = false;
                currentSelection = null;
            }
            
        }


        private void Update()
        {
            bool hasATowerBeenSelected = currentSelection != null;
            if (!hasATowerBeenSelected)
                return;

            currentSelection.IsSelected = true;


            //Deselect all tower whenever a tower gets an element
            //I didnt want to see the radius of the tower after it got an element
            //But in the end I accidently deselected not only the tower that got an element, but all the other ones
            //I decided to leave it like this (maybe this will change later)
            if(someTowerJustGotAnElement)
            {
                currentSelection.IsSelected = false;
                currentSelection = null;
                someTowerJustGotAnElement = false;
            }

            //Right click to sell selection
            bool hasPlayerPressedRightMouseButton = Input.GetMouseButtonDown(1);
            if (hasPlayerPressedRightMouseButton)
            {
                SellSelection();
            }
        }

        private void SellSelection()
        {
            OnTowerSelectedSell?.Invoke(currentSelection.TowerHolder);
            currentSelection = null;
        }
    }

}

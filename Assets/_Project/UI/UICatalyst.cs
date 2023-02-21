using System.Collections;
using System.Collections.Generic;
using TD.Entities.Towers;
using TD.TowersManager.TowerSelectionManager;
using UnityEngine;
using TD.ElementSystem;
using UnityEngine.UI;
using TD.Map;
using UnityEditor;

namespace UI
{
    public class UICatalyst : MonoBehaviour
    {
        [SerializeField] private Transform towerHolder;
        [SerializeField] private SelectionAreaBehaviour towerSelectionArea;
        [SerializeField] private CatalystSelectionAreaBehaviour catalystSelectionArea;
        [SerializeField] private ElementsTracker elementsTracker;
        

        [SerializeField] private UICatalystButtonBehaviour catalystButtonBehaviour;
        private Dictionary<TowerElement, Color> elementColors2;
        private Dictionary<TowerElement, Color> elementColors;

        [Header("Catalyse Button")]       
        [SerializeField] private Color fire;
        [SerializeField] private Color earth;
        [SerializeField] private Color wind;

        [Header("Fuse button")]
        [SerializeField] private Color fire2;
        [SerializeField] private Color earth2;
        [SerializeField] private Color wind2;

        private Canvas canvas;
   


        private void OnEnable()
        {
            ClickOnMap.OnMapClick += DeactivateAllUICatalyst;
            SelectionAreaBehaviour.OnTowerSelected += ShowTargetCatalyseButtonOnly;
            CatalystSelectionAreaBehaviour.OnCatalystSelected += ShowTargetFuseButtonOnly;
            UICatalystButtonBehaviour.OnCatalyseButtonPressed += DeactivateAllUICatalyst;
         

        }

        private void OnDisable()
        {
            ClickOnMap.OnMapClick -= DeactivateAllUICatalyst;
            SelectionAreaBehaviour.OnTowerSelected -= ShowTargetCatalyseButtonOnly;
            CatalystSelectionAreaBehaviour.OnCatalystSelected -= ShowTargetFuseButtonOnly;
            UICatalystButtonBehaviour.OnCatalyseButtonPressed -= DeactivateAllUICatalyst;
   

        }

        private void Awake()
        {
            canvas = GetComponent<Canvas>();
        }

        private void Start()
        {
            //Catalyse Button
            elementColors = new Dictionary<TowerElement, Color>();

            elementColors.Add(TowerElement.Fire, fire);
            elementColors.Add(TowerElement.Earth, earth);
            elementColors.Add(TowerElement.Wind, wind);

            //Fuse Button
            elementColors2 = new Dictionary<TowerElement, Color>();

            elementColors2.Add(TowerElement.Fire, fire2);
            elementColors2.Add(TowerElement.Earth, earth2);
            elementColors2.Add(TowerElement.Wind, wind2);


        }
        private void Update()
        {
            if (!towerSelectionArea.IsSelected)
                return;

            int slotTaken = 0;
            foreach (var element in elementsTracker.CurrTowerElements)
            {
                if (element == TowerElement.None)
                    continue;
                
                slotTaken++;               
            }

            if (slotTaken != 1)
                return;

            if(canvas.enabled == false)
            {
                TowerElement catalystElement = elementsTracker.CurrTowerElements[0];
                Color buttonColor = elementColors[catalystElement];
                catalystButtonBehaviour.SetupCatalyseButton(towerHolder, catalystElement, buttonColor);
                canvas.enabled = true;
            }
            
        }

        public void DeactivateAllUICatalyst()
        {
            canvas.enabled = false;   
        }

        public void ShowTargetCatalyseButtonOnly(SelectionAreaBehaviour selection)
        {
            if (towerHolder == selection.TowerHolder)
                return;
        
            canvas.enabled = false;        
        }

        public void DeactivateAllUICatalyst(Transform tower, TowerElement element)
        {
            canvas.enabled = false;
        }

        public void ShowTargetFuseButtonOnly(Transform targetTower)
        {
            if (targetTower != towerHolder)
            {
                canvas.enabled = false;
                return;
            }
                
            TowerElement catalystElement = elementsTracker.CurrTowerElements[0];
            Color buttonColor = elementColors2[catalystElement];
            catalystButtonBehaviour.SetupFuseButton(towerHolder, catalystElement, buttonColor);
            canvas.enabled = true;
        }

    }
   
}
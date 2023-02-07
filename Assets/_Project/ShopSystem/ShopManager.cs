using TD.ElementSystem;
using TD.Entities.Towers;
using TD.MonetarySystem;
using TD.TowersManager.Storer;
using TD.TowersManager.TowerSelectionManager;
using TD.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TD.ShopSystem
{

    public class ShopManager : MonoBehaviour
    {
        [Header("Towers Shop")]
        [Tooltip("Tower Can Buy Panel Color")]
        [SerializeField] private Color towerPanelCanBuyColor;
        [Tooltip("Tower Cannot Buy Panel Color")]
        [SerializeField] private Color towerPanelCannotBuyColor;
        [SerializeField] private TowerShopData[] towersInShop;

        [Header("Elements Shop")]
        [SerializeField] private ElementShopData[] elementsInShop;

        [Header("BackInShop")]
        [SerializeField] private BackInShop backInShop;

        public delegate void TowerBuyCallback(Transform tower, int cost);
        public static event TowerBuyCallback OnTowerBuy;

        public delegate void TowerSellCallback(Transform tower, float sellMoney);
        public static event TowerSellCallback OnTowerSell;

        public delegate void ElementQueueForBuyCallback(ElementShopData elementShopData);
        public static event ElementQueueForBuyCallback OnElementQueueForBuy;

        public delegate void ElementUnqueueForBuyCallback(TowerElement element);
        public static event ElementUnqueueForBuyCallback OnElementUnqueueForBuy;


        private void OnEnable()
        {
            UIShopModelSelector.OnTowerModelSelected += BuyTower;
            TowerSelectionManager.OnTowerSelectedSell += SellTower;

            UIElementSelectorManager.OnUIElementSelected += QueueElementForBuy;
            UIElementSelectorManager.OnUIElementUnselected += UnqueueElementForBuy;

        }
        private void OnDisable()
        {
            UIShopModelSelector.OnTowerModelSelected -= BuyTower;
            TowerSelectionManager.OnTowerSelectedSell -= SellTower;

            UIElementSelectorManager.OnUIElementSelected -= QueueElementForBuy;
            UIElementSelectorManager.OnUIElementUnselected -= UnqueueElementForBuy;
        }

        private void Start()
        {
            foreach(var towerModel in towersInShop)
            {
                towerModel.costDisplay.text = towerModel.cost.ToString();
                towerModel.costDisplay.text += "$";
            }

            foreach(var elementModel in elementsInShop)
            {
                elementModel.costDisplay.text = elementModel.cost.ToString();
                elementModel.costDisplay.text += "$";
            }
        }

        private void Update()
        {
            CheckTowerAvailableForBuy();
            CheckElementAvailableForBuy();
        }

        public void BuyTower(Transform model)
        {
            if (backInShop.Tower != null) backInShop.PutTowerBackInShop();
            else
            {
                foreach (var tower in towersInShop)
                {
                    if (tower.towerPrefab == model)
                    {
                        if (MoneyManager.Instance.Money >= tower.cost)
                        {
                            OnTowerBuy?.Invoke(tower.towerPrefab, tower.cost);
                        }
                    }
                }
            }
        }
        private void SellTower(Transform towerSelected)
        {
            TowerTypeAccessor towerTypeAccessor = towerSelected.GetComponent<TowerTypeAccessor>();
            TowerType selectedTowerType = towerTypeAccessor.TowerType;

            foreach (var shopTower in towersInShop)
            {
                if (shopTower.towerType == selectedTowerType)
                {
                    TowerStorer.Instance.DeployedTowers.Remove(towerSelected);
                    Destroy(towerSelected.gameObject);
                    towerSelected = null;

                    float sellMoney = shopTower.cost / 2;
                    OnTowerSell?.Invoke(towerSelected, sellMoney);
                }
            }
        }

        private void QueueElementForBuy(TowerElement element)
        {
            ElementShopData elementShopData = FindElementDataInShop(element);
            OnElementQueueForBuy?.Invoke(elementShopData);
        }
        private void UnqueueElementForBuy(TowerElement element)
        {
            OnElementUnqueueForBuy?.Invoke(element);
        }
        private void CheckTowerAvailableForBuy()
        {
            //Color canBuyColor = new Color(0.2783573f, 1, 0, 1);
            //Color cannotBuyColor = new Color(1, 0.04518003f, 0, 1);
            Color canBuyColor = towerPanelCanBuyColor;
            Color cannotBuyColor = towerPanelCannotBuyColor;
            foreach(var tower in towersInShop)
            {
                if (MoneyManager.Instance.Money >= tower.cost)
                {
                    tower.panel.color = canBuyColor;
                }
                else
                {
                    tower.panel.color = cannotBuyColor;
                }
            }
        }
        public void CheckElementAvailableForBuy()
        {
            /*Color canBuyColor = new Color(1,0.7521507f, 0,1);
            Color cannotBuyColor = new Color(0.6980392f, 0.6980392f, 0.6980392f,1);
            */

            foreach(var element in elementsInShop)
            {

                Color canBuyColor = element.panelCanBuyColor;
                Color cannotBuyColor = element.panelCannotBuyColor;
                Sprite canBuyVisualizer = element.canBuyVisualizer;
                Sprite cannotBuyVisualizer = element.cannotBuyVisualizer;

                if (MoneyManager.Instance.Money >= element.cost)
                {
                    element.panel.color = canBuyColor;
                    element.visualizer.sprite = canBuyVisualizer;             
                }
                else
                {
                    element.panel.color = cannotBuyColor;
                    element.visualizer.sprite = cannotBuyVisualizer;
                }
            }

        }     
        private ElementShopData FindElementDataInShop(TowerElement element)
        {
            foreach(var elementData in elementsInShop)
            {
                if(elementData.element == element)
                {
                    return elementData;
                }
            }

            return null;
        }

       
    }
}

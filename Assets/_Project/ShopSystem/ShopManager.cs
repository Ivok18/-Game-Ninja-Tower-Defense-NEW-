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
        [Tooltip("Tower Shop")]
        [SerializeField] private TowerScriptableObject[] towersInShop;

        [Header("Elements Shop")]
        [Tooltip("Element Shop")]
        [SerializeField] private ElementScriptableObject[] elementsInShop;

        [Header("BackInShop")]
        [SerializeField] private BackInShop backInShop;

        public TowerScriptableObject[] TowersInShop => towersInShop;
        public ElementScriptableObject[] ElementsInShop => elementsInShop;

        public delegate void TowerBuyCallback(Transform tower, int cost);
        public static event TowerBuyCallback OnTowerBuy;

        public delegate void TowerSellCallback(Transform tower, float sellMoney);
        public static event TowerSellCallback OnTowerSell;

        public delegate void ElementQueueForBuyCallback(ElementScriptableObject elementData);
        public static event ElementQueueForBuyCallback OnElementQueueForBuy;

        public delegate void ElementUnqueueForBuyCallback(ElementScriptableObject elementData);
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

        public void BuyTower(TowerScriptableObject towerData)
        {
            if (backInShop.Tower != null) 
                backInShop.PutTowerBackInShop();
            else
            {
                foreach (var shopTower in towersInShop)
                {
                    bool doesShopModelCorrespondToBuyChoice = shopTower == towerData;
                    if (!doesShopModelCorrespondToBuyChoice)
                        continue;

                    bool hasEnoughMoney = MoneyManager.Instance.Money >= shopTower.Cost;
                    if (!hasEnoughMoney)
                        continue;

                    OnTowerBuy?.Invoke(shopTower.Preview, shopTower.Cost);
                    return;
                }
            }
        }
        private void SellTower(Transform towerSelected)
        {
            TowerTypeAccessor towerTypeAccessor = towerSelected.GetComponent<TowerTypeAccessor>();
            TowerType selectedTowerType = towerTypeAccessor.TowerType;

            foreach (var shopTower in towersInShop)
            {
                bool isTheDataOfTheTowerToSell = shopTower.TowerType == selectedTowerType;
                if (!isTheDataOfTheTowerToSell)
                    continue;


                TowerStorer.Instance.DeployedTowers.Remove(towerSelected);
                Destroy(towerSelected.gameObject);
                towerSelected = null;

                float sellMoney = shopTower.Cost / 2;
                OnTowerSell?.Invoke(towerSelected, sellMoney);
                return;
            }
        }

        private void QueueElementForBuy(ElementScriptableObject elementData)
        {
         
            OnElementQueueForBuy?.Invoke(elementData);
        }
        private void UnqueueElementForBuy(ElementScriptableObject elementData)
        {
            OnElementUnqueueForBuy?.Invoke(elementData);
        }
    }
}

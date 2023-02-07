using TD.ElementSystem;
using TD.Entities.Enemies;
using TD.Entities.Towers;
using TD.ShopSystem;
using UnityEngine;
using UnityEngine.UI;

namespace TD.MonetarySystem
{
    public class MoneyManager : MonoBehaviour
    {
        [SerializeField] private float money;
        [SerializeField] private Text moneyText;
        public static MoneyManager Instance;

        public float  Money => money;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            UpdateMoneyFormat();
        }
        private void OnEnable()
        {
            ShopManager.OnTowerSell += GainMoneyOnTowerSell;
            BackInShop.OnTowerBackInShop += GainMoneyOnTowerPutBackInShop;
            DamageReceiver.OnEnemyDead += GainMoneyOnTowerKillEnemy;

            ShopManager.OnTowerBuy += LoseMoneyOnTowerBuy;
            ElementDataApplier.OnElementDataAppliedOnTower += LooseMoneyOnElementAppliedOnTower;
        }

        private void OnDisable()
        {

            ShopManager.OnTowerSell -= GainMoneyOnTowerSell;
            BackInShop.OnTowerBackInShop -= GainMoneyOnTowerPutBackInShop;
            DamageReceiver.OnEnemyDead -= GainMoneyOnTowerKillEnemy;

            ShopManager.OnTowerBuy -= LoseMoneyOnTowerBuy;
            ElementDataApplier.OnElementDataAppliedOnTower -= LooseMoneyOnElementAppliedOnTower;

        }

        private void LoseMoneyOnTowerBuy(Transform tower, int cost)
        {           
            money -= cost;
            money = Mathf.Clamp(money,0,Mathf.Infinity); //Make sure money value is never negative
            UpdateMoneyFormat();
        }

        private void LooseMoneyOnElementAppliedOnTower(Transform tower, TowerElement element, int elementCost)
        {
            money -= elementCost;
            money = Mathf.Clamp(money, 0, Mathf.Infinity); //Make sure money value is never negative
            UpdateMoneyFormat();
        }
        private void GainMoneyOnTowerSell(Transform tower, float sellMoney)
        {
            money += sellMoney;
            UpdateMoneyFormat();
        }

        private void GainMoneyOnTowerPutBackInShop(Transform tower, int cost)
        {
            money += cost;
            UpdateMoneyFormat();
        }

        private void GainMoneyOnTowerKillEnemy(Transform enemy, Transform killerTower, float reward)
        {
            money += reward;
            UpdateMoneyFormat();
        }

        private void UpdateMoneyFormat()
        {
            //If money is a thousand(1000) value ..
            if (money >= 1000 && money < 1000000)
            {
                //get quotient of division by 1000
                int formattedMoney = (int)(money / 1000);
          
                //get remainder 
                int remainder = (int)(money % 1000);
  
                //if remainder is less than 100
                if(remainder >= 0 && remainder < 100)
                {
                    //just write the quotient
                    moneyText.text = formattedMoney.ToString();
                }
                else if(remainder >= 100 && remainder < 1000)
                {
                    //Get first first digit
                    remainder /= 100;
                    moneyText.text = formattedMoney.ToString() + "." + remainder.ToString();
                }
           
                //Add thousands symbol
                moneyText.text += "K";
            }
            else
            {
                //just write the current money (no formatting)
                moneyText.text = money.ToString();
            }   
        }
    }
}

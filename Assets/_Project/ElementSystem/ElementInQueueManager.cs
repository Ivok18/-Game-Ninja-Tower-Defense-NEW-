using System.Collections.Generic;
using TD.Map;
using TD.ShopSystem;
using UnityEngine;

namespace TD.ElementSystem
{
    public class ElementInQueueManager : MonoBehaviour
    {
        //Only used to see first element in queue in the inspector instead of the actual queue
        //Because it's only the first element we want to see
        //Better visualization
        //This variable serves no other puropose than that
        [SerializeField] private TowerElement elementInQueue;

        private List<ElementShopData> currElementsInQueue;
        
        public delegate void ElementAddedToQueueCallback(TowerElement element, int cost);
        public static event ElementAddedToQueueCallback OnElementAddedToQueue;

        public delegate void QueueClearCallback(TowerElement element);
        public static event QueueClearCallback OnQueueClear;
         

        private void Awake()
        {
            currElementsInQueue = new List<ElementShopData>();
        }

        private void OnEnable()
        {
            ShopManager.OnElementQueueForBuy += PutInQueue;
            ShopManager.OnElementUnqueueForBuy += ClearQueue;
            ClickOnMap.OnMapClick += TryClearQueue;
        }
        private void OnDisable()
        {
            ShopManager.OnElementQueueForBuy -= PutInQueue;
            ShopManager.OnElementUnqueueForBuy -= ClearQueue;
            ClickOnMap.OnMapClick -= TryClearQueue;
        }

        private void PutInQueue(ElementShopData elementShopData)
        {
           
            if(currElementsInQueue.Count > 0)  //Swap if queue is not empty
            {
                elementInQueue = elementShopData.element;
                OnElementAddedToQueue?.Invoke(elementShopData.element, elementShopData.cost);
            }
            else //Add if queue is empty
            {
                currElementsInQueue.Add(elementShopData);
                elementInQueue = currElementsInQueue[0].element;
                OnElementAddedToQueue?.Invoke(elementInQueue, elementShopData.cost);
            }               
        }

        private void ClearQueue(TowerElement element)
        {
            if(currElementsInQueue.Count > 0)
            {
                currElementsInQueue.RemoveAt(0);
                elementInQueue = TowerElement.None;

                OnQueueClear?.Invoke(elementInQueue);
            }
        }

        //Clear queue if it is not emppty
        private void TryClearQueue()
        {
            if (currElementsInQueue.Count > 0)
            {
                currElementsInQueue.RemoveAt(0);
                elementInQueue = TowerElement.None;

                OnQueueClear?.Invoke(elementInQueue);
            }
        }
    }

}

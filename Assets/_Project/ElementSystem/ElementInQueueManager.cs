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

        private List<ElementScriptableObject> currElementsInQueue;
        
        public delegate void ElementAddedToQueueCallback(ElementScriptableObject elementData);
        public static event ElementAddedToQueueCallback OnElementAddedToQueue;

        public delegate void QueueClearCallback(TowerElement element);
        public static event QueueClearCallback OnQueueClear;
         

        private void Awake()
        {
            currElementsInQueue = new List<ElementScriptableObject>();
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

        private void PutInQueue(ElementScriptableObject elementData)
        {
            bool isThereAlreadyAnElementInQueue = currElementsInQueue.Count > 0;
            if (isThereAlreadyAnElementInQueue)  //Swap if queue is not empty
            {
                elementInQueue = elementData.Element;
                OnElementAddedToQueue?.Invoke(elementData);
            }
            else //Add if queue is empty
            {
                currElementsInQueue.Add(elementData);
                elementInQueue = currElementsInQueue[0].Element;
                OnElementAddedToQueue?.Invoke(elementData);
            }               
        }

        private void ClearQueue(ElementScriptableObject elementData)
        {
            bool isQueueEmpty = currElementsInQueue.Count <= 0;
            if (!isQueueEmpty)
            { 
                currElementsInQueue.RemoveAt(0);
                elementInQueue = TowerElement.None;

                OnQueueClear?.Invoke(elementInQueue);
            }
        }

        //Clear queue if it is not emppty
        private void TryClearQueue()
        {
            bool isQueueEmpty = currElementsInQueue.Count <= 0;
            if (!isQueueEmpty)
            { 
                currElementsInQueue.RemoveAt(0);
                elementInQueue = TowerElement.None;

                OnQueueClear?.Invoke(elementInQueue);
            }
        }
    }

}

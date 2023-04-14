using System.Collections;
using System.Collections.Generic;
using TD.Entities.Towers;
using UnityEngine;

namespace TD.TowersManager.TowerSelectionManager
{
    public class DoubleClickOnSelectionAreaDetector : MonoBehaviour
    {
        private float firstClickTime;
        private SelectionAreaBehaviour selectionAreaBehaviour;
        [SerializeField] bool coroutineAllowed;
        [SerializeField] private float timeBetweenClicks;
        [SerializeField] private int clickCounter;

        public delegate void DoubleClickOnSelectionAreaCallback(Transform selectionAreaTowerHolder);
        public static event DoubleClickOnSelectionAreaCallback OnDoubleClickOnSelectionArea;

        private void Awake()
        {
            selectionAreaBehaviour = GetComponent<SelectionAreaBehaviour>();
        }

        private void Start()
        {
            firstClickTime = 0f;
            coroutineAllowed = true;
        }

        private void Update()
        {
            TowerStateSwitcher towerStateSwitcher = selectionAreaBehaviour.TowerHolder.GetComponent<TowerStateSwitcher>();
            if (towerStateSwitcher.CurrentTowerState == TowerState.Undeployed)
                return;

            if(clickCounter == 1 && coroutineAllowed)
            {
                firstClickTime = Time.time;
                StartCoroutine(DoubleClickDetection());
            }
        }

        private IEnumerator DoubleClickDetection()
        {
            coroutineAllowed = false;
            while(Time.time < firstClickTime + timeBetweenClicks)
            {
                if(clickCounter == 2)
                {
                    OnDoubleClickOnSelectionArea?.Invoke(selectionAreaBehaviour.TowerHolder);
                    break;
                }
                yield return new WaitForEndOfFrame();
            }

            clickCounter = 0;
            firstClickTime = 0f;
            coroutineAllowed = true;
        }

        private void OnMouseDown()
        {
            clickCounter++;
        }
    }
}

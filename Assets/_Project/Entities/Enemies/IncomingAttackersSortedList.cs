using System.Collections;
using System.Collections.Generic;
using TD.Entities.Towers;
using TD.TowersManager.Storer;
using UnityEngine;

namespace TD.Entities.Enemies
{
    public class IncomingAttackersSortedList : MonoBehaviour
    {
        [SerializeField] private List<Transform> incomingAttackers;


        private void OnEnable()
        {
            TowerPlacer.OnTowerPlaced += AddToList;
        }

        private void OnDisable()
        {
            TowerPlacer.OnTowerPlaced -= AddToList;
        }

        private void Start()
        {
            foreach(var deployedTower in TowerStorer.Instance.DeployedTowers)
            {
                Add(deployedTower);
             
            }
        }

        public void AddToList(Transform tower)
        {
            Add(tower);
        }

        public void Add(Transform tower)
        {
            incomingAttackers.Add(tower);
        }

        public void Remove(Transform tower)
        {

        }

        public Transform Find(Transform tower)
        {
            return null;
        }
    }
}
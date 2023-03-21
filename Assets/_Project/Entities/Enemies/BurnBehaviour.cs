using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TD.Entities.Enemies
{
    public class BurnBehaviour : MonoBehaviour
    {
        public bool[] IsBurning;
        [SerializeField] private int noOfBurnStrikes;
        [SerializeField] private int noOfBurnStrikesRemaining;
        [SerializeField] private int burnDamage;
        [SerializeField] private float timeBetweenBurnDamages;
        [SerializeField] private float timeUntilNextBurnDamage;
        [SerializeField] private Transform dummyTower;

        private void Start()
        {
            IsBurning = new bool[1];
            noOfBurnStrikesRemaining = noOfBurnStrikes;
            timeUntilNextBurnDamage = timeBetweenBurnDamages;
           
        }

        private void Update()
        {
            if (!IsBurning[0])
                return;

            if (noOfBurnStrikesRemaining <= 0)
            {
                IsBurning[0] = false;
                noOfBurnStrikesRemaining = noOfBurnStrikes;
                return;
            }

            if(timeUntilNextBurnDamage > 0)
            {
                timeUntilNextBurnDamage -= Time.deltaTime;
            }
            else
            {
                GameObject dummyTowerGo = Instantiate(dummyTower.gameObject, new Vector3(-100,-100,100), Quaternion.identity);
                HealthBehaviour healthBehaviour = GetComponent<HealthBehaviour>();
                healthBehaviour.GetDamage(burnDamage, dummyTower);
                timeUntilNextBurnDamage = timeBetweenBurnDamages;
                noOfBurnStrikesRemaining--;
                Destroy(dummyTowerGo);
            }

        }


        
    }

}
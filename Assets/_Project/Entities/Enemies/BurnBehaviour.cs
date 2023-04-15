using TD.Entities.Towers;
using UnityEngine;


namespace TD.Entities.Enemies
{
    public class BurnBehaviour : MonoBehaviour
    {
        public bool[] ValueContainer;
        [SerializeField] private int noOfBurnStrikes;
        [SerializeField] private int noOfBurnStrikesRemaining;
        [SerializeField] private int burnDamage;
        public float TimeBetweenBurnDamages;
        public float TimeUntilNextBurnDamage;
        [SerializeField] private Transform dummyTower;

        private void Start()
        {
            ValueContainer = new bool[1];
            noOfBurnStrikesRemaining = noOfBurnStrikes;
            TimeUntilNextBurnDamage = TimeBetweenBurnDamages;
           
        }

        private void Update()
        {
            if (!IsBurning())
                return;

            bool hasInflictedAllBurnStrikes = noOfBurnStrikesRemaining <= 0 ? true : false;
            if (hasInflictedAllBurnStrikes)
            {
                ValueContainer[0] = false;
                noOfBurnStrikesRemaining = noOfBurnStrikes;
                return;
            }

            bool isWaitingForNextBurnStrike = TimeUntilNextBurnDamage > 0 ? true : false;
            if (isWaitingForNextBurnStrike)
            {
                TimeUntilNextBurnDamage -= Time.deltaTime;
            }
            else
            {
                GameObject dummyTowerGo = Instantiate(dummyTower.gameObject, new Vector3(-100,-100,100), Quaternion.identity);
                HealthBehaviour healthBehaviour = GetComponent<HealthBehaviour>();
                healthBehaviour.GetDamage(burnDamage, dummyTower);
                TimeUntilNextBurnDamage = TimeBetweenBurnDamages;
                noOfBurnStrikesRemaining--;
                Destroy(dummyTowerGo);
            }

        }

        public bool IsBurning()
        {
            return ValueContainer[0] == true ? true : false;
        } 
    }

}
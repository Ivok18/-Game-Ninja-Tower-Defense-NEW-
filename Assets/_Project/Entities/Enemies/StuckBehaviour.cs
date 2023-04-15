using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TD.Entities.Enemies
{
    public class StuckBehaviour : MonoBehaviour
    {
        public bool[] ValueContainer;
        [SerializeField] private int noOfStuckStrikes;
        [SerializeField] private int noOfStuckStrikesRemaining;
        [SerializeField] private int stuckDamage;
        public float TimeBetweenStuckDamages;
        public float TimeUntilNextStuckDamage;
        [SerializeField] private Transform dummyTower;
        private EnemyMovement enemyMovement;

        private void Awake()
        {
            enemyMovement = GetComponent<EnemyMovement>();
        }


        private void Start()
        {
            ValueContainer = new bool[1];
            TimeUntilNextStuckDamage = TimeBetweenStuckDamages;
            noOfStuckStrikesRemaining = noOfStuckStrikes;

        }

        private void Update()
        {
            if (!IsStuck())
            {
                enemyMovement.CurrentSpeed = enemyMovement.Speed;
                return;
            }

            enemyMovement.CurrentSpeed = 0;

            bool hasInflictedAllStuckStrikes = noOfStuckStrikesRemaining <= 0 ? true : false;
            if (hasInflictedAllStuckStrikes)
            {
                ValueContainer[0] = false;
                noOfStuckStrikesRemaining = noOfStuckStrikes;
                return;
            }

            bool isWaitingForNextStuckStrike = TimeUntilNextStuckDamage > 0 ? true : false;
            if (isWaitingForNextStuckStrike)
            {
                TimeUntilNextStuckDamage -= Time.deltaTime;
            }
            else
            {
                GameObject dummyTowerGo = Instantiate(dummyTower.gameObject, new Vector3(-100, -100, 100), Quaternion.identity);
                HealthBehaviour healthBehaviour = GetComponent<HealthBehaviour>();
                healthBehaviour.GetDamage(stuckDamage, dummyTower);
                TimeUntilNextStuckDamage = TimeBetweenStuckDamages;
                noOfStuckStrikesRemaining--;
                Destroy(dummyTowerGo);
            }

        }

        public bool IsStuck()
        {
            return ValueContainer[0] == true ? true : false;
        }


    }
}

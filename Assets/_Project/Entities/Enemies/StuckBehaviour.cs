using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TD.Entities.Enemies
{
    public class StuckBehaviour : MonoBehaviour
    {
        public bool[] IsStuck;
        [SerializeField] private int noOfStuckStrikes;
        [SerializeField] private int noOfStuckStrikesRemaining;
        [SerializeField] private int stuckDamage;
        [SerializeField] private float timeBetweenStuckDamages;
        [SerializeField] private float timeUntilNextStuckDamage;
        [SerializeField] private Transform dummyTower;
        private EnemyMovement enemyMovement;

        private void Awake()
        {
            enemyMovement = GetComponent<EnemyMovement>();
        }


        private void Start()
        {
            IsStuck = new bool[1];
            timeUntilNextStuckDamage = timeBetweenStuckDamages;
            noOfStuckStrikesRemaining = noOfStuckStrikes;

        }

        private void Update()
        {
            if (!IsStuck[0])
            {
                enemyMovement.CurrentSpeed = enemyMovement.Speed;
                return;
            }

            enemyMovement.CurrentSpeed = 0;

            if (noOfStuckStrikesRemaining <= 0)
            {
                IsStuck[0] = false;
                noOfStuckStrikesRemaining = noOfStuckStrikes;
                return;
            }

            if (timeUntilNextStuckDamage > 0)
            {
                timeUntilNextStuckDamage -= Time.deltaTime;
            }
            else
            {
                GameObject dummyTowerGo = Instantiate(dummyTower.gameObject, new Vector3(-100, -100, 100), Quaternion.identity);
                HealthBehaviour healthBehaviour = GetComponent<HealthBehaviour>();
                healthBehaviour.GetDamage(stuckDamage, dummyTower);
                timeUntilNextStuckDamage = timeBetweenStuckDamages;
                noOfStuckStrikesRemaining--;
                Destroy(dummyTowerGo);
            }

        }


    }
}

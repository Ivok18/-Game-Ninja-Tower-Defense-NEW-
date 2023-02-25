using UnityEngine;
using TD.EnemiesManager.Storer;
using TD.Entities.Towers.States;
using TD.Entities.Enemies;
using TD.ElementSystem;
using UI;

namespace TD.Entities.Towers
{
    public class RadiusDetectionBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform TowerHolder;
        public SpriteRenderer RadiusVizualizer;
        public float Radius;
        private TowerStateSwitcher towerStateSwitcher;
        
        public ListOfTargets ListOfTargets { get; set; }

        private void OnEnable()
        {
            LimitBreakActioner.OnTowerLimitBreak += UpdateRadiusVizualizer;
            UICatalystButtonBehaviour.OnCatalyseButtonPressed += ReduceRadius;
        }

        private void OnDisable()
        {
            LimitBreakActioner.OnTowerLimitBreak -= UpdateRadiusVizualizer;
            UICatalystButtonBehaviour.OnCatalyseButtonPressed -= ReduceRadius;
        }

        public void UpdateRadiusVizualizer(Transform tower)
        {
            if(TowerHolder == tower) 
                RadiusVizualizer.transform.localScale = new Vector3(Radius * 2.3f, Radius * 2.3f, Radius * 2.3f);
        }

        public void ReduceRadius(Transform targetTower, TowerElement elementOfCatalyst)
        {
            if (targetTower != TowerHolder)
                return;

            Radius /= 2.2f;
        }

        private void Awake()
        {
            towerStateSwitcher = GetComponentInParent<TowerStateSwitcher>();
            ListOfTargets = GetComponentInParent<ListOfTargets>();
        }

        void Start()
        {
            RadiusVizualizer.transform.localScale = new Vector3(Radius * 2.3f, Radius * 2.3f, Radius * 2.3f);
        }

        private void Update()
        {
            if (towerStateSwitcher.CurrentTowerState == TowerState.Undeployed) 
                return;

            if (TowerHolder != null) 
                ProcessRadiusDetection();
            else 
                RadiusVizualizer.enabled = false;         
        }

        public void ProcessRadiusDetection()
        {
            foreach (Transform enemyOnMap in EnemyStorer.Instance.Enemies)
            {
                bool doesEnemyObjectFoundExist = enemyOnMap != null;
                if (!doesEnemyObjectFoundExist)
                    continue;

                bool isItInTowerRadius = IsInRadius(enemyOnMap);
                if (isItInTowerRadius)
                {

                    /* if (enemyMovement.IsWinded)  //inside radius, remove any enemy affected by wind element from potential targets
                        {
                            ListOfTargets.EnemiesToAttack.Remove(enemyOnMap);
                        }*/
                    bool isItDead = enemyOnMap.CompareTag("Dead");
                    bool isItHereToAnnounceVictory = enemyOnMap.CompareTag("Victorious");
                    if (isItDead || isItHereToAnnounceVictory) //inside radius, remove any enemy flagged "Dead" from potential targets
                    {
                        ListOfTargets.EnemiesToAttack.Remove(enemyOnMap);
                    }
                    else
                    {
                        bool doesItHaveClonesInTowerRadius = HasClonesInRadius(enemyOnMap);

                        if (!doesItHaveClonesInTowerRadius)                           
                        {
                            //Confirm the enemy has entered the radius (not its clones)
                            RadiusEntrySignaler radiusEntrySignaler = enemyOnMap.GetComponent<RadiusEntrySignaler>();
                            radiusEntrySignaler.HasEnteredRadius = true;

                            //If an enemy inside the radius has not been hit yet by wind element, add it to
                            //enemies to attack
                            ListOfTargets.EnemiesToAttack.Add(enemyOnMap);
                        }
                    }              
                }
                else
                {
                    RadiusEntrySignaler radiusEntrySignaler = enemyOnMap.GetComponent<RadiusEntrySignaler>();
                    RadiusExitSignaler radiusExitSignaler = enemyOnMap.GetComponent<RadiusExitSignaler>();

                    bool hasEnemyEnteredTheTowerRadius = radiusEntrySignaler.HasEnteredRadius;
                    if (hasEnemyEnteredTheTowerRadius)
                    {
                        radiusExitSignaler.HasExitedRadius = true;
                        bool hasEnemyExitedTheTowerRadius = radiusExitSignaler.HasExitedRadius;
                        if (hasEnemyExitedTheTowerRadius)
                        {
                            LockTargetState lockTargetState = TowerHolder.GetComponent<LockTargetState>();
                            bool isEnemyTheTargetOfTheTower = lockTargetState.Target == enemyOnMap.transform;
                            if (isEnemyTheTargetOfTheTower)
                            {
                                ListOfTargets.SwitchTargetFrom(enemyOnMap);
                            }
                        }

                    }
                    ListOfTargets.EnemiesToAttack.Remove(enemyOnMap);
                }
                
               
            }
        }

        private bool HasClonesInRadius(Transform enemyOnMap)
        {
            return ListOfTargets.CloneFinder.FindClone(enemyOnMap);
        }

        private bool TowerIsAttacking()
        {
            return towerStateSwitcher.CurrentTowerState == TowerState.Attacking;
        }

        private bool IsInRadius(Transform enemyOnMap)
        {
            return Vector2.Distance(enemyOnMap.transform.position, transform.position) <= Radius;
        }

        
    }

}

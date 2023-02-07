using UnityEngine;
using TD.EnemiesManager.Storer;
using TD.Entities.Towers.States;
using TD.Entities.Enemies;


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
        }

        private void OnDisable()
        {
            LimitBreakActioner.OnTowerLimitBreak -= UpdateRadiusVizualizer;
        }

        private void UpdateRadiusVizualizer(Transform tower)
        {
            if(TowerHolder == tower) RadiusVizualizer.transform.localScale = new Vector3(Radius * 1.79f, Radius * 1.79f, Radius * 1.79f);
        }

        private void Awake()
        {
            towerStateSwitcher = GetComponentInParent<TowerStateSwitcher>();
            ListOfTargets = GetComponentInParent<ListOfTargets>();
        }

        void Start()
        {
            TowerHolder = transform.parent;
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
                if (enemyOnMap != null)
                {
                    if (IsInRadius(enemyOnMap))
                    {
                        
                        /* if (enemyMovement.IsWinded)  //inside radius, remove any enemy affected by wind element from potential targets
                         {
                             ListOfTargets.EnemiesToAttack.Remove(enemyOnMap);
                         }*/
                        if (enemyOnMap.CompareTag("Dead") || enemyOnMap.CompareTag("Victorious")) //inside radius, remove any enemy flagged "Dead" from potential targets
                        {
                            ListOfTargets.EnemiesToAttack.Remove(enemyOnMap);
                        }
                        else
                        {
                            if (!HasClonesInRadius(enemyOnMap))
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
                        if (radiusEntrySignaler.HasEnteredRadius)
                        {
                            radiusExitSignaler.HasExitedRadius = true;
                            if (radiusExitSignaler.HasExitedRadius)
                            {
                                LockTargetState lockTargetState = TowerHolder.GetComponent<LockTargetState>();
                                if (lockTargetState.Target == enemyOnMap.transform)
                                {
                                    ListOfTargets.SwitchTargetFrom(enemyOnMap);
                                }
                            }

                        }
                        ListOfTargets.EnemiesToAttack.Remove(enemyOnMap);
                    }
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

using UnityEngine;
using TD.Entities.Towers.States;

namespace TD.Entities.Towers.AttackPattern
{
    public class PatternBehaviour : MonoBehaviour
    {
        [SerializeField] private bool hasBeenReached;
        public float minReachDistance;
        [SerializeField] private float minReachDistanceCoef = 15; 
        [SerializeField] Transform tower;
        [SerializeField] private Transform parent;
        [SerializeField] private Vector3 startPosition;

        private void Start()
        {
            parent = transform.parent;

            AttackState attackBehaviour = parent.GetComponentInParent<AttackState>();
            minReachDistance = attackBehaviour.BaseDashSpeed / (minReachDistanceCoef);
        }

        public bool HasBeenReached 
        {
            get => hasBeenReached;
            set => hasBeenReached = value;
        }

        public void AttachToParent()
        {
            transform.parent = parent;
            transform.localPosition = startPosition;
        
        }

    
        private void OnTriggerExit2D(Collider2D collision)
        {
            bool isCollidingWhithHolder = collision.transform == tower;
            if (!isCollidingWhithHolder)
            {
                //Debug.Log("is not colliding with holder");
                return;
            }
                

            AttackState attackState = tower.GetComponent<AttackState>();
            bool isNextPatternToReach = attackState.NextPattern == this;
            if (!isNextPatternToReach)
            {
                //Debug.Log("is not next pattern to reach");
                return;
            }
               

            bool isVeryCloseToHolder = Vector2.Distance(transform.position, tower.position) <= minReachDistance;
            if (!isVeryCloseToHolder)
            {
                //Debug.Log("is not close to holder");
                return;
            }
                
            
            HasBeenReached = true;
            
        }
    }

}

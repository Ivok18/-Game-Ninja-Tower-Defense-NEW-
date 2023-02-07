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
            if (collision.transform == tower)
            {
                AttackState attackState = tower.GetComponent<AttackState>();
                if (attackState.NextPattern == this)
                {
                    if (Vector2.Distance(transform.position, tower.position) <= minReachDistance)
                    {
                        HasBeenReached = true;
                    }
                }

            }
        }
    }

}

using UnityEngine;
using TD.Entities.Towers.States;

namespace TD.Entities.Towers.AttackPattern
{
    public class PatternBehaviour : MonoBehaviour
    {
        public bool HasBeenReached;
        [SerializeField] Transform tower;
        [SerializeField] private Transform parent;
        [SerializeField] private Vector3 startPosition;

        private void Start()
        {
            parent = transform.parent;
        }
        public void AttachToParent()
        {
            transform.parent = parent;
            transform.localPosition = startPosition;
        }
    }

}

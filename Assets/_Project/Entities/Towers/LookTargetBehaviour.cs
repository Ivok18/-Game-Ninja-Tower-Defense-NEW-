using UnityEngine;
using TD.Entities.Towers.States;
using static UnityEngine.GraphicsBuffer;

namespace TD.Entities.Towers
{
    public class LookTargetBehaviour : MonoBehaviour
    {
        private Rigidbody2D rb;
  
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }
        private void FixedUpdate()
        {
            LookTarget();
        }

        private void LookTarget()
        {
            LockTargetState lockTargetState = GetComponent<LockTargetState>();
            bool isThereATargetToLookAt = lockTargetState.Target != null;
            if (isThereATargetToLookAt)
            {
                Vector2 aimDirection = (Vector2)lockTargetState.Target.position - rb.position;
                float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
                rb.rotation = aimAngle;
            }
        }

        private void OnDrawGizmos()
        {
            LockTargetState lockTargetState = GetComponent<LockTargetState>();
            if (lockTargetState.Target != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, lockTargetState.Target.position);

            }

        }
    }
}


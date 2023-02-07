using UnityEngine;

namespace TD.Entities.Towers
{
    public class ChargeAttackBarBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform bar;
        public float CurrentValue;
        public float MaxValue;
 
        void Update()
        {
            float localScaleY = bar.localScale.y;
            float localScaleZ = bar.localScale.z;
            if (CurrentValue > 0 && MaxValue > 0)
            {
                bar.localScale = new Vector3(1 - (CurrentValue / MaxValue), localScaleY, localScaleZ);
            }        
        }       
    }

}

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
            CurrentValue = Mathf.Clamp(CurrentValue, 0, MaxValue);

            float localScaleY = bar.localScale.y;
            float localScaleZ = bar.localScale.z;
            bool uiChargeAttackBarDealsWithPositiveValues = CurrentValue > 0 && MaxValue > 0;
            if (uiChargeAttackBarDealsWithPositiveValues)
            {
                bar.localScale = new Vector3(1 - (CurrentValue / MaxValue), localScaleY, localScaleZ);
            }        
        }       
    }

}

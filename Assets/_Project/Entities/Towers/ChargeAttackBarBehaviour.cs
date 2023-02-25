using TD.Entities.Towers.States;
using UnityEngine;

namespace TD.Entities.Towers
{
    public class ChargeAttackBarBehaviour : MonoBehaviour
    {
        public Transform Bar;
        public GameObject Container;
        public float CurrentValue;
        public float MaxValue;
       
      

        void Update()
        {
            CurrentValue = Mathf.Clamp(CurrentValue, 0, MaxValue);

            float localScaleY = Bar.localScale.y;
            float localScaleZ = Bar.localScale.z;
            bool uiChargeAttackBarDealsWithPositiveValues = CurrentValue > 0 && MaxValue > 0;
            if (uiChargeAttackBarDealsWithPositiveValues)
            {
                Bar.localScale = new Vector3(1 - (CurrentValue / MaxValue), localScaleY, localScaleZ);
            }        
        }       
    }

}

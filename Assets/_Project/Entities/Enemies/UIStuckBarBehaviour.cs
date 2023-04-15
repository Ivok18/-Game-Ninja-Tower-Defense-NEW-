using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TD.Entities.Enemies
{
    public class UIStuckBarBehaviour : MonoBehaviour
    {
        private StuckBehaviour stuckBehaviour;
        [SerializeField] private Transform bar;

        private void Awake()
        {
            stuckBehaviour = GetComponent<StuckBehaviour>();
        }

        private void Update()
        {
            bool isStuck = stuckBehaviour.IsStuck() ? true : false;
            if (!isStuck)
                return;

            float currentValue = stuckBehaviour.TimeUntilNextStuckDamage;
            float maxValue = stuckBehaviour.TimeBetweenStuckDamages;
            currentValue = Mathf.Clamp(currentValue, 0, maxValue);

            float localScaleY = bar.localScale.y;
            float localScaleZ = bar.localScale.z;
            bool uiStuckBarDealsWithPositiveValues = currentValue > 0 && maxValue > 0;

            if (!uiStuckBarDealsWithPositiveValues)
                return;

            bool isWaitingForNextStuckStrike = stuckBehaviour.TimeUntilNextStuckDamage > 0 ? true : false;
            if (!isWaitingForNextStuckStrike)
                return;

            bar.localScale = new Vector3(1 - (currentValue / maxValue), localScaleY, localScaleZ);
        }
    }
}

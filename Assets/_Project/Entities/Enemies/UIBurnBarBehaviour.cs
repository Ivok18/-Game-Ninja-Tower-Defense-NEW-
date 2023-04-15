using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TD.Entities.Enemies
{
    public class UIBurnBarBehaviour : MonoBehaviour
    {
        private BurnBehaviour burnBehaviour;
        [SerializeField] private Transform bar;

        private void Awake()
        {
            burnBehaviour = GetComponent<BurnBehaviour>();
        }

        private void Update()
        {
            if (!burnBehaviour.IsBurning())
                return;

            float currentValue = burnBehaviour.TimeUntilNextBurnDamage;
            float maxValue = burnBehaviour.TimeBetweenBurnDamages;
            currentValue = Mathf.Clamp(currentValue, 0, maxValue);

            float localScaleY = bar.localScale.y;
            float localScaleZ = bar.localScale.z;
            bool uiBurnBarDealsWithPositiveValues = currentValue > 0 && maxValue > 0;

            if (!uiBurnBarDealsWithPositiveValues)
                return;

            bool isWaitingForNextBurnStrike = burnBehaviour.TimeUntilNextBurnDamage > 0 ? true : false;
            if (!isWaitingForNextBurnStrike)
                return;

            bar.localScale = new Vector3(1 - (currentValue / maxValue), localScaleY, localScaleZ);
        }

    }
}

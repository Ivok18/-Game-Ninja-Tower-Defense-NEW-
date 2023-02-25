using TD.Entities.Towers.States;
using UnityEngine;

namespace TD.Entities.Towers
{
    public class LimitBreakActioner : MonoBehaviour
    {

        public delegate void TowerLimitBreakCallback(Transform tower);
        public static event TowerLimitBreakCallback OnTowerLimitBreak;

        public LimitBreakNewVarsSetter LimitBreakNewVarsSetter
        {
            get => GetComponent<LimitBreakNewVarsSetter>();
        }

        public void LimitBreak()
        {
            //Set new attack rate for tower
            ChargeAttackState chargeAttackState = GetComponent<ChargeAttackState>();
            float limitBreakBonusAttackRate = chargeAttackState.CurrentTimeBetweenAttacks - LimitBreakNewVarsSetter.NewAttackRate;
            chargeAttackState.BoostTimeBetweenAttacks = -limitBreakBonusAttackRate;
            chargeAttackState.CurrentTimeBetweenAttacks = chargeAttackState.BaseTimeBetweenAttacks + chargeAttackState.BoostTimeBetweenAttacks;
            if (chargeAttackState.TimeUntilNextAttack >= chargeAttackState.CurrentTimeBetweenAttacks)
            {
                chargeAttackState.TimeUntilNextAttack = chargeAttackState.BaseTimeBetweenAttacks + chargeAttackState.BoostTimeBetweenAttacks;
            }

            //Set new radius
            RadiusDetectionBehaviour radiusDetectionBehaviour = GetComponent<RadiusGetter>().RadiusTransform.GetComponent< RadiusDetectionBehaviour>();
            radiusDetectionBehaviour.Radius = LimitBreakNewVarsSetter.NewRadius;

            //Set new color for sprite
            SpriteGetter spriteGetter = GetComponent<SpriteGetter>();
            Color newSpriteColor = LimitBreakNewVarsSetter.NewSpriteColor;
            spriteGetter.SpriteRenderer.color = newSpriteColor;

            //Set new color for limit break vizualizer 
            SpriteRenderer vizualizerSpriteRenderer = 
                LimitBreakNewVarsSetter.LimitBreakVizualizer.GetComponent<SpriteRenderer>();
            Color newVizualizerColor = LimitBreakNewVarsSetter.NewVizualizerColor;
            vizualizerSpriteRenderer.color = newVizualizerColor;
            
            //Set new scale for limit break vizualizer
            Vector3 newNewVizualizerScale = new Vector3(
                LimitBreakNewVarsSetter.NewVizualizerScale,
                LimitBreakNewVarsSetter.NewVizualizerScale,
                LimitBreakNewVarsSetter.NewVizualizerScale);
            LimitBreakNewVarsSetter.LimitBreakVizualizer.transform.localScale = newNewVizualizerScale;

            OnTowerLimitBreak?.Invoke(transform);          
        }


    }
}

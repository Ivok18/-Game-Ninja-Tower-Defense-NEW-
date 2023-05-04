using TD.Entities.Enemies;
using TD.Entities.Towers.States;
using TD.NodeSystem;
using UnityEngine;


namespace TD.Entities.Towers
{
    public class DodgeCounterTrigger : MonoBehaviour
    {
        [SerializeField] private float interceptorAlpha;

        private void OnEnable()
        {
            EnemyAssaultDetectionBehaviour.OnEnemyDetectAssault += Intercept;
        }

        private void OnDisable()
        {
            EnemyAssaultDetectionBehaviour.OnEnemyDetectAssault -= Intercept;
        }

        private void Intercept(Transform targetedEnemy, Transform attackingTower)
        {
            if (targetedEnemy == null || targetedEnemy.CompareTag("Dead"))
                return;

            TowerStateSwitcher towerStateSwitcher = GetComponent<TowerStateSwitcher>();
            if (towerStateSwitcher == null)
                return;

            if (towerStateSwitcher.CurrentTowerState != TowerState.Attacking)
                return;

            LockTargetState lockTargetState = GetComponent<LockTargetState>();
            if (lockTargetState == null)
                return;

            if (lockTargetState.Target == null || lockTargetState.Target.CompareTag("Dead"))
                return;

            DummyDetector dummyDetector = lockTargetState.Target.GetComponent<DummyDetector>();
            if (dummyDetector == null)
                return;

            if (lockTargetState.Target != dummyDetector.transform)
                return;

            //Instantiate an after image that represents the rapid refocus of tower to find its real target
            //Instantiante interceptor 2 nodes ahead of enemy
            Node enemyNearestNode = NodeManager.Instance.GetNodeAtPosition(targetedEnemy.position);
            Node interceptorSpawnNode = NodeManager.Instance.GetNodeAtIndex(enemyNearestNode.nodeIndex + 2);
            GameObject interceptorGo = Instantiate(transform.gameObject, interceptorSpawnNode.center, Quaternion.identity);

            //Change its alpha to differentiate it from the tower
            SpriteGetter towerSpriteGetter = GetComponent<SpriteGetter>();
            Color newColor = towerSpriteGetter.SpriteRenderer.color;
            newColor.a = interceptorAlpha;
            SpriteGetter interceptorSpriteGetter = interceptorGo.GetComponent<SpriteGetter>();
            interceptorSpriteGetter.SpriteRenderer.color = newColor;

            //Set target (origin of the dummy)
            LockTargetState interceptorLockTargetState = interceptorGo.GetComponent<LockTargetState>();
            interceptorLockTargetState.Target = dummyDetector.Origin;

            //Disable dodge countee trigger script
            DodgeCounterTrigger interceptorDodgeCounterTrigger = interceptorGo.GetComponent<DodgeCounterTrigger>();
            interceptorDodgeCounterTrigger.enabled = false;


            //Disable look target script 
            //LookTargetBehaviour interceptorLookTargetBehaviour = interceptorGo.GetComponent<LookTargetBehaviour>();
            //interceptorLookTargetBehaviour.enabled = false;

            //Disable charge attack state
            ChargeAttackState interceptorChargeAttackState = interceptorGo.GetComponent<ChargeAttackState>();
            interceptorChargeAttackState.enabled = false;

            //Set charge attack bar invisible
            //..get all sprite renderers under the container of the bar and set their alpha to 0      
            ChargeAttackBarBehaviour interceptorChargeAttackBarBehaviour = interceptorGo.GetComponent<ChargeAttackBarBehaviour>();
            SpriteRenderer[] spriteRenderers = interceptorChargeAttackBarBehaviour.Container.GetComponentsInChildren<SpriteRenderer>();
            foreach(var spriteRender in spriteRenderers)
            {
                spriteRender.color = new Color(0, 0, 0, 0);
            }
            

            //Activate autodestruction after hit
            DodgeShadowCounterBehaviour interceptorAutoDestructBehaviour = interceptorGo.GetComponent<DodgeShadowCounterBehaviour>();
            

            interceptorAutoDestructBehaviour.IsActive = true;
            interceptorAutoDestructBehaviour.OriginTower = transform;
            

            

        }
    }
}

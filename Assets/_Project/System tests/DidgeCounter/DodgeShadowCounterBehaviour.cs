using System.Collections;
using System.Collections.Generic;
using TD.Entities.Enemies;
using TD.Entities.Towers.States;
using UnityEditor.U2D.Path;
using UnityEngine;


namespace TD.Entities.Towers
{
    public class DodgeShadowCounterBehaviour : MonoBehaviour
    {
        [Header("Tower where inteceptor comes from")]
        public Transform OriginTower;
        private Color originColor;

        [Header("Idle settings after successul hit")]
        [SerializeField] private float idleTime;
        public float IdleTimeRemaining;

        [Header("Max active time")]
        [SerializeField] private float maxActiveTime;

        [Header("Return speed")]
        [SerializeField] private float returnSpeed;

        [Header("Booleans")]
        private bool mustStayInIdle;
        public bool IsActive;
      

        private void OnEnable()
        {
            EnemyHitDetection.OnEnemyHit += TryAutoDestruct;
        }

        private void OnDisable()
        {
            EnemyHitDetection.OnEnemyHit -= TryAutoDestruct;
        }

        private void Start()
        {
            IdleTimeRemaining = idleTime;
        }


        private void TryAutoDestruct(Transform enemy, Transform attackingTower, Vector3 hitPosition)
        {
            if (attackingTower != transform)
                return;
            
       
            if (!IsActive)
                return;

            transform.position = hitPosition;
            mustStayInIdle = true;

            SpriteGetter originTowerSpriteGetter = OriginTower.GetComponent<SpriteGetter>();
            SpriteRenderer originTowerSprite = originTowerSpriteGetter.SpriteRenderer;
            originColor = originTowerSprite.color;
            originTowerSprite.color = Color.yellow;
           

            LookTargetBehaviour lookTargetBehaviour = GetComponent<LookTargetBehaviour>();
            lookTargetBehaviour.enabled = false;
        }

        private void FixedUpdate()
        {
            if (!IsActive)
                return;


            RadiusGetter radiusGetter = GetComponent<RadiusGetter>();
            Transform radiusTransform = radiusGetter.RadiusTransform;
            RadiusDetectionBehaviour radiusDetectionBehaviour = radiusTransform.GetComponent<RadiusDetectionBehaviour>();
            StationaryState originTowerStationaryState = OriginTower.GetComponent<StationaryState>();
            Vector3 radiusCenter = originTowerStationaryState.StartPosition;
            float radius = radiusDetectionBehaviour.Radius;
            float distanceFromOriginTower = Vector2.Distance(transform.position, radiusCenter);
            float maxDistanceFromOriginTower = radius + (0.25f * radius);
            //Debug.Log("distance from origin tower -> " + distanceFromOriginTower);
            //Debug.Log("max distance from origin tower -> " + maxDistanceFromOriginTower);
            if (distanceFromOriginTower > maxDistanceFromOriginTower)
            {
                SpriteGetter spriteGetter = GetComponent<SpriteGetter>();
                SpriteRenderer sprite = spriteGetter.SpriteRenderer;
                Color invisible = new Color();
                invisible.a = 0;
                sprite.color = originColor;
                Destroy(gameObject);
              
            }

            if(maxActiveTime > 0)
            {     
                if(!mustStayInIdle)
                {
                    maxActiveTime -= Time.fixedDeltaTime;
                } 
            }
            else
            {
                Destroy(gameObject);
            }

            
            if (!mustStayInIdle)
                return;

            if (IdleTimeRemaining > 0)
            {
                IdleTimeRemaining -= Time.deltaTime;
            }
            else
            {
                //Go back to origin
                if (Vector2.Distance(transform.position, OriginTower.position) > 0.1f)
                {
                    transform.position = Vector2.MoveTowards(transform.position, OriginTower.position, Time.fixedDeltaTime * returnSpeed);
                }
                else //Destroy object once it has gone back to origin tower
                {
                    SpriteGetter originTowerSpriteGetter = OriginTower.GetComponent<SpriteGetter>();
                    SpriteRenderer originTowerSprite = originTowerSpriteGetter.SpriteRenderer;
                    originTowerSprite.color = originColor;
                    Destroy(gameObject);
                }
            }


          
          
        }

    }
}

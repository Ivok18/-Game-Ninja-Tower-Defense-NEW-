using UnityEngine;

namespace TD.Entities
{
    public class SpriteGetter : MonoBehaviour
    {
        [SerializeField] private GameObject spriteGameObj;
        
        public SpriteRenderer SpriteRenderer
        {
            get => spriteGameObj.GetComponent<SpriteRenderer>();
        }
    }
}
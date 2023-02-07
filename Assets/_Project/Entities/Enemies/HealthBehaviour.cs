using UnityEngine;

namespace TD.Entities.Enemies
{
    public class HealthBehaviour : MonoBehaviour
    {
        public int CurrentHealth;
        public int MaxHealth;
    
        private void Start()
        {
            CurrentHealth = MaxHealth;
        }
      
    }
}


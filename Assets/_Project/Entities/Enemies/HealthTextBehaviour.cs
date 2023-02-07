using UnityEngine;
using TMPro;

namespace TD.Entities.Enemies
{
    public class HealthTextBehaviour : MonoBehaviour
    {
        [SerializeField] private HealthBehaviour health;
        private TextMeshProUGUI textMesh;

        private void Awake()
        {
            textMesh = GetComponent<TextMeshProUGUI>();

        }
        void Update()
        {
            textMesh.text = health.CurrentHealth.ToString();
        }
    }

}

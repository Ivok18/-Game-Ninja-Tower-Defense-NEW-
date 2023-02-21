using System.Collections;
using System.Collections.Generic;
using TD.ElementSystem;
using UI;
using UnityEngine;

namespace TD.Entities.Towers
{
    public class CatalystSelector : MonoBehaviour
    {
        private Dictionary<TowerElement, Transform> catalystsDictionary;
        public Transform TowerHolder;

        [Header("Potential visualizers")]
        [SerializeField] private Transform fireCatalyst;
        [SerializeField] private Transform earthCatalyst;
        [SerializeField] private Transform windCatalyst;

        private void OnEnable()
        {
            UICatalystButtonBehaviour.OnCatalyseButtonPressed += TrySelectCatalyst;
        }

        private void OnDisable()
        {
            UICatalystButtonBehaviour.OnCatalyseButtonPressed -= TrySelectCatalyst;
        }

        private void Start()
        {
            catalystsDictionary = new Dictionary<TowerElement, Transform>();

            catalystsDictionary.Add(TowerElement.Fire, fireCatalyst);
            catalystsDictionary.Add(TowerElement.Earth, earthCatalyst);
            catalystsDictionary.Add(TowerElement.Wind, windCatalyst);
        }

        public void TrySelectCatalyst(Transform targetTower, TowerElement elementOfCaralyst)
        {
            if (targetTower != TowerHolder)
                return;

            Transform catalystType = catalystsDictionary[elementOfCaralyst];
            catalystType.gameObject.SetActive(true);

        }
    }
}
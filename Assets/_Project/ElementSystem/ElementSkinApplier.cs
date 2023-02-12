using JetBrains.Annotations;
using System.Collections.Generic;
using TD.Entities;
using TD.Entities.Towers;
using UnityEngine;


namespace TD.ElementSystem
{
    public class ElementSkinApplier : MonoBehaviour
    {
        [SerializeField] private Sprite fire;
        [SerializeField] private Sprite earth;
        [SerializeField] private Sprite wind;
        [SerializeField] private Sprite firePlusEarth;
        [SerializeField] private Sprite firePlusWind;
        [SerializeField] private Sprite earthPlusWind;
        [SerializeField] private Sprite firePlusEarthPlusWind;

        private void OnEnable()
        {
            ElementDataApplier.OnElementDataAppliedOnTower += UpdateSkin;
        }

        private void OnDisable()
        {
            ElementDataApplier.OnElementDataAppliedOnTower -= UpdateSkin;
        }

        private void UpdateSkin(Transform tower, TowerElement element, int elementCost)
        {
            ElementsTracker elementsTracker = tower.GetComponent<ElementsTracker>();
            SpriteRenderer spriteRenderer = tower.GetComponent<SpriteGetter>().SpriteRenderer;

            if(
                    FindFire(elementsTracker.CurrTowerElements)  
                && !FindEarth(elementsTracker.CurrTowerElements)
                && !FindWind(elementsTracker.CurrTowerElements)
              )
            {
                spriteRenderer.sprite = fire;
            }

            else if(
                  !FindFire(elementsTracker.CurrTowerElements)
               &&  FindEarth(elementsTracker.CurrTowerElements)
               && !FindWind(elementsTracker.CurrTowerElements)
                )
            {
                spriteRenderer.sprite = earth;
            }

            else if (
                   !FindFire(elementsTracker.CurrTowerElements)
                && !FindEarth(elementsTracker.CurrTowerElements)
                &&  FindWind(elementsTracker.CurrTowerElements)
                )
            {
                spriteRenderer.sprite = wind;
            }

            else if (
                   FindFire(elementsTracker.CurrTowerElements)
               &&  FindEarth(elementsTracker.CurrTowerElements)
               && !FindWind(elementsTracker.CurrTowerElements)
                )
            {
                spriteRenderer.sprite = firePlusEarth;
            }

            else if (
                   FindFire(elementsTracker.CurrTowerElements)
               && !FindEarth(elementsTracker.CurrTowerElements)
               &&  FindWind(elementsTracker.CurrTowerElements)
                )
            {
                spriteRenderer.sprite = firePlusWind;
            }

            else if (
                  !FindFire(elementsTracker.CurrTowerElements)
               &&  FindEarth(elementsTracker.CurrTowerElements)
               &&  FindWind(elementsTracker.CurrTowerElements)
                )
            {
                spriteRenderer.sprite = earthPlusWind;
            }

            else if (
                   FindFire(elementsTracker.CurrTowerElements)
               &&  FindEarth(elementsTracker.CurrTowerElements)
               &&  FindWind(elementsTracker.CurrTowerElements)
                )
            {
                spriteRenderer.sprite = firePlusEarthPlusWind;
            }

        }
        private bool FindFire(List<TowerElement> towerCurrElements)
        {
            foreach(var element in towerCurrElements)
            {
                if(element == TowerElement.Fire)
                {
                    return true;
                }
            }

            return false;
        }
        private bool FindEarth(List<TowerElement> towerCurrElements)
        {
            foreach (var element in towerCurrElements)
            {
                if (element == TowerElement.Earth)
                {
                    return true;
                }
            }

            return false;
        }
        private bool FindWind(List<TowerElement> towerCurrElements)
        {
            foreach (var element in towerCurrElements)
            {
                if (element == TowerElement.Wind)
                {
                    return true;
                }
            }

            return false;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TD.ElementSystem
{
    public interface IElementBoost
    {
        public void Boost(ElementScriptableObject elementData);
        public void RemoveBoost(TowerElement element);
    }
}

using System.Collections;
using System.Collections.Generic;
using TD.ElementSystem;
using UnityEngine;

[CreateAssetMenu(fileName = "Element", menuName = "MyGame/Element")]
public class ElementScriptableObject : ScriptableObject
{
    [SerializeField] private TowerElement element;
    [SerializeField] private int cost;

    public TowerElement Element => element;
    public int Cost => cost;
   
}

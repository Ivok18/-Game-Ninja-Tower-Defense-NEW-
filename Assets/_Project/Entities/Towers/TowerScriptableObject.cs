using TD.Entities.Towers;
using UnityEngine;

[CreateAssetMenu(fileName = "Tower", menuName = "MyGame/Tower")]
public class TowerScriptableObject : ScriptableObject
{
    [SerializeField] private TowerState startState;
    [SerializeField] private TowerType towerType;

    [Header("Attack rate")]
    [SerializeField] private float baseAttackRate;
    [SerializeField] private float elementBonusAttackRate;

    [Header("Damage per dash")]
    [SerializeField] private int baseDamagePerDash;
    [SerializeField] private int elementBonusDamagePerDash;

    [Header("Radius")]
    [SerializeField] private float radius;

    [Header("Dash speed")]
    [SerializeField] private float baseDashSpeed;
    [SerializeField] private float elementBonusDashSpeed;

    [Header("Dash number")]
    [SerializeField] private int noOfBonusDash;

    public TowerState StartState => startState;
    public TowerType TowerType => towerType;

    public float BaseAttackRate => baseAttackRate;
    public float ElementBonusAttackRate => elementBonusAttackRate;


    public int BaseDamagePerDash => baseDamagePerDash;
    public int ElementBonusDamagePerDash => elementBonusDamagePerDash;

    public float Radius => radius;

    public float BaseDashSpeed => baseDashSpeed;
    public float ElementBonusDashSpeed => elementBonusDashSpeed;

    public int NoOfBonusDash => noOfBonusDash;
}

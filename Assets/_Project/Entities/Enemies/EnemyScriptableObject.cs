using UnityEngine;

[CreateAssetMenu(fileName = "Enemy",menuName = "MyGame/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    [SerializeField] private float speed;
    [SerializeField] private int maxHealth;
    [SerializeField] private float reward;

    public float Speed => speed;
    public int MaxHealth => maxHealth;

    public float Reward => reward;
}

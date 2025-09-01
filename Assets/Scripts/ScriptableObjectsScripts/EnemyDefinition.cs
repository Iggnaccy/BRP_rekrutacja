using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyDefinition", menuName = "ScriptableObjects/EnemyDefinition", order = 1)]
public class EnemyDefinition : ScriptableObject
{
    public Sprite Sprite;
    public int Score;
    public DamageType Weakness;
}

using UnityEngine;

[CreateAssetMenu(fileName = "NewMonsterData", menuName = "Countdown/Monster Data")]
public class MonsterData : ScriptableObject
{
    [Header("Display Info")]
    public string monsterName = "Goblin";
    public GameObject monsterObject; // Or GameObject prefab if using 3D models

    [Header("Combat Stats")]
    public int maxHP = 50;
    public int attackDamage = 8;
    public float movementSpeed = 10f;

    [Header("Rewards")]
    public int goldReward = 15;
    public int expReward = 10;
}
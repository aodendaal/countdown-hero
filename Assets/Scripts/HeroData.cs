using UnityEngine;

[CreateAssetMenu(fileName = "NewHeroData", menuName = "Countdown/Hero Data")]
public class HeroData : ScriptableObject
{
    [Header("Combat Stats")]
    public int maxHP = 50;
    public int attackDamage = 8;
}
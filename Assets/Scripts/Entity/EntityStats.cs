using UnityEngine;

/// <summary>
/// 공통 스탯.
/// </summary>
[System.Serializable]
public class EntityStats
{
    [Header("Level & Exp")]
    public int level;                   // 레벨.
    public float exp;                   // 경험치.

    [Header("Attack")]
    public float attackDamage;          // 공격력.
    public float attackSpeed;           // 공격 속도.
    public float criticalChance;        // 크리티컬 확률.
    public float criticalDamage;        // 크리티컬 데미지.

    [Header("Defense")]
    public float currentHP;             // 현재 체력.
    public float maxHP;                 // 최대 체력.
    public float healthRegeneration;    // 체력 재생.
    public float evasion;               // 회피율.

    [Header("Movement")]
    public float moveSpeed;             // 이동 속도.
}

/// <summary>
/// 플레이어 전용 스탯.
/// </summary>
[System.Serializable]
public class PlayerStats : EntityStats
{
    [Header("Player Only")]
    public float dashCooldown;          // 대시 쿨타임.
}

/// <summary>
/// 몬스터 전용 스탯.
/// </summary>
[System.Serializable]
public class EnemyStats : EntityStats
{
    [Header("Monster Only")]
    public string attackType;           // 공격 유형 (좀비: Melee, 스켈레톤: Ranged).
    public float hpStep;                // 레벨업 시 가산될 체력 수치 (좀비: 50, 스켈레톤: 25).
    public float damageStep;            // 레벨업 시 가산될 공격력 수치 (좀비: 5, 스켈레톤: 10).
}

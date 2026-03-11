using System.Linq;
using UnityEngine;

[System.Serializable]
public class EntityStats
{
    // 현재 스탯.
    [Header("Current Stats")]
    [SerializeField] private Stat currentExp;
    [SerializeField] private Stat currentHP;

    // 모든 스탯을 담는 배열.
    [Header("Stats")]
    [SerializeField] private Stat[] stats;

    // 외부에서 현재 경험치 정보에 접근하기 위한 프로퍼티.
    public Stat CurrentExp => currentExp;
    // 외부에서 현재 체력 정보에 접근하기 위한 프로퍼티.
    public Stat CurrentHP => currentHP;
    // Stat 객체를 직접 넘겨서 같은 타입의 스탯을 찾는 함수.
    public Stat GetStat(Stat stat) => stats.FirstOrDefault(s => s.StatType == stat.StatType);
    // StatType을 넘겨서 해당 스탯을 찾아오는 함수.
    public Stat GetStat(StatType statType) => stats.FirstOrDefault(s => s.StatType == statType);
}

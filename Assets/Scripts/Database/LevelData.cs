using UnityEngine;

[CreateAssetMenu]
public class LevelData : ScriptableObject
{
    // 플레이어가 도달할 수 있는 최종 레벨.
    [SerializeField] private int maxLevel;
    // 각 레벨업에 필요한 경험치 요구량 배열.
    [SerializeField] private float[] maxExp;

    public int MaxLevel => maxLevel;
    public float[] MaxExp => maxExp;
}

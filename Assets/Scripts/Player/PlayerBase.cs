using UnityEngine;

public class PlayerBase : EntityBase
{
    // 플레이어 전용 스탯 데이터.
    [SerializeField] private PlayerStats stats;

    // 부모(EntityBase)가 정의한 추상 프로퍼티 구현.
    public override EntityStats Stats => stats;
    
    // 현재 플레이어의 이동 상태.
    public bool IsMoved { get; set; } = false;

    private void Awake()
    {
        // 플레이어는 몬스터와 달리 별도의 레벨업 공식을 적용하지 않고,
        // 인스펙터에 설정된 초기 스탯을 기반으로 부모의 Setup을 호출하여 초기화함.
        base.Setup();
    }
}

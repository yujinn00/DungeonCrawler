using UnityEngine;

public class EnemyBase : EntityBase
{
    // UI가 몬스터의 어느 부위를 따라다닐지 지정하는 위치.
    [SerializeField] private Transform hudPoint;
    // 생성할 HP UI 프리팹.
    [SerializeField] private GameObject uiPrefab;
    
    private void Awake()
    {
        // 몬스터는 플레이어와 달리 종류마다 기본 스탯과 레벨별 성장 공식이 다르므로,
        // 오버라이드된 Setup을 호출하여 각 몬스터에 맞는 수치를 먼저 계산한 뒤 초기화함.
        Setup();
    }

    protected override void Setup()
    {
        // (레벨당 스탯 증가량 x 성장 횟수)를 계산하여 추가 스탯에 덮어씌움 (순수 기본 스탯은 건들지 않음). 
        Stats.GetStat(StatType.HealthPoint).BonusValue = Stats.GetStat(StatType.HpStep).Value * (Stats.level - 1);
        Stats.GetStat(StatType.AttackDamage).BonusValue = Stats.GetStat(StatType.DamageStep).Value * (Stats.level - 1);
        
        // 최종 계산된 스탯을 반영하기 위해 부모의 Setup 호출.
        base.Setup();
    }

    /// <summary>
    /// 몬스터가 스폰될 때 외부에서 호출하는 초기화 함수.
    /// </summary>
    /// <param name="enemySpawner">자신을 생성한 몬스터 스포너</param>
    /// <param name="parent">UI가 소속될 부모 트랜스폼</param>
    public void Initialize(EnemySpawner enemySpawner, Transform parent)
    {
        // UI 프리팹을 지정된 캔버스의 자식으로 생성.
        GameObject clone = Instantiate(uiPrefab, parent);
        
        // 부모 설정 시 스케일이 변형되는 것을 방지하기 위해 초기화.
        clone.transform.localScale = Vector3.one;
        // UI가 따라다닐 위치 설정.
        clone.GetComponent<FollowTargetUI>().Setup(hudPoint);
        // 체력바 갱신을 위해 현재 몬스터의 스탯 정보 연결.
        clone.GetComponentInChildren<UIHP>().Setup(this);
    }
}

using UnityEngine;
using Unity.Cinemachine;

public class PlayerBase : EntityBase
{
    // 게임의 전반적인 상태를 제어하는 컨트롤러.
    [SerializeField] private GameController gameController;
    // 현재 타겟의 위치를 실시간으로 표시하는 조준점 오브젝트.
    [SerializeField] private FollowTarget targetMark;
    // 인게임 레벨별 경험치 테이블 정보.
    [SerializeField] private LevelData levelData;
    // 스킬 습득 및 레벨업 팝업 로직을 제어하는 시스템.
    [SerializeField] private SkillSystem skillSystem;
    
    // 카메라 흔들림을 연출하기 위한 컴포넌트.
    private CinemachineImpulseSource impulseSource;
    // 매 프레임 흡수하는 경험치 양.
    private float expAmount = 2f;
    // 현재 플레이어의 이동 상태.
    public bool IsMoved { get; set; } = false;
    // 적을 죽이고 축적된 경험치.
    public float AccumulationExp { get; set; } = 0f;

    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        
        // 플레이어는 몬스터와 달리 별도의 레벨업 공식을 적용하지 않고,
        // 인스펙터에 설정된 초기 스탯을 기반으로 부모의 Setup을 호출하여 초기화함.
        base.Setup();
        
        // 현재 보유한 경험치를 0으로 초기화.
        Stats.CurrentExp.DefaultValue = 0f;
        // 경험치가 변할 때마다 레벨업 여부를 체크하도록 이벤트 함수 구독.
        Stats.CurrentExp.OnValueChanged += IsLevelUp;
        // 다음 레벨업에 필요한 총 경험치량을 레벨 데이터의 첫 번째 값으로 설정.
        Stats.GetStat(StatType.Exp).DefaultValue = levelData.MaxExp[0];
    }

    private void Update()
    {
        if (Target == null)
        {
            targetMark.gameObject.SetActive(false);
        }

        SearchTarget();
        Recovery();
        UpdateExp();
    }

    /// <summary>
    /// 플레이어가 사망했을 때 실행되는 메소드.
    /// </summary>
    protected override void OnDie()
    {
        gameController.GameOver();
    }

    /// <summary>
    /// 가장 가까운 적을 찾아 타겟으로 설정하는 메소드.
    /// </summary>
    private void SearchTarget()
    {
        // 가장 가까운 거리를 찾기 위해, 초기값을 무한대로 설정.
        float closestDistance = Mathf.Infinity;

        // 스포너에 등록된 모든 몬스터들을 하나씩 검사.
        foreach (var entity in EnemySpawner.Enemies)
        {
            // 내 위치와 몬스터 위치 사이의 거리를 계산.
            // sqrMagnitude: 루트 연산을 뺀 제곱 거리라 성능이 좋음.
            float distance = (entity.transform.position - transform.position).sqrMagnitude;
            
            
            if (distance < closestDistance)
            {
                // 최단 거리 갱신.
                closestDistance = distance;
                // 현재 몬스터를 타겟으로 설정.
                Target = entity.GetComponent<EntityBase>();
            }
        }

        if (Target != null)
        {
            // 타겟 마크가 적을 따라다니게 설정.
            targetMark.SetTarget(Target.transform);
            // 타겟 마크 위치 즉시 이동.
            targetMark.transform.position = Target.transform.position;
            // 타겟 마크 활성화.
            targetMark.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 시간이 지남에 따라 체력을 자동으로 회복하는 메소드.
    /// </summary>
    private void Recovery()
    {
        // 현재 체력이 최대 체력보다 적을 때만 회복.
        if (Stats.CurrentHP.DefaultValue < Stats.GetStat(StatType.HealthPoint).Value)
        {
            Stats.CurrentHP.DefaultValue += Time.deltaTime * Stats.GetStat(StatType.HealthRegen).Value;
        }
        // 체력이 꽉 찼거나 넘치면, 최대 체력으로 고정.
        else
        {
            Stats.CurrentHP.DefaultValue = Stats.GetStat(StatType.HealthPoint).Value;
        }
    }

    /// <summary>
    /// 축적된 경험치를 일정량씩 소모하여 실제 플레이어 경험치로 반영하는 메소드.
    /// </summary>
    private void UpdateExp()
    {
        // 소모할 경험치가 없거나, 스킬 선택 창이 활성화된 상태라면 중단.
        if (Mathf.Approximately(AccumulationExp, 0f) || skillSystem.IsSelectSkill == true)
        {
            return;
        }
        
        // 프레임당 최대 흡수량만큼 경험치를 축적.
        float getExp = AccumulationExp > expAmount ? expAmount : AccumulationExp;
        // 축적된 경험치에서 getExp만큼 차감.
        AccumulationExp -= getExp;
        // 플레이어 현재 경험치를 getExp만큼 증가.
        Stats.CurrentExp.DefaultValue += getExp;
    }

    /// <summary>
    /// 경험치 변경 시 호출되어 레벨업 조건을 체크하고 실제 레벨업 처리를 수행하는 메소드.
    /// </summary>
    /// <param name="stat">상태가 변경된 스탯 정보</param>
    /// <param name="prev">변경 전 수치</param>
    /// <param name="cur">변경 후 수치</param>
    private void IsLevelUp(Stat stat, float prev, float cur)
    {
        // 현재 경험치가 목표 경험치에 도달하지 않았다면 중단.
        if (!Mathf.Approximately(Stats.CurrentExp.Value, Stats.GetStat(StatType.Exp).Value))
        {
            return;
        }

        // 플레이어 레벨 수치 증가.
        Stats.GetStat(StatType.Level).DefaultValue++;

        // 현재 경험치 초기화 (목표 수치만큼 차감하여 초과분은 유지).
        Stats.CurrentExp.DefaultValue -= Stats.GetStat(StatType.Exp).Value;

        // 다음 레벨의 목표 경험치량 설정.
        // 레벨 데이터 배열 범위 내라면 다음 레벨 수치를 적용하고, 만렙 이후라면 만렙 수치를 쭉 적용.
        if (Stats.GetStat(StatType.Level).Value < levelData.MaxExp.Length)
        {
            Stats.GetStat(StatType.Exp).DefaultValue = levelData.MaxExp[(int)Stats.GetStat(StatType.Level).Value - 1];
        }
        else
        {
            Stats.GetStat(StatType.Exp).DefaultValue = levelData.MaxExp[levelData.MaxExp.Length - 1];
        }
        
        // 레벨업 시 스킬을 선택할 수 있도록 스킬 선택 팝업 출력.
        skillSystem.StartSelectSkill();
    }
    
    /// <summary>
    /// 플레이어가 데미지를 입었을 때 카메라 흔들림 연출을 추가하는 메소드.
    /// </summary>
    /// <param name="damage">입은 데미지</param>
    public override void TakeDamage(float damage)
    {
        // 부모 클래스의 기본 데미지 처리 실행.
        base.TakeDamage(damage);

        // 데미지를 입으면 카메라 흔들림 발생.
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse(); 
        }
    }
}

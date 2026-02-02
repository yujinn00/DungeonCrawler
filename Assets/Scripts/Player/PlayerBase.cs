using UnityEngine;

public class PlayerBase : EntityBase
{
    // 현재 타겟의 위치를 실시간으로 표시하는 조준점 오브젝트.
    [SerializeField] private FollowTarget targetMark;

    // 현재 플레이어의 이동 상태.
    public bool IsMoved { get; set; } = false;

    private void Awake()
    {
        // 플레이어는 몬스터와 달리 별도의 레벨업 공식을 적용하지 않고,
        // 인스펙터에 설정된 초기 스탯을 기반으로 부모의 Setup을 호출하여 초기화함.
        base.Setup();
    }

    private void Update()
    {
        if (Target == null)
        {
            targetMark.gameObject.SetActive(false);
        }

        SearchTarget();
        Recovery();
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
}

using UnityEngine;

public class PlayerBase : EntityBase
{
    // 현재 타겟의 위치를 실시간으로 표시하는 조준점 오브젝트.
    [SerializeField] private FollowTarget targetMark;
    
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

    private void Update()
    {
        if (Target == null)
        {
            targetMark.gameObject.SetActive(false);
        }

        SearchTarget();
    }

    private void SearchTarget()
    {
        float closestDistance = Mathf.Infinity;

        foreach (var entity in EnemySpawner.Enemies)
        {
            // 제일 가까운 적을 찾기 때문에 sqrMagnitude를 사용함.
            float distance = (entity.transform.position - transform.position).sqrMagnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                Target = entity.GetComponent<EntityBase>();
            }
        }

        if (Target != null)
        {
            targetMark.SetTarget(Target.transform);
            targetMark.transform.position = Target.transform.position;
            targetMark.gameObject.SetActive(true);
        }
    }
}

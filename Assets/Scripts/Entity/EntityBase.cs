using UnityEngine;

public abstract class EntityBase : MonoBehaviour
{
    // 세부 스탯을 관리하는 데이터 컨테이너.
    [SerializeField] private EntityStats stats;
    // 투사체가 날아갈 때 목표가 되는 지점.
    [SerializeField] private Transform middlePoint;
    
    // 외부에서 스탯 정보를 읽을 수 있게 제공하는 프로퍼티 (Get).
    // 실제 데이터는 자식들이 가지고 있고, 부모는 껍데기만 빌려 씀.
    public EntityStats Stats => stats;
    // 현재 체력이 0 이하면 사망 상태로 판단.
    public bool IsDead => Stats.CurrentHP != null && Mathf.Approximately(Stats.CurrentHP.DefaultValue, 0f);
    // 외부에서 중앙 위치를 가져올 때 쓰는 프로퍼티.
    public Vector3 MiddlePoint => middlePoint != null ? middlePoint.position : Vector3.zero;
    // 현재 이 엔티티가 공격하려고 주시하는 대상.
    public EntityBase Target { get; set; }

    protected virtual void Setup()
    {
        // 게임 시작 시, 최대 체력으로 현재 체력 초기화.
        Stats.CurrentHP.DefaultValue = Stats.GetStat(StatType.HealthPoint).Value;
    }

    /// <summary>
    /// 데미지를 입었을 때 호출되는 함수.
    /// </summary>
    /// <param name="damage">적용할 데미지 수치</param>
    public virtual void TakeDamage(float damage)
    {
        if (IsDead)
        {
            return;
        }

        // 회피 확률 로직: 0~100 사이 랜덤값이 스탯의 회피율보다 작으면 공격 무시.
        if (Random.Range(0f, 100f) < Stats.GetStat(StatType.Evasion).Value)
        {
            Debug.Log($"{gameObject.name}이(가) 공격을 회피했습니다.");
            return;
        }

        // 현재 체력에서 받은 데미지를 차감.
        Stats.CurrentHP.DefaultValue -= damage;
        Debug.Log($"{gameObject.name}이(가) {damage}의 데미지를 입었습니다.");
        
        // 체력이 0 이하가 되면 0으로 고정하고, 사망 처리 함수 호출.
        if (Mathf.Approximately(Stats.CurrentHP.DefaultValue, 0f))
        {
            Stats.CurrentHP.DefaultValue = 0;
            OnDie();
        }
    }

    /// <summary>
    /// 게임 오브젝트가 사망했을 때 실행되는 함수.
    /// </summary>
    protected virtual void OnDie()
    {
        // Todo: 추후 사망 로직 추가할 예정.
        Debug.Log($"{gameObject.name}이(가) 사망했습니다.");
    }
}

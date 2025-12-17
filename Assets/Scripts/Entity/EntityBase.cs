using UnityEngine;

public class EntityBase : MonoBehaviour
{
    [SerializeField] protected EntityStats stats;

    // 외부에서 스탯 정보를 읽을 수 있게 제공하는 프로퍼티 (Get).
    public EntityStats Stats => stats;
    // 현재 체력이 0 이하면 사망 상태로 판단.
    public bool IsDead => stats.currentHP <= 0;

    protected virtual void Setup()
    {
        // 게임 시작 시, 최대 체력으로 현재 체력 초기화.
        stats.currentHP = stats.maxHP;
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
        if (Random.Range(0f, 100f) < stats.evasion)
        {
            Debug.Log($"{gameObject.name}이(가) 공격을 회피했습니다.");
            return;
        }

        // 현재 체력에서 받은 데미지를 차감.
        stats.currentHP -= damage;
        Debug.Log($"{gameObject.name}이(가) {damage}의 데미지를 입었습니다.");
        
        // 체력이 0 이하가 되면 0으로 고정하고, 사망 처리 함수 호출.
        if (stats.currentHP <= 0)
        {
            stats.currentHP = 0;
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

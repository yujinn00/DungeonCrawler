using UnityEngine;

[System.Serializable]
public class Stat
{
    // 값이 변경될 때 알림을 보낼 델리게이트.
    public delegate void ValueChangedHandler(Stat stat, float prev, float cur);
    
    // 값이 조금이라도 변했을 때, 발생하는 이벤트.
    public event ValueChangedHandler OnValueChanged;
    // 값이 최대 한계치에 도달했을 때, 발생하는 이벤트.
    public event ValueChangedHandler OnValueMax;
    // 값이 최소 한계치에 도달했을 때, 발생하는 이벤트.
    public event ValueChangedHandler OnValueMin;

    // 스탯의 종류.
    [SerializeField] private StatType statType;
    // 값의 최대 한계치.
    [SerializeField] private float maxValue;
    // 값의 최소 한계치.
    [SerializeField] private float minValue;
    // 순수 기본 스탯.
    [SerializeField] private float defaultValue;
    // 추가 스탯.
    [SerializeField] private float bonusValue;

    // 스탯 종류를 외부에서 읽기 위한 프로퍼티.
    public StatType StatType => statType;
    // 기본값과 추가값을 더한 뒤, 최소/최대 범위로 제한하여 최종 능력치로 산정.
    public float Value => Mathf.Clamp(defaultValue + bonusValue, minValue, maxValue);

    // 기본 스탯을 수정할 때 사용하는 프로퍼티.
    public float DefaultValue
    {
        get => defaultValue;
        set
        {
            float prev = Value;
            defaultValue = Mathf.Clamp(value, minValue, maxValue);
            TryInvokeValueChangedEvent(prev, Value);
        }
    }
    
    // 추가 스탯을 수정할 때 사용하는 프로퍼티.
    public float BonusValue
    {
        get => bonusValue;
        set => bonusValue = value;
    }

    /// <summary>
    /// 이전 값과 현재 값을 비교하여 변화가 있을 때만 이벤트를 호출하는 내부 메소드.
    /// </summary>
    /// <param name="prev">변경되기 전의 기존 값</param>
    /// <param name="cur">변경된 후의 현재 값</param>
    private void TryInvokeValueChangedEvent(float prev, float cur)
    {
        // float 연산 오차를 고려하여 값이 실질적으로 다른지 확인.
        if (!Mathf.Approximately(prev, cur))
        {
            // 값이 바뀌었으므로 구독자들에게 알림.
            OnValueChanged?.Invoke(this, prev, cur);

            if (Mathf.Approximately(cur, maxValue))
            {
                // 현재 값이 최댓값과 같은지 확인.
                OnValueMax?.Invoke(this, prev, maxValue);
            }
            else if (Mathf.Approximately(cur, minValue))
            {
                // 현재 값이 최솟값과 같은지 확인.
                OnValueMin?.Invoke(this, prev, minValue);
            }
        }
    }
}

// 스탯 종류 정의.
public enum StatType
{
    // 공통 스탯.
    AttackDamage = 0, AttackSpeed, CriticalChance, CriticalDamage, HealthPoint, Evasion,
    // 플레이어 전용 스탯.
    HealthRegen, DashCooldown, ProjectileCount, PierceCount,
    // 몬스터 전용 스탯.
    HpStep, DamageStep
}

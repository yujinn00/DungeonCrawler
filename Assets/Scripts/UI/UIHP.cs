using UnityEngine;
using UnityEngine.UI;

public class UIHP : MonoBehaviour
{
    // 체력바의 채움 효과를 보여줄 UI 이미지.
    [SerializeField] private Image image;
    // 체력 데이터를 가지고 있는 대상.
    [SerializeField] private EntityBase entity;

    private void Awake()
    {
        if (entity != null)
        {
            // 인스펙터에 미리 할당된 대상(플레이어)이 있다면, 게임 시작 시 바로 구독.
            entity.Stats.CurrentHP.OnValueChanged += UpdateHP;
        }
    }
    
    public void Setup(EntityBase entity)
    {
        this.entity = entity;
        
        // 게임 도중 생성된 대상(몬스터)을 코드로 받아와서, 나중에 구독.
        this.entity.Stats.CurrentHP.OnValueChanged += UpdateHP;
    }

    /// <summary>
    /// 체력 값이 변경될 때마다 자동으로 호출되는 이벤트 핸들러.
    /// </summary>
    /// <param name="stat">변경된 스탯 객체</param>
    /// <param name="prev">변경 전 값</param>
    /// <param name="cur">변경 후 값</param>
    private void UpdateHP(Stat stat, float prev, float cur)
    {
        // UI 갱신 공식: [현재 체력 / 최대 체력] = 0.0 ~ 1.0 사이의 비율.
        image.fillAmount = entity.Stats.CurrentHP.Value / entity.Stats.GetStat(StatType.HealthPoint).Value;
    }
}

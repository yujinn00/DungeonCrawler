using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPlayerData : MonoBehaviour
{
    // 레벨 숫자를 표시할 텍스트.
    [SerializeField] private TextMeshProUGUI textLevel;
    // 경험치 게이지 이미지.
    [SerializeField] private Image fillGaugeExp;
    // 데이터를 가져올 플레이어 참조.
    [SerializeField] private PlayerBase owner;

    private void Awake()
    {
        // 경험치 값이 변경될 때마다 UI를 갱신하도록 이벤트 함수 구독.
        owner.Stats.CurrentExp.OnValueChanged += UpdateExp;
    }

    /// <summary>
    /// 실제 UI 요소를 현재 데이터에 맞게 갱신하는 함수.
    /// </summary>
    /// <param name="stat">상태가 변경된 스탯 정보</param>
    /// <param name="prev">변경 전 수치</param>
    /// <param name="cur">변경 후 수치</param>
    private void UpdateExp(Stat stat, float prev, float cur)
    {
        // 레벨 텍스트 업데이트.
        textLevel.text = $"Lv.{owner.Stats.GetStat(StatType.Level).Value}";
        // 경험치 슬라이더 업데이트.
        fillGaugeExp.fillAmount = owner.Stats.CurrentExp.Value / owner.Stats.GetStat(StatType.Exp).Value;
    }
}

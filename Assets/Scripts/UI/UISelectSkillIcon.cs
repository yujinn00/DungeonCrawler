using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UISelectSkillIcon : MonoBehaviour, IPointerClickHandler
{
    // 스킬의 대표 아이콘 이미지.
    [SerializeField] private Image skillIcon;
    // 스킬의 이름 텍스트.
    [SerializeField] private TextMeshProUGUI skillName;
    // 스킬의 상세 설명 텍스트.
    [SerializeField] private TextMeshProUGUI skillDescription;
    
    // 스킬 정보 처리를 위한 시스템 참조.
    private SkillSystem skillSystem;
    // 현재 이 아이콘이 들고 있는 실제 스킬 데이터.
    private SkillBase skillBase;

    public void Setup(SkillSystem skillSystem, SkillBase skillBase)
    {
        this.skillSystem = skillSystem;
        this.skillBase = skillBase;
        
        // 스킬 데이터에서 아이콘, 이름, 설명을 가져와 UI에 적용.
        skillIcon.sprite = skillBase.EnableIcon;
        skillName.text = skillBase.SkillName;
        skillDescription.text = skillBase.Description;
    }

    /// <summary>
    /// 스킬 아이콘을 마우스로 클릭했을 때 호출되는 이벤트 메소드.
    /// </summary>
    /// <param name="eventData">클릭 위치 및 버튼 정보 등 포인트 관련 이벤트 데이터</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        skillSystem.EndSelectSkill(skillBase);
    }
}

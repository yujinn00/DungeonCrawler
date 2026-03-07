using UnityEngine;

public class UISelectSkill : MonoBehaviour
{
    // 스킬 선택 화면 전체를 담고 있는 패널 오브젝트.
    [SerializeField] private GameObject selectSkillPanel;
    // 선택지에 나타날 3개의 스킬 아이콘 UI 배열.
    [SerializeField] private UISelectSkillIcon[] skillIcons;

    /// <summary>
    /// 스킬 선택 창을 열고, 랜덤으로 선정된 스킬 데이터를 각 아이콘 UI에 전달 및 초기화하는 메소드.
    /// </summary>
    /// <param name="system">레벨업 처리를 담당하는 스킬 시스템 참조</param>
    /// <param name="skills">선택지로 등장할 스킬 데이터 배열</param>
    public void StartSelectSkillUI(SkillSystem system, SkillBase[] skills)
    {
        // 선택 패널 활성화.
        selectSkillPanel.SetActive(true);

        // 전달받은 스킬 개수만큼 루프를 돌며 각 아이콘 UI 세팅.
        for (int i = 0; i < skills.Length; ++i)
        {
            skillIcons[i].Setup(system, skills[i]);
        }
    }

    /// <summary>
    /// 스킬 선택이 완료되었을 때 선택 창을 닫는 메소드.
    /// </summary>
    public void EndSelectSkillUI()
    {
        selectSkillPanel.SetActive(false);
    }
}

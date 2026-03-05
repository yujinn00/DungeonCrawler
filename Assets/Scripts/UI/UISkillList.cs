using System.Collections.Generic;
using UnityEngine;

public class UISkillList : MonoBehaviour
{
    // 스킬 아이콘을 나타내는 프리팹.
    [SerializeField] private UISkillIcon skillIconPrefab;
    // 속성별로 아이콘이 배치될 부모 트랜스폼 배열.
    [SerializeField] private Transform[] skillElementType;

    // 생성된 스킬 아이콘들을 이름으로 빠르게 찾기 위한 딕셔너리.
    private Dictionary<string, UISkillIcon> skillIcons;

    public void Setup(Dictionary<string, SkillTemplate> skills)
    {
        skillIcons = new Dictionary<string, UISkillIcon>();

        foreach (var item in skills)
        {
            // 스킬의 속성 값에서 100을 빼서 배열 인덱스로 사용.
            SpawnIcon(item.Value, skillElementType[(int)item.Value.skillElement - 100]);
        }
    }

    /// <summary>
    /// 특정 스킬이 레벨업했을 때 해당 아이콘의 UI를 갱신하는 메소드.
    /// </summary>
    /// <param name="skill">레벨업이 발생한 스킬 객체</param>
    public void LevelUp(SkillBase skill)
    {
        // 관리 중인 아이콘 목록에 해당 스킬이 있는지 확인.
        if (skillIcons.ContainsKey(skill.SkillName))
        {
            // 해당 아이콘을 찾아 현재 레벨과 활성화 이미지를 전달.
            skillIcons[skill.SkillName].LevelUp(skill.CurrentLevel, skill.EnableIcon);
        }
    }

    /// <summary>
    /// 실제 스킬 아이콘 오브젝트를 생성하고 초기 상태를 설정하는 메소드.
    /// </summary>
    /// <param name="skill">생성할 스킬의 템플릿 데이터</param>
    /// <param name="parent">배치될 속성별 부모 위치</param>
    private void SpawnIcon(SkillTemplate skill, Transform parent)
    {
        // 프리팹 복제 생성.
        var clone = Instantiate(skillIconPrefab, parent);
        // UI 크기 왜곡 방지를 위해 스케일을 1로 초기화.
        clone.transform.localScale = Vector3.one;
        // 미습득 상태로 초기 UI 설정.
        clone.Setup(skill.disableIcon);
        
        // 딕셔너리에 저장하여 추후 레벨업 시 참조 가능하게 함.
        skillIcons.Add(skill.skillName, clone);
    }
}

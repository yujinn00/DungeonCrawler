using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillSystem : MonoBehaviour
{
    // 기본 공격을 담당하는 투사체 스크립트.
    [SerializeField] private SkillBullet skillBullet;
    // 투사체가 발사될 위치.
    [SerializeField] private Transform skillSpawnPoint;

    // 스킬을 사용하는 주체.
    private PlayerBase owner;

    // 게임에 존재하는 모든 스킬을 관리하는 사전.
    private Dictionary<string, SkillBase> skills = new Dictionary<string, SkillBase>();

    private void Awake()
    {
        owner = GetComponent<PlayerBase>();
        skillBullet.Setup(owner, skillSpawnPoint);
        
        // "Resources/Skills/" 폴더에 있는 모든 스킬 데이터 로드.
        var skillDict = Resources.LoadAll<SkillTemplate>("Skills/").ToDictionary(item => item.name, item => item);
        // 로드한 데이터를 기반으로 실제 스킬 객체를 생성하고 등록.
        foreach (var item in skillDict)
        {
            SkillBase skill = null;
            if (item.Value.skillType.Equals(SkillType.Buff))
            {
                skill = new SkillBuff();
            }
            
            // 스킬 데이터와 주체 초기화.
            skill.Setup(item.Value, owner);
            // 완성된 스킬을 사전에 등록.
            skills.Add(item.Key, skill);
            
            Logger.Log($"[{skill.SkillName}] Lv. {skill.CurrentLevel}\n{skill.Description}");
        }
    }

    private void Update()
    {
        // 키보드 숫자 1을 누르면 강제로 스킬 획득 시도.
        if (UnityEngine.InputSystem.Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            SelectSkill();
        }
        
        // 플레이어의 목표가 없거나, 이동 중이면 모든 스킬 시전 불가.
        if (owner.Target == null || owner.IsMoved == true)
        {
            return;
        }
        
        // 기본 공격 스킬 업데이트.
        skillBullet.OnSkill();
    }

    /// <summary>
    /// 특정 스킬의 레벨을 실제로 올리는 메소드.
    /// </summary>
    public void LevelUp(SkillBase skill)
    {
        // 사전에 등록된 스킬인지 확인.
        if (skills.ContainsValue(skill))
        {
            // 스킬 내부의 레벨업 로직 실행.
            skill.TryLevelUp();
            Logger.Log($"Level Up [{skill.SkillName}] {skill.SkillElement}, Lv. {skill.CurrentLevel}");
        }
    }

    /// <summary>
    /// 레벨업 시 랜덤 스킬을 뽑고 선택하는 메소드.
    /// </summary>
    public void SelectSkill()
    {
        // 습득 가능한 임의의 스킬 3개 추출.
        var randomSkills = GetRandomSkills(skills, 3);
        if (randomSkills == null)
        {
            Logger.Log("더 이상 습득할 수 있는 스킬이 없습니다.");
            return;
        }
        
        // 스킬 선택 UI가 없기 때문에 임시로 스킬 습득 처리.
        int index = Random.Range(0, randomSkills.Count);
        LevelUp(randomSkills[index]);
    }

    /// <summary>
    /// 전체 스킬 중 마스터하지 않은 스킬 N개를 랜덤으로 뽑는 메소드.
    /// </summary>
    private List<SkillBase> GetRandomSkills(Dictionary<string, SkillBase> skills, int count = 3)
    {
        // 이미 최고 레벨인 스킬은 제외하고, 습득 가능한 후보 리스트 생성.
        var values = new List<SkillBase>(skills.Values.Where(skill => !skill.IsMaxLevel)).ToList();
        var randomSkills = new List<SkillBase>();

        // 후보가 요청 수보다 적으면 남은 개수만큼만 추출.
        count = values.Count == 0 ? 0 : count;
        
        if (values.Count < count)
        {
            count = values.Count;
        }

        if (count == 0)
        {
            return null;
        }

        // 랜덤 뽑기.
        for (int i = 0; i < count; ++i)
        {
            // 남은 후보 중에서 랜덤으로 하나 추출.
            int index = Random.Range(0, values.Count);
            
            // 추출한 스킬을 결과 리스트에 추가.
            randomSkills.Add(values[index]);
            
            // 중복 방지를 위해 선택된 항목 제거.
            values.RemoveAt(index);
        }
        
        Logger.Log($"선택 가능한 3개의 스킬\n{randomSkills[0].SkillName}," +
                   $"{randomSkills[1].SkillName}, {randomSkills[2].SkillName}");
        
        return randomSkills;
    }
}

using UnityEngine;

public abstract class SkillBase
{
    // 스킬 정보.
    protected SkillTemplate skillTemplate;
    // 스킬 소유자.
    protected PlayerBase owner;
    // 현재 스킬 레벨.
    protected int currentLevel = 0;
    
    // 외부에서 접근이 필요한 스킬의 정보들을 Get 프로퍼티로 정의.
    public string SkillName => skillTemplate.skillName;
    public SkillType SkillType => skillTemplate.skillType;
    public SkillElement SkillElement => skillTemplate.skillElement;
    public string Description => skillTemplate.description;
    public int CurrentLevel => currentLevel;
    public bool IsMaxLevel => currentLevel == skillTemplate.maxLevel;
    public Sprite EnableIcon => skillTemplate.enableIcon;

    public virtual void Setup(SkillTemplate skillTemplate, PlayerBase owner)
    {
        this.skillTemplate = skillTemplate;
        this.owner = owner;
    }

    public void TryLevelUp()
    {
        if (IsMaxLevel)
        {
            Logger.Log($"'[{SkillName}] 스킬이 최고 레벨에 도달했습니다.");
            return;
        }

        currentLevel++;

        OnLevelUp();
    }

    // 스킬 레벨업 시 1회 호출.
    public abstract void OnLevelUp();
    // 스킬 사용 시 호츨.
    public abstract void OnSkill();
}

public class SkillReinforce : SkillBase
{
    public override void OnLevelUp()
    {
        skillTemplate.reinforceStatList.ForEach(stat =>
        {
            owner.Stats.GetStat(stat).BonusValue += stat.DefaultValue;
        });
    }
    
    public override void OnSkill() { }
}

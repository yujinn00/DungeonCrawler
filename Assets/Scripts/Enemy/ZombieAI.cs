public class ZombieAI : EnemyAI
{
    
    public override void PerformAttack()
    {
        if (owner.Target == null)
        {
            return;
        }

        Logger.Log($"{gameObject.name}가 할퀴기를 시전했습니다.");

        owner.Target.TakeDamage(owner.Stats.GetStat(StatType.AttackDamage).Value);
    }
}

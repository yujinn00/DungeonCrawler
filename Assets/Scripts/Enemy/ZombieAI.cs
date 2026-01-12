using UnityEngine;

public class ZombieAI : EnemyAI
{
    
    public override void PerformAttack()
    {
        if (owner.Target == null)
        {
            return;
        }

        Debug.Log($"<color=red>{gameObject.name}가 할퀴기 시전!</color>");

        owner.Target.TakeDamage(Stats.attackDamage);
    }
}

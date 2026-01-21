using UnityEngine;

public class SkillBullet : MonoBehaviour
{
    [SerializeField] private ProjectileBullet projectileBullet;

    private float currentCooldownTime;
    private Transform spawnPoint;
    private PlayerBase owner;

    public void Setup(PlayerBase owner, Transform spawnPoint)
    {
        this.owner = owner;
        this.spawnPoint = spawnPoint;
    }

    public void OnSkill()
    {
        // 스킬이 사용 가능한 상태인지 쿨타임 검사.
        if (Time.time - currentCooldownTime > owner.Stats.GetStat(StatType.AttackSpeed).Value)
        {
            var result = CalculateDamage();
            ProjectileBullet projectile = Instantiate(projectileBullet, spawnPoint.position, Quaternion.identity);
            projectile.Setup(owner.Target, result);
            
            currentCooldownTime = Time.time;
        }
    }

    private float CalculateDamage()
    {
        bool isCriticalHit = Random.value < owner.Stats.GetStat(StatType.CriticalChance).Value;
        float damage = owner.Stats.GetStat(StatType.AttackDamage).Value;

        if (isCriticalHit)
        {
            return damage * owner.Stats.GetStat(StatType.CriticalDamage).Value;
        }
        else
        {
            return damage;
        }
    }
}

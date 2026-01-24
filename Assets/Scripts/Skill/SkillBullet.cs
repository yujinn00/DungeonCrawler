using UnityEngine;

public class SkillBullet : MonoBehaviour
{
    // 생성할 투사체 프리팹.
    [SerializeField] private ProjectileBullet projectileBullet;

    // 마지막으로 스킬을 사용한 시간.
    private float currentCooldownTime;
    // 투사체가 생성될 위치.
    private Transform spawnPoint;
    // 스킬을 사용하는 주체.
    private PlayerBase owner;

    public void Setup(PlayerBase owner, Transform spawnPoint)
    {
        this.owner = owner;
        this.spawnPoint = spawnPoint;
    }

    /// <summary>
    /// 실제 스킬을 사용하는 메소드.
    /// </summary>
    public void OnSkill()
    {
        // 쿨타임 체크 로직.
        if (Time.time - currentCooldownTime > owner.Stats.GetStat(StatType.AttackSpeed).Value)
        {
            // 데미지 계산.
            var result = CalculateDamage();
            // 투사체 생성.
            ProjectileBullet bullet = Instantiate(projectileBullet, spawnPoint.position, Quaternion.identity);
            // 투사체 초기화.
            bullet.Setup(owner.Target, result.Item1, result.Item2);
            
            // 쿨타임 초기화.
            currentCooldownTime = Time.time;
        }
    }

    /// <summary>
    /// 데미지와 크리티컬 여부를 계산하는 메소드.
    /// 반환 타입은 C# 튜플 문법으로, 두 개의 값을 한 번에 반환.
    /// </summary>
    /// <returns>데미지, 크리티컬 여부</returns>
    private (float, bool) CalculateDamage()
    {
        // 0.0 ~ 1.0 사이의 랜덤 값이 크리티컬 확률보다 낮으면 크리티컬 성공.
        bool isCriticalHit = Random.value < owner.Stats.GetStat(StatType.CriticalChance).Value;
        // 플레이어의 기본 공격력 가져오기.
        float damage = owner.Stats.GetStat(StatType.AttackDamage).Value;

        if (isCriticalHit)
        {
            // 크리티컬 O: 기본 데미지 * 크리티컬 배율 반환.
            return (damage * owner.Stats.GetStat(StatType.CriticalDamage).Value, isCriticalHit);
        }
        else
        {
            // 크리티컬 X: 기본 데미지 반환.
            return (damage, isCriticalHit);
        }
    }
}

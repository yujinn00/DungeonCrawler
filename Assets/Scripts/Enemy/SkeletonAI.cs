using UnityEngine;

public class SkeletonAI : EnemyAI
{
    // 생성할 투사체 프리팹.
    [SerializeField] private GameObject projectilePrefab;
    // 투사체가 발사될 위치.
    [SerializeField] private Transform projectileSpawnPoint;

    public override void PerformAttack()
    {
        if (owner.Target == null)
        {
            return;
        }
        
        Logger.Log($"{gameObject.name}가 투사체를 투척했습니다.");
        
        // 발사 위치 결정.
        Vector3 spawnPos = projectileSpawnPoint != null ? projectileSpawnPoint.position : transform.position;
        // 투사체 생성.
        GameObject clone = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        
        // 투사체 컴포넌트를 가져와서 초기화.
        var projectile = clone.GetComponent<EnemyProjectile>();
        if (projectile != null && owner.Target != null)
        {
            // 타겟의 현재 위치로 날아가도록 목표점을 설정하고,
            // 몬스터 스탯에 있는 공격력 정보를 넘겨줌.
            projectile.Setup(owner.Target.transform.position, owner.Stats.GetStat(StatType.AttackDamage).Value);
        }
    }
}

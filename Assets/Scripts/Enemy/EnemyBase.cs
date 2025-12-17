public class EnemyBase : EntityBase<EnemyStats>
{
    private void Awake()
    {
        // 몬스터는 플레이어와 달리 종류마다 기본 스탯과 레벨별 성장 공식이 다르므로,
        // 오버라이드된 Setup을 호출하여 각 몬스터에 맞는 수치를 먼저 계산한 뒤 초기화함.
        Setup();
    }

    protected override void Setup()
    {
        // 인스펙터에 적힌 기본값에 (레벨 성장치 * 단계)를 더함.
        stats.maxHP += stats.hpStep * (stats.level - 1);
        stats.attackDamage += stats.damageStep * (stats.level - 1);
        
        // 최종 계산된 최대 체력을 현재 체력에 반영하기 위해 부모의 Setup 호출.
        base.Setup();
    }
}

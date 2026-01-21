using UnityEngine;

public class SkillSystem : MonoBehaviour
{
    [SerializeField] private SkillBullet skillBullet;
    [SerializeField] private Transform skillSpawnPoint;

    private PlayerBase owner;

    private void Awake()
    {
        owner = GetComponent<PlayerBase>();
        skillBullet.Setup(owner, skillSpawnPoint);
    }

    private void Update()
    {
        // 플레이어의 목표가 없거나, 이동 중이면 모든 스킬 시전 불가.
        if (owner.Target == null || owner.IsMoved == true)
        {
            return;
        }
        
        // 기본 공격 스킬 업데이트.
        skillBullet.OnSkill();
    }
}

using UnityEngine;

public class ProjectileBullet : MonoBehaviour
{
    // 히트 이펙트 프리팹.
    [SerializeField] private Transform hitEffect;
    // 데미지 텍스트 프리팹.
    [SerializeField] private UIDamageText damageText;
    
    // 물리 이동 처리를 위한 MovementRigidbody2D 참조 변수.
    private MovementRigidbody2D movementRigidbody2D;
    // 투사체가 추적할 타겟.
    private EntityBase target;
    // 투사체가 입힐 데미지.
    private float damage;
    // 치명타 발생 여부.
    private bool isCritical;

    public void Setup(EntityBase target, float damage, bool isCritical = false)
    {
        movementRigidbody2D = GetComponent<MovementRigidbody2D>();
        this.target = target;
        this.damage = damage;
        this.isCritical = isCritical;
        
        // 발사체를 목표 방향으로 회전.
        transform.rotation = Utils.RotateToTarget(transform.position, target.MiddlePoint, 90);

        // 발사체 이동 방향 설정.
        movementRigidbody2D.MoveTo((target.MiddlePoint - transform.position).normalized);
        
        // 3초 동안 아무것도 못 맞추고 멀리 날아가면 스스로 삭제.
        Destroy(gameObject, 3.0f);
    }
    
    // 투사체의 Collider가 다른 물체와 닿았을 때 자동으로 실행.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 벽과 충돌했을 경우, 그냥 삭제함.
        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        // 몬스터와 충돌했을 경우, 몬스터에게 데미지를 입힘.
        else if (collision.CompareTag("Enemy") && collision.TryGetComponent<EntityBase>(out var entity))
        {
            if (entity != target)
            {
                return;
            }

            if (hitEffect != null)
            {
                // 충돌 위치에 히트 이펙트 생성.
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }

            if (damageText != null)
            {
                // 충돌 위치에 데미지 텍스트 생성. 
                UIDamageText clone = Instantiate(damageText, transform.position, Quaternion.identity);
                // 데미지 수치를 문자열로 변환하고, 크리티컬 여부에 따라 색상 설정.
                clone.Setup(damage.ToString("F0"), isCritical ? Color.red : Color.white);
            }
            
            entity.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}

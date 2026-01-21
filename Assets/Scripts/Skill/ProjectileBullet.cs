using UnityEngine;

public class ProjectileBullet : MonoBehaviour
{
    // 피격 이펙트 프리팹.
    [SerializeField] private Transform hitEffect;
    
    // 물리 이동 처리를 위한 MovementRigidbody2D 참조 변수.
    private MovementRigidbody2D movementRigidbody2D;
    // 투사체가 추적할 타겟.
    private EntityBase target;
    // 투사체가 입힐 데미지.
    private float damage;

    public void Setup(EntityBase target, float damage)
    {
        movementRigidbody2D = GetComponent<MovementRigidbody2D>();
        this.target = target;
        this.damage = damage;
        
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
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }
            
            entity.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}

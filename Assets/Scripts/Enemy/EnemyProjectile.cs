using UnityEngine;

// 이 스크립트가 붙은 오브젝트는 반드시 MovementRigidbody2D가 있어야 함을 강제함.
[RequireComponent(typeof(MovementRigidbody2D))]
public class EnemyProjectile : MonoBehaviour
{
    // 물리 이동 처리를 위한 MovementRigidbody2D 참조 변수.
    private MovementRigidbody2D movementRigidbody2D;
    // 투사체가 입힐 데미지.
    private float damage;

    public void Setup(Vector3 target, float damage)
    {
        movementRigidbody2D = GetComponent<MovementRigidbody2D>();
        this.damage = damage;
        
        // 발사체 이동 방향 설정.
        movementRigidbody2D.MoveTo((target - transform.position).normalized);
        
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
        // 플레이어와 충돌했을 경우, 플레이어에게 데미지를 입힘.
        else if (collision.CompareTag("Player") && collision.TryGetComponent<EntityBase>(out var entity))
        {
            entity.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}

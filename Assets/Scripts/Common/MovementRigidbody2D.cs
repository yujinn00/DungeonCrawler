using UnityEngine;

// 이 스크립트가 부착된 게임 오브젝트는 반드시 Rigidbody2D 컴포넌트를 필요로 함.
[RequireComponent(typeof(Rigidbody2D))]
public class MovementRigidbody2D : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Rigidbody2D rigid2D;

    private void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// PlayerBase의 스탯을 참조하여 Rigidbody2D로 물리 이동을 처리하는 함수.
    /// Rigidbody2D의 선형 속도(linearVelocity)를 직접 설정하여 오브젝트를 이동시킴.
    /// 방향 벡터(direction)에 이동 속도(moveSpeed)를 곱하여 최종 속도를 결정함.
    /// Rigidbody를 사용하면 프레임 속도에 관계없이 안정적인 물리 이동이 가능함.
    /// </summary>
    /// <param name="direction">이동하고자 하는 방향 벡터</param>
    public void MoveTo(Vector3 direction)
    {
        // 가져온 속도를 방향 벡터에 곱하여 물리 속도를 적용함.
        rigid2D.linearVelocity = direction * moveSpeed;
    }
}

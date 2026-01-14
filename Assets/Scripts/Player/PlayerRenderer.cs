using UnityEngine;

public class PlayerRenderer : MonoBehaviour
{
    // 플레이어의 좌우 반전을 위한 Transform.
    [SerializeField] private Transform playerModel;
    // 플레이어의 무기 회전을 위한 Transform.
    [SerializeField] private Transform playerArmsModel;
    
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// 플레이어의 이동 속도에 따라 애니메이션을 재생/정지시키는 메소드.
    /// </summary>
    /// <param name="speed">이동 속도 값</param>
    public void OnMovement(float speed)
    {
        // Animator 컴포넌트의 "moveSpeed"라는 이름의 Float 파라미터 값을 설정함.
        // 이 파라미터 값에 따라 Animator Controller 내에서 Idle <-> Run 전환이 일어남.
        animator.SetFloat("moveSpeed", speed);
    }

    /// <summary>
    /// 플레이어의 이동 방향(X축)에 따라 모델을 좌우 반전시키는 메소드.
    /// </summary>
    /// <param name="x">플레이어의 X축 이동 입력 값</param>
    public void SpriteFlipX(float x)
    {
        // 현재 playerModel의 스케일 값을 가져옴.
        Vector3 currentScale = playerModel.localScale;
        
        // x < 0 (왼쪽 이동): 스케일 X를 -1.5f로 설정하여 모델을 좌우 반전시킴.
        // x >= 0 (오른쪽 또는 정지): 스케일 X를 1.5f로 설정하여 모델을 기본 방향으로 유지함.
        // 참고로 1.5f 값은 기본 스케일을 1.5배 확대한 값임.
        currentScale.x = x < 0 ? -1.5f : 1.5f;
        
        // 변경된 스케일 값을 playerModel에 다시 적용함.
        playerModel.localScale = currentScale;
    }

    /// <summary>
    /// 플레이어의 이동 상태에 따라 캐릭터의 좌우 반전 및 무기의 조준 회전을 처리하는 메소드.
    /// </summary>
    /// <param name="playerBase"></param>
    public void LookRotation(PlayerBase playerBase)
    {
        if (playerBase.Target == null)
        {
            return;
        }

        Vector3 target = playerBase.Target.MiddlePoint;

        // 타겟이 플레이어보다 왼쪽에 있으면 -1, 오른쪽에 있으면 1.
        float flip = target.x - transform.position.x < 0 ? -1 : 1;

        // 계산된 방향에 따라 캐릭터 스프라이트 좌우 반전.
        SpriteFlipX(flip);

        // 플레이어 무기를 타겟 방향으로 회전.
        // flip이 1(오른쪽)일 때: (1 - 1) * 90 = 0도 보정 (그대로)
        // flip이 -1(왼쪽)일 때: (1 - (-1)) * 90 = 180도 보정 (반전)
        // 이유: 스프라이트가 왼쪽을 볼 때 회전축도 같이 뒤집히기 때문에, 각도를 180도 돌려줘야 정상적으로 타겟을 향함.
        playerArmsModel.rotation = Utils.RotateToTarget(playerArmsModel.position, target, (1 - flip) * 90);
    }
}

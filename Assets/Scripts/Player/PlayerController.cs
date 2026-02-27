using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private MovementRigidbody2D movement2D;
    private PlayerRenderer playerRenderer;
    private PlayerBase playerBase;
    private SkillDash skillDash;
    
    // 이동 방향 (기본값은 정지 상태).
    private Vector2 moveInput = Vector2.zero;
    // 마지막으로 이동한 방향 (기본값은 위쪽).
    private Vector2 lastMoveDirection = Vector2.up;

    private void Awake()
    {
        movement2D = GetComponent<MovementRigidbody2D>();
        playerRenderer = GetComponentInChildren<PlayerRenderer>();
        playerBase = GetComponent<PlayerBase>();
        skillDash = GetComponent<SkillDash>();
    }

    private void Update()
    {
        // 대쉬 중에는 이동 입력을 무시하여 대쉬 관성을 유지함.
        if (skillDash != null && skillDash.IsDashing)
        {
            return;
        }
        
        // 플레이어가 현재 이동 입력을 하고 있는지 검사함.
        playerBase.IsMoved = moveInput.x != 0 || moveInput.y != 0;
        
        // 플레이어를 좌우 반전 처리함.
        if (moveInput.x != 0)
        {
            // moveInput.x 값에 따라 스프라이트를 좌우 반전시킴.
            playerRenderer.SpriteFlipX(moveInput.x);
        }
        
        // isMoved가 true면 1을, false면 0을 전달하여,
        // Idle 또는 Run 애니메이션을 재생하도록 함.
        playerRenderer.OnMovement(playerBase.IsMoved ? 1 : 0);
        
        // 현재 입력된 방향으로 캐릭터를 물리적으로 이동시킴.
        movement2D.MoveTo(moveInput);
        
        // 목표 방향으로 플레이어 및 무기 회전.
        playerRenderer.LookRotation(playerBase);
    }
    
    /// <summary>
    /// Input System의 Dash에서 콜백 함수.
    /// </summary>
    /// <param name="context">입력 이벤트의 컨텍스트 정보</param>
    public void OnDash(InputAction.CallbackContext context)
    {
        // 키를 눌렀을 때 대쉬 실행.
        if (context.started && skillDash != null)
        {
            // 현재 입력이 (0, 0)이면 마지막 방향으로, 아니면 현재 입력 방향으로 대쉬.
            Vector2 dashDir = moveInput == Vector2.zero ? lastMoveDirection : moveInput.normalized;
            skillDash.OnDash(dashDir);
        }
    }

    /// <summary>
    /// Input System의 Action에서 호출되는 콜백 함수.
    /// 플레이어의 이동 입력(WASD)을 받아 moveInput 변수에 저장함.
    /// </summary>
    /// <param name="context">입력 이벤트의 컨텍스트 정보</param>
    public void OnMove(InputAction.CallbackContext context)
    {
        // context.performed: 키를 눌렀을 때.
        // context.canceled: 키를 놓았을 때.
        if (context.performed || context.canceled)
        {
            // 키를 눌렀을 때: (1, 0)과 같은 이동 방향을 moveInput에 저장함.
            // 키를 놓았을 때: (0, 0)과 같은 정지 상태를 moveInput에 저장함.
            moveInput = context.ReadValue<Vector2>();
            
            // 입력이 발생했을 때만 마지막 방향을 갱신함.
            if (moveInput != Vector2.zero)
            {
                lastMoveDirection = moveInput.normalized;
            }
        }
    }
}

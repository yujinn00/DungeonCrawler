using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private MovementRigidbody2D movement2D;
    private PlayerRenderer playerRenderer;
    private Vector2 moveInput = Vector2.zero; // 초기값은 정지 상태 (0, 0).

    private void Awake()
    {
        movement2D = GetComponent<MovementRigidbody2D>();
        playerRenderer = GetComponentInChildren<PlayerRenderer>();
    }

    private void Update()
    {
        // 플레이어가 현재 이동 입력을 하고 있는지 검사함.
        bool isMoved = moveInput.x != 0 || moveInput.y != 0;
        
        // 플레이어를 좌우 반전 처리함.
        if (moveInput.x != 0)
        {
            // moveInput.x 값에 따라 스프라이트를 좌우 반전시킴.
            playerRenderer.SpriteFlipX(moveInput.x);
        }
        
        // isMoved가 true면 1을, false면 0을 전달하여,
        // Idle 또는 Run 애니메이션을 재생하도록 함.
        playerRenderer.OnMovement(isMoved ? 1 : 0);
        
        // 현재 입력된 방향으로 캐릭터를 물리적으로 이동시킴.
        movement2D.MoveTo(moveInput);
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
        }
    }
}

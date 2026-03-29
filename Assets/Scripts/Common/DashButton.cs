using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DashButtonHandler : MonoBehaviour, IPointerDownHandler
{
    // 플레이어 컨트롤러 컴포넌트.
    [SerializeField] private PlayerController playerController;
    // 스킬 대쉬 컴포넌트.
    [SerializeField] private SkillDash skillDash;
    // 버튼의 시각적 피드백을 위한 이미지.
    [SerializeField] private Image buttonImage;
    // 쿨타임 중일 때 적용할 알파값.
    [SerializeField] private float cooldownAlpha = 0.3f;

    private void Update()
    {
        if (skillDash == null || buttonImage == null)
        {
            return;
        }

        // 대쉬 중이거나 쿨타임 중이라면 버튼을 반투명하게 설정.
        if (skillDash.IsDashing || !skillDash.CanDash()) 
        {
            SetAlpha(cooldownAlpha);
        }
        // 사용 가능한 상태라면 버튼을 원래 밝기로 복구.
        else
        {
            SetAlpha(1.0f);
        }
    }
    
    private void SetAlpha(float alpha)
    {
        if (buttonImage != null)
        {
            Color color = buttonImage.color;
            color.a = alpha;
            buttonImage.color = color;
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (playerController != null && skillDash != null)
        {
            // 플레이어로부터 현재 이동 값과 마지막 이동 방향을 가져옴.
            Vector2 moveInput = playerController.GetMoveInput();
            Vector2 lastDir = playerController.GetLastMoveDirection();

            // 이동 중이면 입력 방향으로, 정지 상태면 마지막 시선 방향으로 결정.
            Vector2 dashDir = (moveInput == Vector2.zero) ? lastDir : moveInput.normalized;
            
            // 결정된 방향으로 대쉬 스킬 발동.
            skillDash.OnDash(dashDir);
        }
    }
}

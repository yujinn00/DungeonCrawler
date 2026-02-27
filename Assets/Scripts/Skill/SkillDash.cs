using UnityEngine;
using System.Collections;

public class SkillDash : MonoBehaviour
{
    // 대쉬 이동 속도.
    [SerializeField] private float dashForce = 20f;
    // 대쉬 지속 시간.
    [SerializeField] private float dashDuration = 0.2f;
    // 대쉬 중 투명도.
    [SerializeField, Range(0f, 1f)] private float dashAlpha = 0.2f;
    
    // 플레이어 본체.
    private PlayerBase playerBase;
    // 물리 엔진 컴포넌트.
    private Rigidbody2D rigid2D;
    // 본체와 무기 등 모든 스프라이트 조절용.
    private SpriteRenderer[] spriteRenderers;
    // 마지막 대쉬 시점 저장 (쿨타임 계산).
    private float lastDashTime = -99f;
    
    // 기본 레이어 (Default).
    private int playerLayer;
    // 대쉬 전용 무적 레이어 (Dash).
    private int dashLayer;
    
    // 외부에서 대쉬 상태를 확인할 수 있는 프로퍼티.
    public bool IsDashing { get; private set; }

    private void Awake()
    {
        playerBase = GetComponent<PlayerBase>();
        rigid2D = GetComponent<Rigidbody2D>();
        
        // 자식 오브젝트를 포함한 모든 SpriteRenderer를 미리 찾아 배열에 저장 (성능 최적화).
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        
        // 레이어 번호를 미리 캐싱하여 오타 방지 및 성능 향상
        playerLayer = LayerMask.NameToLayer("Default");
        dashLayer = LayerMask.NameToLayer("Dash");
    }

    /// <summary>
    /// PlayerController로부터 방향을 전달받아 대쉬를 시작하는 메소드.
    /// </summary>
    /// <param name="dashDir">대쉬할 방향</param>
    public void OnDash(Vector2 dashDir)
    {
        // 현재 스탯 시스템에서 쿨타임 값을 실시간으로 가져옴.
        float cooldown = playerBase.Stats.GetStat(StatType.DashCooldown).Value;
        
        // 쿨타임 중이거나 이미 대쉬 중이라면 무시함.
        if (Time.time - lastDashTime < cooldown || IsDashing)
        {
            return;
        }

        // 실제 대쉬 물리 로직과 시각 연출을 담당하는 코루틴 실행.
        StartCoroutine(DashCoroutine(dashDir, cooldown));
    }

    private IEnumerator DashCoroutine(Vector2 direction, float cooldown)
    {
        IsDashing = true;
        lastDashTime = Time.time;

        // 무적 레이어로 변경 및 투명화 연출.
        gameObject.layer = dashLayer;
        SetAllSpritesAlpha(dashAlpha);

        // 중력 무시 및 정해진 방향으로 강한 속도 부여.
        float originalGravity = rigid2D.gravityScale;
        rigid2D.gravityScale = 0f;
        rigid2D.linearVelocity = direction * dashForce;

        // 설정한 지속 시간만큼 대쉬 유지.
        yield return new WaitForSeconds(dashDuration);
	
        // 원래 레이어 및 불투명도 복구.
        gameObject.layer = playerLayer;
        SetAllSpritesAlpha(1.0f);

        // 중력 복구 및 속도 초기화.
        rigid2D.gravityScale = originalGravity;
        rigid2D.linearVelocity = Vector2.zero;
        IsDashing = false;
        
        // 이미 대쉬 시간만큼 흘렀으므로, 남은 쿨타임 시간만큼 추가 대기.
        float remainingCooldown = cooldown - dashDuration;
        if (remainingCooldown > 0)
        {
            yield return new WaitForSeconds(remainingCooldown);
        }

        Logger.Log("<color=cyan><b>[Dash]</b></color> 대쉬 쿨타임이 돌아왔습니다!");
    }
    
    /// <summary>
    /// 캐릭터 본체와 총을 포함한 모든 자식 스프라이트의 알파값을 일괄 변경하는 메소드.
    /// </summary>
    /// <param name="alpha"></param>
    private void SetAllSpritesAlpha(float alpha)
    {
        if (spriteRenderers == null)
        {
            return;
        }

        foreach (var sr in spriteRenderers)
        {
            if (sr != null)
            {
                Color color = sr.color;
                color.a = alpha;
                sr.color = color;
            }
        }
    }
}

using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

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
    
    // 오브젝트 파괴 시 진행 중인 작업을 취소하기 위한 소스.
    private CancellationTokenSource cts;

    private void Awake()
    {
        playerBase = GetComponent<PlayerBase>();
        rigid2D = GetComponent<Rigidbody2D>();
        
        // 자식 오브젝트를 포함한 모든 SpriteRenderer를 미리 찾아 배열에 저장 (성능 최적화).
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        
        // 레이어 번호를 미리 캐싱하여 오타 방지 및 성능 향상.
        playerLayer = LayerMask.NameToLayer("Default");
        dashLayer = LayerMask.NameToLayer("Dash");
    }
    
    private void OnDestroy()
    {
        // 오브젝트가 파괴될 때 실행 중인 모든 비동기 작업을 안전하게 종료함.
        if (cts != null)
        {
            cts.Cancel();
            cts.Dispose();
        }
    }

    /// <summary>
    /// PlayerController로부터 방향을 전달받아 대쉬를 시작하는 메소드.
    /// </summary>
    /// <param name="direction">대쉬할 방향</param>
    public void OnDash(Vector2 direction)
    {
        // 현재 스탯 시스템에서 쿨타임 값을 실시간으로 가져옴.
        float cooldown = playerBase.Stats.GetStat(StatType.DashCooldown).Value;
        
        // 쿨타임 중이거나 이미 대쉬 중이라면 무시함.
        if (Time.time - lastDashTime < cooldown || IsDashing)
        {
            return;
        }
        
        // UniTask 비동기 메소드 호출.
        DashAsync(direction, cooldown).Forget();
    }

    private async UniTaskVoid DashAsync(Vector2 direction, float cooldown)
    {
        IsDashing = true;
        lastDashTime = Time.time;

        // 기존 작업이 있다면 취소하고 새로 생성.
        cts?.Cancel();
        cts?.Dispose();
        cts = new CancellationTokenSource();
        var token = cts.Token;

        try
        {
            // 무적 레이어로 변경 및 투명화 연출.
            gameObject.layer = dashLayer;
            SetAllSpritesAlpha(dashAlpha);

            // 중력 무시 및 정해진 방향으로 강한 속도 부여.
            float originalGravity = rigid2D.gravityScale;
            rigid2D.gravityScale = 0f;
            rigid2D.linearVelocity = direction * dashForce;

            // 설정한 지속 시간만큼 대쉬 유지.
            await UniTask.Delay((int)(dashDuration * 1000), cancellationToken: token);

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
                await UniTask.Delay((int)(remainingCooldown * 1000), cancellationToken: token);
            }

            Logger.Log("대쉬 쿨타임이 돌아왔습니다.");
        }
        catch (System.OperationCanceledException)
        {
            
        }
    }
    
    /// <summary>
    /// 캐릭터 본체와 총을 포함한 모든 자식 스프라이트의 알파값을 일괄 변경하는 메소드.
    /// </summary>
    /// <param name="alpha">투명도</param>
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

    /// <summary>
    /// 현재 대쉬 스킬이 사용 가능한 상태인지 확인하는 메소드.
    /// </summary>
    public bool CanDash()
    {
        // 스탯 시스템에서 실시간 대쉬 쿨타임 값을 가져옴.
        float cooldown = playerBase.Stats.GetStat(StatType.DashCooldown).Value;
    
        // 마지막 대쉬 이후 쿨타임이 지났고, 현재 대쉬 중이 아닐 때만 true 반환.
        return (Time.time - lastDashTime >= cooldown) && !IsDashing;
    }
}

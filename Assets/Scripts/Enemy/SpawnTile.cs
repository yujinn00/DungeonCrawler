using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class SpawnTile : MonoBehaviour
{
    // 타일의 이미지 스프라이트.
    private SpriteRenderer spriteRenderer;
    // 오브젝트 파괴 시 진행 중인 작업을 취소하기 위한 소스.
    private CancellationTokenSource cts;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        // 작업 취소를 위한 토큰 소스 초기화.
        cts = new CancellationTokenSource();
        // 비동기 루프 함수 호출.
        FadeLoop(cts.Token).Forget();
    }

    private void OnDisable()
    {
        // 오브젝트 비활성화 시 진행 중인 모든 비동기 작업 즉시 취소.
        if (cts != null)
        {
            cts.Cancel();
            cts.Dispose();
            cts = null;
        }
    }

    /// <summary>
    /// 취소 요청이 오기 전까지 타일을 무한히 깜빡이게 하는 루프 함수.
    /// </summary>
    /// <param name="token">작업 취소 여부를 확인하기 위한 토큰</param>
    private async UniTaskVoid FadeLoop(CancellationToken token)
    {
        try
        {
            // 토큰을 통해 취소 요청이 있는지 매 루프마다 확인.
            while (!token.IsCancellationRequested)
            {
                // 0.5초 동안 서서히 사라짐.
                await FadeEffect.Fade(spriteRenderer, 1, 0, 0.5f);
                
                // 0.5초 동안 서서히 나타남.
                await FadeEffect.Fade(spriteRenderer, 0, 1, 0.5f);
            }
        }
        catch (System.OperationCanceledException)
        {
            
        }
    }
}

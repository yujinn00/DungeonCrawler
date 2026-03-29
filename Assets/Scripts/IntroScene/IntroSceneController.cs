using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using System.Threading;

public class IntroSceneController : MonoBehaviour
{
    // 전환될 다음 씬 이름.
    [SerializeField] private SceneNames nextScene;
    // 점멸할 안내 텍스트.
    [SerializeField] private TextMeshProUGUI textPressAnyKey;

    // 오브젝트가 파괴될 때 비동기 작업을 멈추기 위한 취소 토큰 소스.
    private CancellationTokenSource cts;

    private void Awake()
    {
        // 발열 방지 및 배터리 절약을 위해 최대 프레임을 60으로 제한.
        Application.targetFrameRate = 60;
    }
    
    private void Start()
    {
        cts = new CancellationTokenSource();
        
        // 텍스트 점멸 루프 시작.
        PlayBlinkingText(cts.Token).Forget();
    }
    
    /// <summary>
    /// 텍스트를 무한히 깜빡이게 하는 비동기 루프 메소드.
    /// </summary>
    /// <param name="token">취소 토큰 소스</param>
    private async UniTaskVoid PlayBlinkingText(CancellationToken token)
    {
        try
        {
            // 토큰에 취소 요청이 오기 전까지 무한 반복.
            while (!token.IsCancellationRequested)
            {
                // Fade Out 대기.
                await FadeEffect.Fade(textPressAnyKey, 1, 0, 1f);
                
                // Fade In 대기.
                await FadeEffect.Fade(textPressAnyKey, 0, 1, 1f);
            }
        }
        catch (System.OperationCanceledException)
        {
        }
    }

    private void Update()
    {
        // 아무 키나 입력되면 다음 씬으로 전환.
        if (Utils.IsAnyInputDown())
        {
            SceneLoader.Instance.LoadScene(nextScene);
        }
    }
    
    private void OnDestroy()
    {
        // 오브젝트 파괴 및 씬 전환 시 실행 중인 모든 비동기 작업 취소 및 메모리 해제.
        cts?.Cancel();
        cts?.Dispose();
    }
}

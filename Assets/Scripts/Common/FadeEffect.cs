using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;

public static class FadeEffect
{
    /// <summary>
    /// TextMeshProUGUI의 알파값을 일정 시간 동안 보간하여 변경하는 메소드.
    /// </summary>
    /// <param name="target">대상 UI 텍스트</param>
    /// <param name="start">시작 투명도</param>
    /// <param name="end">목표 투명도</param>
    /// <param name="fadeTime">지속 시간</param>
    public static async UniTask Fade(TextMeshProUGUI target, float start, float end, float fadeTime = 1f)
    {
        if (target == null)
        {
            return;
        }

        float percent = 0;

        // 목표 시간에 도달할 때까지 루프.
        while (percent < 1)
        {
            // 프레임 독립적인 진행률 계산.
            percent += Time.deltaTime / fadeTime;

            // 색상의 알파 값 변경 및 적용.
            Color color = target.color;
            color.a = Mathf.Lerp(start, end, percent);
            target.color = color;

            // 다음 Update 타이밍까지 대기.
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }
}

using UnityEngine;

public static class Logger
{
    // 프로젝트 설정에 DEBUG라는 단어가 등록되어 있을 때만 이 함수를 실행.
    [System.Diagnostics.Conditional("DEBUG")]
    public static void Log(object message)
    {
        Debug.Log(message);
    }
}

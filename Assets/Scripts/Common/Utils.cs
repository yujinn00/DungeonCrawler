using UnityEngine;
using UnityEngine.InputSystem;

public static class Utils
{
    /// <summary>
    /// 현재 위치에서 목표 위치를 바라보는 2D 회전값을 구하는 메소드.
    /// </summary>
    /// <param name="owner">회전할 주체의 현재 위치</param>
    /// <param name="target">바라볼 목표 지점의 위치</param>
    /// <param name="weight">각도 보정값</param>
    /// <returns>목표 지점을 향하는 Z축 회전값</returns>
    public static Quaternion RotateToTarget(Vector2 owner, Vector2 target, float weight = 0)
    {
        float dx = target.x - owner.x;
        float dy = target.y - owner.y;
        
        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        
        return Quaternion.Euler(0f, 0f, degree - weight);
    }
    
    /// <summary>
    /// 각도를 기준으로 원의 둘레 위치를 구하는 메소드.
    /// </summary>
    /// <param name="radius">원의 반지름</param>
    /// <param name="angle">각도</param>
    /// <returns>원의 반지름, 각도에 해당하는 둘레 위치</returns>
    public static Vector3 GetPositionFromAngle(float radius, float angle)
    {
        Vector3 position = Vector3.zero;

        angle = DegreeToRadian(angle);

        position.x = Mathf.Cos(angle) * radius;
        position.y = Mathf.Sin(angle) * radius;

        return position;
    }

    /// <summary>
    /// Degree 값을 Radian 값으로 변환하는 메소드.
    /// </summary>
    /// <param name="angle">각도</param>
    /// <returns>변환된 라디안 값</returns>
    public static float DegreeToRadian(float angle)
    {
        return Mathf.PI * angle / 180;
    }

    /// <summary>
    /// 플랫폼 별로 아무 키가 눌렸는지 검사하는 메소드.
    /// </summary>
    /// <returns>클릭 여부</returns>
    public static bool IsAnyInputDown()
    {
        // 키보드 키 입력.
        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            return true;
        }
        
        // 마우스 왼쪽 버튼 입력.
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            return true;
        }
        
        // 모바일 터치.
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            return true;
        }

        return false;
    }
}

using UnityEngine;

public static class Utils
{
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
}

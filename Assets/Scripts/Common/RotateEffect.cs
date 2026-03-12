using UnityEngine;

public class RotateEffect : MonoBehaviour
{
    // 회전 속도.
    [SerializeField] private float rotateSpeed = 100;
    // 회전 여부.
    [SerializeField] private bool isPlay = true;

    private void Update()
    {
        if (!isPlay)
        {
            return;
        }

        // Z축을 기준으로 매 프레임 일정하게 회전.
        transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
    }
}

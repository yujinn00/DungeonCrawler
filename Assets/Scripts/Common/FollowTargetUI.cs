using UnityEngine;

public class FollowTargetUI : MonoBehaviour
{
    // UI가 추적할 월드 상의 대상.
    [SerializeField] private Transform target;
    
    // UI의 위치를 조절할 컴포넌트.
    private RectTransform rectTransform;
    // 월드 좌표를 화면 좌표로 변환하기 위한 메인 카메라.
    private Camera mainCamera;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        mainCamera = Camera.main;
    }

    public void Setup(Transform target)
    {
        this.target = target;
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        
        // 월드 좌표를 화면 상의 좌표로 변환하여 UI 위치 갱신.
        rectTransform.position = mainCamera.WorldToScreenPoint(target.position);
    }
}

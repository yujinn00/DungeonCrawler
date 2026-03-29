using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    // 컨트롤러의 배경.
    [SerializeField] private RectTransform rectBackground;
    // 터치 정보에 따라 위치가 바뀌는 가상 컨트롤러.
    [SerializeField] private RectTransform rectController;
    
    // 터치 위치를 외부로 보내기 위해 지역 변수가 아닌 멤버 변수로 선언.
    private Vector2 touchPosition;
    
    // x, y 방향 값을 외부에서 열람할 수 있도록 Get 전용 프로퍼티 정의.
    public float Horizontal => touchPosition.x;
    public float Vertical => touchPosition.y;

    private void Awake()
    {
        rectBackground.gameObject.SetActive(false);
    }

    /// <summary>
    /// 터치하는 순간 1회 호출하는 메소드.
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        rectBackground.gameObject.SetActive(true);
        
        // 터치한 위치로 가상 컨트롤러 위치를 변경.
        rectBackground.transform.position = eventData.position;
    }

    /// <summary>
    /// 터치 상태로 드래그할 때 매 프레임 호출되는 메소드.
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        touchPosition = Vector2.zero;
        
        // 화면을 터치한 위치가 Background 오브젝트로부터 얼마나 떨어져 있는지 계산해 touchPosition에 저장.
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectBackground, eventData.position, eventData.pressEventCamera, out touchPosition))
        {
            // touchPosition 값 연산.
            // touchPosition를 가상 컨트롤러 배경 크기로 나누고 2를 곱함.
            touchPosition.x = (touchPosition.x / rectBackground.sizeDelta.x * 2);
            touchPosition.y = (touchPosition.y / rectBackground.sizeDelta.y * 2);
            
            // touchPosition 값 정규화.
            // 현재 터치 위치가 가상 컨트롤러 배경 바깥일 때 -1 ~ 1보다 큰 값이 나오기 때문에 normailzed를 이용해 -1 ~ 1 사이의 값으로 정규화.
            touchPosition = (touchPosition.magnitude > 1) ? touchPosition.normalized : touchPosition;
            
            // 가상 조이스틱 컨트롤러 이동
            rectController.anchoredPosition = new Vector2(
                touchPosition.x * rectBackground.sizeDelta.x * 0.5f,
                touchPosition.y * rectBackground.sizeDelta.y * 0.5f);
        }
    }

    /// <summary>
    /// 터치를 종료하는 순간 1회 호츨하는 메소드.
    /// </summary>
    public void OnPointerUp(PointerEventData eventData)
    {
        // 터치 종료 시 이미지의 위치를 중앙으로 이동.
        rectController.anchoredPosition = Vector2.zero;
        // 터치 종료 시 touchPosition 값도 (0, 0)으로 초기화.
        touchPosition = Vector2.zero;
        
        rectBackground.gameObject.SetActive(false);
    }
}

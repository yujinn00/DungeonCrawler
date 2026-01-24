using UnityEngine;
using TMPro;

public class UIDamageText : MonoBehaviour
{
    // 텍스트가 화면에 머무르는 시간.
    [SerializeField] private float arriveTime = 0.5f;

    // 사라지는 연출 진행도.
    private float percent = 0;
    // 텍스트를 위로 이동시키는 물리 이동 컴포넌트 참조 변수.
    private MovementRigidbody2D movementRigidbody2D;
    // 실제 글자를 화면에 출력하는 TextMeshPro 컴포넌트 참조 변수.
    private TextMeshPro text;

    public void Setup(string text, Color color)
    {
        // 텍스트가 튀어 오르는 연출 설정.
        movementRigidbody2D = GetComponent<MovementRigidbody2D>();
        movementRigidbody2D.MoveTo(new Vector3(Random.Range(-1f, 1f), 1, 0));
        
        // 데미지 수치 및 색상 설정.
        this.text = GetComponent<TextMeshPro>();
        this.text.text = text;
        this.text.color = color;
        
        Destroy(gameObject, arriveTime);
    }

    private void Update()
    {
        if (percent > 1)
        {
            return;
        }
        
        // 시간이 지남에 따라 percent 값을 0에서 1로 증가시킴.
        percent += Time.deltaTime / arriveTime;
        
        // 텍스트의 투명도를 조절하여 서서히 사라지는 페이드 아웃 효과를 줌.
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1 - percent);
    }
}

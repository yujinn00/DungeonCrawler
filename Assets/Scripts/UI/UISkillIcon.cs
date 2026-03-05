using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISkillIcon : MonoBehaviour
{
    // 스킬의 이미지를 표시하는 UI 컴포넌트.
    [SerializeField] private Image skillIcon;
    // 스킬의 현재 레벨을 표시하는 텍스트 컴포넌트.
    [SerializeField] private TextMeshProUGUI skillLevel;

    public void Setup(Sprite defaultSprite)
    {
        // 아이콘 이미지를 비활성 상태의 스프라이트로 설정.
        skillIcon.sprite = defaultSprite;
        // 아직 배우지 않았으므로 레벨 텍스트를 -로 표시.
        skillLevel.text = "-";
    }

    public void LevelUp(int currentLevel, Sprite activeSprite)
    {
        // 아이콘 이미지를 활성 상태의 스프라이트로 변경.
        skillIcon.sprite = activeSprite;
        // 현재 레벨 숫자를 문자열로 변환하여 텍스트에 출력.
        skillLevel.text = currentLevel.ToString();
    }
}

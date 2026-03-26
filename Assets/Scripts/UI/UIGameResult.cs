using UnityEngine;
using TMPro;

public class UIGameResult : MonoBehaviour
{
    // 게임의 전반적인 상태를 제어하는 컨트롤러.
    [SerializeField] private GameController gameController;
    // 결과 화면 전체를 담고 있는 부모 패널 오브젝트.
    [SerializeField] private GameObject panelResult;
    // 게임 결과 문구를 표시할 텍스트.
    [SerializeField] private TextMeshProUGUI textResult;
    // 최종 도달한 스테이지 번호를 표시할 텍스트.
    [SerializeField] private TextMeshProUGUI textStage;

    public void OnResult(bool isClear, int stage)
    {
        // 결과창 패널 활성화.
        panelResult.SetActive(true);

        // 클리어 여부에 따른 결과 문구 설정.
        if (isClear == true)
        {
            textResult.text = "게임 클리어";
        }
        else
        {
            textResult.text = "게임 오버";
        }
        
        // 스테이지 번호를 문자열로 변환하여 출력.
        textStage.text = stage.ToString();
    }

    public void ButtonEvent_ReturnTitle()
    {
        // 멈춰있던 게임 시간을 정상 속도로 복구.
        gameController.SetTimeScale(1);
        // 인트로 씬으로 전환.
        SceneLoader.Instance.LoadScene(SceneNames.Intro);
    }
}

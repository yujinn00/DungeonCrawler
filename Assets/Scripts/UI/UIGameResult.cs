using UnityEngine;
using TMPro;

public class UIGameResult : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private GameObject panelResult;
    [SerializeField] private TextMeshProUGUI textResult;
    [SerializeField] private TextMeshProUGUI textStage;

    public void OnResult(bool isClear, int stage)
    {
        panelResult.SetActive(true);

        if (isClear == true)
        {
            textResult.text = "게임 클리어";
        }
        else
        {
            textResult.text = "게임 오버";
        }
        
        textStage.text = stage.ToString();
    }

    public void ButtonEvent_ReturnTitle()
    {
        gameController.SetTimeScale(1);
        SceneLoader.Instance.LoadScene(SceneNames.Intro);
    }
}

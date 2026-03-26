using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    // 몬스터 소환을 담당하는 스포너 참조.
    [SerializeField] private EnemySpawner enemySpawner;
    // 현재 스테이지 번호를 화면에 표시할 월드 공간용 텍스트 컴포넌트.
    [SerializeField] private TextMeshPro textStageNumber;
    // asdf.
    [SerializeField] private UIGameResult uiGameResult;
    // 스테이지가 올라갈 때마다 추가될 적 수의 가중치.
    [SerializeField] private float enemyCountScale = 0.15f;

    // 현재 진행 중인 스테이지 번호.
    private int currentStage = 0;
    // 각 스테이지에서 생성할 몬스터의 수.
    private int enemyCount = 10;

    private void Start()
    {
        // 현재 스테이지에 남아있는 몬스터의 숫자가 0이면 다음 스테이지로 넘어가도록 설정.
        EnemySpawner.exitEvent.AddListener(SetupStage);
        
        // 게임 시작 시 스테이지 설정 실행.
        SetupStage();
    }

    public void SetupStage()
    {
        // 다음 스테이지로 번호 증가.
        currentStage++;

        if (currentStage > 20)
        {
            GameClear();
            return;
        }
        
        // 맵에 출력하는 currentStage Text UI 갱신.
        textStageNumber.text = $"STAGE {currentStage:D2}";
        // 스포너를 통해 설정된 수만큼 몬스터 생성 프로세스 시작.
        enemySpawner.SpawnEnemys((int)(enemyCount + currentStage * enemyCountScale));
    }

    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    public void GameClear()
    {
        SetTimeScale(0);
    }
    
    public void GameOver()
    {
        SetTimeScale(0);
    }
}

using UnityEngine;

public class GameController : MonoBehaviour
{
    // 몬스터 소환을 담당하는 스포너 참조.
    [SerializeField] private EnemySpawner enemySpawner;

    // 현재 스테이지에서 생성할 몬스터의 수.
    private int enemyCount = 10;

    private void Start()
    {
        // 게임 시작 시 스테이지 설정 실행.
        SetupStage();
    }

    public void SetupStage()
    {
        // 스포너를 통해 설정된 수만큼 몬스터 생성 프로세스 시작.
        enemySpawner.SpawnEnemys(enemyCount);
    }

    /// <summary>
    /// 게임의 흐름 속도를 조절하는 메소드.
    /// </summary>
    /// <param name="scale">시간 배율</param>
    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }
}

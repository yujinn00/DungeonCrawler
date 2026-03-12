using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Cysharp.Threading.Tasks;

public enum SceneNames { Intro = 0, Game }

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    // 로딩 UI 부모 객체.
    [SerializeField] private GameObject loadingScreen;
    // 로딩 배경 이미지.
    [SerializeField] private Image loadingBackground;
    // 랜덤 배경 이미지 목록.
    [SerializeField] private Sprite[] loadingSprites;
    // 진행도 바.
    [SerializeField] private Slider loadingProgress;
    // 진행도 퍼센트 텍스트.
    [SerializeField] private TextMeshProUGUI textProgress;

    private void Awake()
    {
        // 싱글톤 초기화 및 씬 전환 시 파괴 방지.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary>
    /// 외부에서 씬 전환을 요청할 때 호출하는 메소드.
    /// </summary>
    /// <param name="name"></param>
    public void LoadScene(SceneNames name)
    {
        // 비동기 실행 후 관리를 위임.
        LoadSceneAsync(name.ToString()).Forget();
    }

    /// <summary>
    /// 실제 비동기 씬 로드 및 UI 업데이트 메소드.
    /// </summary>
    /// <param name="name">씬 이름</param>
    private async UniTaskVoid LoadSceneAsync(string name)
    {
        // 로딩 화면 설정.
        int index = Random.Range(0, loadingSprites.Length);
        loadingBackground.sprite = loadingSprites[index];
        loadingProgress.value = 0f;
        loadingScreen.SetActive(true);
        
        // 씬 로드 시작.
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name);
        
        // 로딩이 끝날 때까지 UI 갱신.
        while (!asyncOperation.isDone)
        {
            // 진행도 출력.
            loadingProgress.value = asyncOperation.progress;
            textProgress.text = $"{Mathf.RoundToInt(asyncOperation.progress * 100)}%";
            
            // 다음 프레임까지 대기.
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
        
        // 로딩 완료 후 0.5초 대기.
        await UniTask.Delay(500);

        // 로딩 화면 비활성화.
        loadingScreen.SetActive(false);
    }
}

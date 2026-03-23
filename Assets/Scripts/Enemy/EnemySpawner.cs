using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Cysharp.Threading.Tasks;
using System.Threading;

public class EnemySpawner : MonoBehaviour
{
    // 몬스터가 생성될 기준이 되는 타일맵.
    [SerializeField] private Tilemap tilemap;
    // 몬스터가 생성될 위치를 표시할 타일.
    [SerializeField] private GameObject enemySpawnTile;
    // 생성할 몬스터 캐릭터들의 프리팹 배열.
    [SerializeField] private GameObject[] enemyPrefabs;
    // 생성된 몬스터의 HP UI가 자식으로 들어갈 부모 캔버스.
    [SerializeField] private Transform parentTransform;
    // 몬스터가 추적하고 공격할 대상.
    [SerializeField] private EntityBase target;

    // 타일의 중심점에 맞추기 위한 보정값 (타일은 보통 좌측 하단이 기준점이기 때문).
    private Vector3 offset = new Vector3(0.5f, 0.5f, 0);
    // 몬스터가 생성 가능한 타일들의 월드 좌표를 저장하는 리스트.
    private List<Vector3> possibleTiles = new List<Vector3>();
    // 스폰 위치를 미리 알려주는 경고 타일 전용 메모리 풀.
    private MemoryPool enemySpawnTilePool;
    
    // 오브젝트 파괴 시 진행 중인 작업을 취소하기 위한 소스.
    private CancellationTokenSource cts;
    
    // 현재 맵에 존재하는 모든 몬스터들의 정보를 관리하는 정적 리스트.
    public static List<EntityBase> Enemies { get; private set; } = new List<EntityBase>();

    private void Awake()
    {
        // 경고용 스폰 타일을 관리할 메모리 풀 초기화.
        enemySpawnTilePool = new MemoryPool(enemySpawnTile);
        
        // 타일맵의 빈 공간을 제거하여 범위를 실제 타일이 있는 곳으로 압축.
        tilemap.CompressBounds();
        // 스폰 가능한 위치 정보를 미리 계산하여 리스트에 저장.
        CalculatePossibleTiles();
    }
    
    private void OnDestroy()
    {
        // 씬이 바뀌거나 스포너가 파괴되면 스폰 프로세스 중단.
        cts?.Cancel();
        cts?.Dispose();
    }

    public void SpawnEnemys(int count)
    {
        Enemies.Clear();
        
        // 이전 작업이 있다면 취소하고 새로 시작
        cts?.Cancel();
        cts?.Dispose();
        cts = new CancellationTokenSource();

        // UniTaskVoid로 비동기 프로세스 시작.
        Process(count, cts.Token).Forget();
    }

    private async UniTaskVoid Process(int count, CancellationToken token)
    {
        try
        {
            Vector3[] positions = new Vector3[count];
            for (int i = 0; i < count; ++i)
            {
                // 몬스터를 배치할 임의의 위치 설정.
                positions[i] = possibleTiles[Random.Range(0, possibleTiles.Count)];
                // 몬스터가 배치될 위치에 타일 생성.
                enemySpawnTilePool.ActivatePoolItem(positions[i]);
            }
            
            // 2초 대기.
            await UniTask.Delay(2000, cancellationToken: token);
            
            // 모든 타일 삭제.
            enemySpawnTilePool.DeactivateAllPoolItems();

            for (int i = 0; i < count; ++i)
            {
                // 어떤 종류의 몬스터을 생성할지 랜덤 결정.
                int type = Random.Range(0, enemyPrefabs.Length);
                
                // 선택된 타일 위치에 몬스터 오브젝트 생성 (부모를 이 스포너로 설정).
                GameObject clone = Instantiate(enemyPrefabs[type], positions[i], Quaternion.identity, transform);
                // 몬스터 초기화 함수 호출 및 HP UI가 부착될 캔버스 정보 전달.
                clone.GetComponent<EnemyBase>().Initialize(this, parentTransform);
                // 몬스터의 AI에 타겟 정보를 직접 전달하여 그래프 변수 초기화.
                clone.GetComponent<EnemyAI>().Setup(target);
                
                // 생성된 몬스터을 관리 리스트에 등록.
                Enemies.Add(clone.GetComponent<EntityBase>());
            }
        }
        catch (System.OperationCanceledException)
        {
            
        }
    }

    /// <summary>
    /// 타일맵을 전수 조사하여 타일이 존재하는 좌표를 리스트에 담는 함수.
    /// </summary>
    private void CalculatePossibleTiles()
    {
        // 타일맵의 전체 영역 범위.
        BoundsInt bounds = tilemap.cellBounds;
        // 해당 영역 내 모든 타일 데이터를 배열로 가져옴.
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        // 맵의 가장자리를 제외하고 안쪽 타일들만 검사.
        for (int y = 1; y < bounds.size.y - 1; ++y)
        {
            for (int x = 1; x < bounds.size.x - 1; ++x)
            {
                // 타일 데이터 배열에서 현재 위치의 타일을 가져옴.
                TileBase tile = allTiles[y * bounds.size.x + x];

                if (tile != null)
                {
                    // 로컬 좌표(Cell)를 계산.
                    Vector3Int localPosition = bounds.position + new Vector3Int(x, y);
                    // 로컬 좌표(Cell)를 월드 좌표로 변환하고 중심점 오프셋을 더함.
                    Vector3 position = tilemap.CellToWorld(localPosition) + offset;
                    // 2D 게임이므로 Z축은 0으로 고정.
                    position.z = 0;
                    
                    // 스폰 가능한 위치 리스트에 추가.
                    possibleTiles.Add(position);
                }
            }
        }
    }

    /// <summary>
    /// 몬스터가 사망할 때 호출하는 메소드.
    /// </summary>
    /// <param name="enemy">몬스터</param>
    public void Deactivate(EntityBase enemy)
    {
        Enemies.Remove(enemy);
        Destroy(enemy.gameObject);
    }
}

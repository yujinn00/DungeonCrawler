using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    // 적이 생성될 기준이 되는 타일맵.
    [SerializeField] private Tilemap tilemap;
    // 생성할 적 캐릭터들의 프리팹 배열.
    [SerializeField] private GameObject[] enemyPrefabs;
    // 생성된 적의 HP UI가 자식으로 들어갈 부모 캔버스.
    [SerializeField] private Transform parentTransform;
    // 몬스터가 추적하고 공격할 대상.
    [SerializeField] private EntityBase target;
    // 총 생성할 적의 수.
    [SerializeField] private int enemyCount;

    // 타일의 중심점에 맞추기 위한 보정값 (타일은 보통 좌측 하단이 기준점이기 때문).
    private Vector3 offset = new Vector3(0.5f, 0.5f, 0);
    // 적이 스폰 가능한 타일들의 월드 좌표를 저장하는 리스트.
    private List<Vector3> possibleTiles = new List<Vector3>();
    
    // 현재 맵에 존재하는 모든 적들의 정보를 관리하는 정적 리스트.
    public static List<EntityBase> Enemies { get; private set; } = new List<EntityBase>();

    private void Awake()
    {
        // 타일맵의 빈 공간을 제거하여 범위를 실제 타일이 있는 곳으로 압축.
        tilemap.CompressBounds();
        // 스폰 가능한 위치 정보를 미리 계산하여 리스트에 저장.
        CalculatePossibleTiles();

        for (int i = 0; i < enemyCount; ++i)
        {
            // 어떤 종류의 적을 생성할지 랜덤 결정.
            int type = Random.Range(0, enemyPrefabs.Length);
            // 미리 저장해둔 가능한 타일 좌표 중 하나를 랜덤 결정.
            int index = Random.Range(0, possibleTiles.Count);
            
            // 선택된 타일 위치에 적 오브젝트 생성 (부모를 이 스포너로 설정).
            GameObject clone = Instantiate(enemyPrefabs[type], possibleTiles[index], Quaternion.identity, transform);
            // 적 초기화 함수 호출 및 HP UI가 부착될 캔버스 정보 전달.
            clone.GetComponent<EnemyBase>().Initialize(this, parentTransform);
            // 적의 AI에 타겟 정보를 직접 전달하여 그래프 변수 초기화.
            clone.GetComponent<EnemyAI>().Setup(target);
            
            // 생성된 적을 관리 리스트에 등록.
            Enemies.Add(clone.GetComponent<EntityBase>());
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
}

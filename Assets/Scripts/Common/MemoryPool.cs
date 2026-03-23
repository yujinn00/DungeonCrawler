using System.Collections.Generic;
using UnityEngine;

public class MemoryPool
{
    // 메모리 풀 내부에서 관리할 개별 오브젝트의 상태 정보.
    private class PoolItem
    {
        // 실제 씬에 생성된 게임 오브젝트.
        public GameObject gameObject;
        // 현재 사용 중인지 여부.
        private bool isActive;

        // 데이터 변경 시 실제 게임 오브젝트의 활성화 상태를 동기화하는 프로퍼티.
        public bool IsActive
        {
            set
            {
                isActive = value;
                gameObject.SetActive(isActive);
            }
            get => isActive;
        }
    }

    // 복제하여 사용할 원본 프리팹.
    private GameObject poolObject;
    // 생성된 객체들을 보관하는 리스트.
    private List<PoolItem> poolItemList;

    // 오브젝트가 부족할 때 Instantiate() 함수로 한 번에 추가 생성할 단위 개수.
    private readonly int increaseCount = 5;

    // 현재 리스트에 존재하는 전체 오브젝트 개수 (오브젝트 총량).
    public int MaxCount { private set; get; }
    // 현재 게임에서 사용 중인 오브젝트 개수 (대여 중).
    public int ActiveCount { private set; get; }

    // 비활성화된 오브젝트를 숨겨둘 임시 좌표 (월드 바깥).
    private Vector3 tempPosition = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

    /// <summary>
    /// 메모리 풀 생성자 메소드.
    /// 관리할 대상을 설정하고 초기 물량을 확보.
    /// </summary>
    public MemoryPool(GameObject poolObject)
    {
        MaxCount = 0;
        ActiveCount = 0;
        this.poolObject = poolObject;
        poolItemList = new List<PoolItem>();

        // 최초 5개를 생성하여 초기 오브젝트 확보.
        InstantiateObjects();
    }

    /// <summary>
    /// 부족한 물량을 보충하기 위해 오브젝트를 새로 생성하여 리스트에 저장하는 메소드.
    /// </summary>
    public void InstantiateObjects()
    {
        MaxCount += increaseCount;

        for (int i = 0; i < increaseCount; ++i)
        {
            PoolItem poolItem = new PoolItem();

            // 프리팹 복제 생성.
            poolItem.gameObject = GameObject.Instantiate(poolObject);
            // 임시 좌표로 이동.
            poolItem.gameObject.transform.position = tempPosition;
            // 오브젝트 비활성화.
            poolItem.IsActive = false;

            // 리스트에 저장.
            poolItemList.Add(poolItem);
        }
    }

    /// <summary>
    /// 현재 관리 중인 모든 오브젝트를 파괴하여 리스트를 비우는 메소드.
    /// </summary>
    public void DestroyObjects()
    {
        if (poolItemList == null)
        {
            return;
        }

        foreach (var item in poolItemList)
        {
            if (item.gameObject != null)
            {
                GameObject.Destroy(item.gameObject);
            }
        }

        poolItemList.Clear();
    }

    /// <summary>
    /// 리스트에서 대기 중인 오브젝트를 찾아 원하는 위치에 배치하고 활성화하는 메소드.
    /// 현재 모든 오브젝트가 사용 중이면 InstatiateObjects() 함수로 추가 생성.
    /// </summary>
    public GameObject ActivatePoolItem(Vector3 position)
    {
        if (poolItemList == null)
        {
            return null;
        }

        // 모든 오브젝트가 활성화 상태이면 새로운 오브젝트 추가 생성.
        if (MaxCount == ActiveCount)
        {
            InstantiateObjects();
        }

        foreach (var poolItem in poolItemList)
        {
            // 비활성 상태인 것을 찾으면 조건문 실행.
            if (poolItem.IsActive == false)
            {
                // 활성화 개수 증가.
                ActiveCount++;

                // 원하는 위치로 이동.
                poolItem.gameObject.transform.position = position;
                // 오브젝트 활성화.
                poolItem.IsActive = true;

                return poolItem.gameObject;
            }
        }

        return null;
    }

    /// <summary>
    /// 현재 사용이 완료된 오브젝트를 비활성화하여 다시 리스트로 회수하는 메소드.
    /// </summary>
    public void DeactivatePoolItem(GameObject removeObject)
    {
        if (poolItemList == null || removeObject == null)
        {
            return;
        }

        int count = poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if (poolItem.gameObject == removeObject)
            {
                // 활성화 개수 감소.
                ActiveCount--;

                // 오브젝트 비활성화.
                poolItem.IsActive = false;
                // 임시 좌표로 이동.
                poolItem.gameObject.transform.position = tempPosition;

                return;
            }
        }
    }

    /// <summary>
    /// 현재 게임에 사용 중인 모든 오브젝트를 한 번에 비활성화하여 리스트로 회수하는 메소드.
    /// </summary>
    public void DeactivateAllPoolItems()
    {
        if (poolItemList == null)
        {
            return;
        }

        foreach (var poolItem in poolItemList)
        {
            // 사용 중인 오브젝트들만 골라 회수.
            if (poolItem.gameObject != null && poolItem.IsActive)
            {
                // 오브젝트 비활성화.
                poolItem.IsActive = false;
                // 임시 좌표로 이동.
                poolItem.gameObject.transform.position = tempPosition;
            }
        }

        ActiveCount = 0;
    }
}

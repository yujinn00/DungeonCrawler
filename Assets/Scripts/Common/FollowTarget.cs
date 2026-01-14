using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool x, y, z;

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void Update()
    {
        if (target == null)
        {
            return;
        }
        
        // 활성화된 축은 타겟의 위치를 추적하고, 비활성화된 축은 현재 위치를 유지함.
        transform.position = new Vector3(
            (x ? target.position.x + offset.x : transform.position.x),
            (y ? target.position.y + offset.y : transform.position.y),
            (z ? target.position.z + offset.z : transform.position.z)
        );
    }
}

using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private bool x, y, z;

    private void Update()
    {
        if (target == null)
        {
            return;
        }
        
        // 활성화된 축은 타겟의 위치를 추적하고, 비활성화된 축은 현재 위치를 유지함.
        transform.position = new Vector3(
            (x ? target.position.x : transform.position.x),
            (y ? target.position.y : transform.position.y),
            (z ? target.position.z : transform.position.z)
        );
    }
}

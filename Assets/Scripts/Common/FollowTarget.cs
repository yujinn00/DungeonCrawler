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
        
        // 활성화된 축은 target의 위치, 비활성화된 축은 자기 자신의 위치로 설정.
        transform.position = new Vector3(
            (x ? target.position.x : transform.position.x),
            (y ? target.position.y : transform.position.y),
            (z ? target.position.z : transform.position.z)
        );
    }
}

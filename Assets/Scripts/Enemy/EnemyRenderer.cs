using UnityEngine;

public class EnemyRenderer : MonoBehaviour
{
    // 몬스터의 좌우 반전을 위한 Transform.
    [SerializeField] private Transform enemyModel;

    /// <summary>
    /// 몬스터의 이동 방향(X축)에 따라 모델을 좌우 반전시키는 메소드.
    /// </summary>
    /// <param name="x">몬스터의 X축 이동 입력 값</param>
    public void SpriteFlipX(float x)
    {
        // 현재 enemyModel 스케일 값을 가져옴.
        Vector3 currentScale = enemyModel.localScale;
        
        // x < 0 (왼쪽 이동): 스케일 X를 -1f로 설정하여 모델을 좌우 반전시킴.
        // x >= 0 (오른쪽 또는 정지): 스케일 X를 1f로 설정하여 모델을 기본 방향으로 유지함.
        currentScale.x = x < 0 ? -1f : 1f;
        
        // 변경된 스케일 값을 enemyModel 다시 적용함.
        enemyModel.localScale = currentScale;
    }
}

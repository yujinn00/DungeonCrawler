using UnityEngine;
using UnityEngine.UI;

public class UIHP : MonoBehaviour
{
    // 체력바 역할을 할 이미지.
    [SerializeField] private Image image;
    // 체력 데이터를 가지고 있는 대상.
    [SerializeField] private EntityBase entity;

    public void Setup(EntityBase entity)
    {
        this.entity = entity;
    }

    private void Update()
    {
        if (entity == null)
        {
            return;
        }
        
        // 매 프레임마다 [현재 체력 / 최대 체력] 비율을 계산하여 이미지의 채움 정도를 갱신.
        image.fillAmount = entity.Stats.currentHP / entity.Stats.maxHP;
    }
}

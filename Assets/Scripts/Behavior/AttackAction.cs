using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Attack", story: "Try Attack", category: "Action", id: "19d7c64004cc7d58eae32778823582e0")]
public partial class AttackAction : Action
{
    // 캐싱을 위한 변수
    private EnemyAI enemyAI;

    protected override Status OnStart()
    {
        // 에이전트(몬스터)에 붙어있는 EnemyAI 컴포넌트를 가져옴
        if (GameObject != null)
        {
            enemyAI = GameObject.GetComponent<EnemyAI>();
        }

        if (enemyAI == null)
        {
            LogFailure("EnemyAI component missing on the GameObject.");
            return Status.Failure;
        }

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (enemyAI == null) return Status.Failure;

        // EnemyAI의 TryAttack 호출
        // (쿨타임 체크는 TryAttack 내부에서 하므로 매 프레임 호출해도 안전함)
        enemyAI.TryAttack();

        // 공격 상태가 유지되어야 하므로 Running 반환
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}


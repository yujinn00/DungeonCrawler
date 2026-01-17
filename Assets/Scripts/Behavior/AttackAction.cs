using System;
using Unity.Behavior;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Attack", story: "Try Attack", category: "Action", id: "19d7c64004cc7d58eae32778823582e0")]
public partial class AttackAction : Action
{
    // 성능 최적화를 위해 EnemyAI 컴포넌트를 미리 저장해둘 캐싱 변수.
    private EnemyAI enemyAI;

    protected override Status OnStart()
    {
        if (GameObject != null)
        {
            // 몬스터에 붙어있는 EnemyAI 컴포넌트를 캐싱.
            enemyAI = GameObject.GetComponent<EnemyAI>();
        }

        if (enemyAI == null)
        {
            return Status.Failure;
        }

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (enemyAI == null)
        {
            return Status.Failure;
        }

        // 실제 공격 로직 호출.
        enemyAI.TryAttack();

        return Status.Running;
    }
}

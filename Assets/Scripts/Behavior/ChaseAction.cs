using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Chase", story: "[Self] Navigate to [Target]", category: "Action", id: "28ca7c12e031edb22a6b23bea6d135a7")]
public partial class ChaseAction : Action
{
    // 자기 자신.
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    // 추적할 타겟.
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    
    // 적의 길찾기 및 이동 처리를 위한 컴포넌트.
    private NavMeshAgent agent;
    // 추적할 타겟.
    private EntityBase target;
    // 원래 속도를 기억할 변수.
    private float originalSpeed;

    protected override Status OnStart()
    {
        agent = Self.Value.GetComponent<NavMeshAgent>();
        target =  Target.Value.GetComponent<EntityBase>();
        
        // 속도를 바꾸기 전에 원래 속도를 저장.
        originalSpeed = agent.speed;
        
        // 추적 속도로 변경.
        agent.speed = 4.0f;
        
        return Status.Running;
    }
    
    protected override Status OnUpdate()
    {
        // 타겟의 위치로 이동. 
        agent.SetDestination(target.MiddlePoint);

        return Status.Running;
    }

    protected override void OnEnd()
    {
        // 추적하기 전 속도로 원상복구.
        agent.speed = originalSpeed;
    }
}

using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "UpdateDistance", story: "Update [Self] and [Target] [CurrentDistance]", category: "Action", id: "3e51556c7fe4d4d2b2c07c42cc8969e4")]
public partial class UpdateDistanceAction : Action
{
    // 자기 자신.
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    // 추적할 타겟.
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    // 현재 거리.
    [SerializeReference] public BlackboardVariable<float> CurrentDistance;

    // 자기 자신.
    private EntityBase self;
    // 추적할 타겟.
    private EntityBase target;

    protected override Status OnStart()
    {
        self = Self.Value.GetComponent<EntityBase>();
        target = Target.Value.GetComponent<EntityBase>();

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        // 자기 자신과 추적할 타겟 사이의 거리 계산.
        CurrentDistance.Value = Vector2.Distance(self.MiddlePoint, target.MiddlePoint);
        
        return Status.Success;
    }
}

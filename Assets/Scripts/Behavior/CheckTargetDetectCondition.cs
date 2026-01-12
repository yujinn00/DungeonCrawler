using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CheckTargetDetect", story: "Compare values of [CurrentDistance] and [ChaseDistance]", category: "Conditions", id: "b579fb9994f28d8b5e3ee08f2c73d5b1")]
public partial class CheckTargetDetectCondition : Condition
{
    // 현재 거리.
    [SerializeReference] public BlackboardVariable<float> CurrentDistance;
    // 추적 거리.
    [SerializeReference] public BlackboardVariable<float> ChaseDistance;

    public override bool IsTrue()
    {
        // 현재 거리가 추적 거리보다 작거나 같아지면 타겟을 감지함.
        if (CurrentDistance.Value <= ChaseDistance.Value)
        {
            return true;
        }
        
        return false;
    }
}

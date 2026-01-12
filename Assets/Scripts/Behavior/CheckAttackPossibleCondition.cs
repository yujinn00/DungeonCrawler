using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CheckAttackPossible", story: "Compare values of [CurrentDistance] and [AttackDistance]", category: "Conditions", id: "7995f47676a91b50c8f3f764f7a1c2ac")]
public partial class CheckAttackPossibleCondition : Condition
{
    // 현재 거리.
    [SerializeReference] public BlackboardVariable<float> CurrentDistance;
    // 공격 거리.
    [SerializeReference] public BlackboardVariable<float> AttackDistance;

    public override bool IsTrue()
    {
        // 현재 거리가 공격 거리보다 작거나 같아지면 공격이 가능함.
        if (CurrentDistance.Value <= AttackDistance.Value)
        {
            return true;
        }
        
        return false;
    }
}

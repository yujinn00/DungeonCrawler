using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Patrol", story: "[Self] Navigate to PatrolPosition", category: "Action", id: "39d0b1e3ccdcd165dede8a73c6b28ef5")]
public partial class PatrolAction : Action
{
    // 자기 자신.
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    
    // 적의 길찾기 및 이동 처리를 위한 컴포넌트.
    private NavMeshAgent agent;
    // 이동할 목표 지점.
    private Vector3 patrolPosition;
    // 이동 시작 시간.
    private float currentPatrolTime = 0f;
    // 최대 이동 제한 시간.
    private float maxPatrolTime = 5f;

    protected override Status OnStart()
    {
        // 최소 각도.
        int jitterMin = 0;
        // 최대 각도.
        int jitterMax = 360;
        // 현재 위치를 원점으로 하는 원의 반지름 (2.5m ~ 6.0m 사이의 거리).
        float patrolRadius = UnityEngine.Random.Range(2.5f, 6.0f);
        // 최소 각도와 최대 각도 사이의 임의의 각도.
        int patrolJitter = UnityEngine.Random.Range(jitterMin, jitterMax);
        
        // 현재 내 위치를 기준으로, 랜덤한 거리와 각도만큼 떨어진 곳을 목표 지점으로 설정.
        patrolPosition = Self.Value.transform.position + Utils.GetPositionFromAngle(patrolRadius, patrolJitter);
        
        // NavMeshAgent에게 이동 명령 내리기.
        agent = Self.Value.GetComponent<NavMeshAgent>();
        agent.SetDestination(patrolPosition);
        
        // 이동 시작 시간 기록.
        currentPatrolTime = Time.time;
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        // 목적지에 거의 도착했거나, 시간이 너무 오래 지났다면 성공으로 처리.
        if ((patrolPosition - Self.Value.transform.position).sqrMagnitude < 0.1f
            || Time.time - currentPatrolTime > maxPatrolTime)
        {
            return Status.Success;
        }
        
        return Status.Running;
    }
}

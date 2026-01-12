using UnityEngine;
using UnityEngine.AI;
using Unity.Behavior;

public abstract class EnemyAI : MonoBehaviour
{
    protected EnemyBase owner;
    protected NavMeshAgent navMeshAgent;
    protected BehaviorGraphAgent behaviorAgent;

    // 자식 클래스에서 owner.Stats를 EnemyStats로 형변환해서 편하게 쓰기 위한 프로퍼티.
    protected EnemyStats Stats => owner.Stats as EnemyStats;
    // 공격 쿨타임 관리 변수.
    private float lastAttackTime;

    protected virtual void Awake()
    {
        owner = GetComponent<EnemyBase>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        behaviorAgent = GetComponent<BehaviorGraphAgent>();

        // 2D 탑다운 게임이므로 NavMeshAgent가 멋대로 회전하거나 눕지 않도록 설정.
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    public void Setup(EntityBase target)
    {
        // 코드 상의 타겟 설정.
        owner.Target = target;
        
        // Behavior Graph의 Blackboard 변수에도 타겟 등록.
        behaviorAgent.SetVariableValue("Target", target.gameObject);
    }

    public void TryAttack()
    {
        // 쿨타임 계산.
        float attackDelay = 1f / Stats.attackSpeed;
        if (Time.time < lastAttackTime + attackDelay)
        {
            return;
        }

        // 쿨타임 갱신.
        lastAttackTime = Time.time;

        // 실제 공격은 자식에게 맡김.
        PerformAttack();
    }

    // 쿨타임을 제외한 구체적인 공격 방식은 자식 클래스에서 정의하도록 강제함.
    public abstract void PerformAttack();
}

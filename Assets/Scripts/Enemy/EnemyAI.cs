using UnityEngine;
using UnityEngine.AI;
using Unity.Behavior;

public abstract class EnemyAI : MonoBehaviour
{
    protected EnemyBase owner;
    protected NavMeshAgent navMeshAgent;
    protected BehaviorGraphAgent behaviorAgent;
    protected EnemyRenderer enemyRenderer;

    // 공격 쿨타임 관리 변수.
    private float lastAttackTime;

    protected virtual void Awake()
    {
        owner = GetComponent<EnemyBase>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        behaviorAgent = GetComponent<BehaviorGraphAgent>();
        enemyRenderer = GetComponentInChildren<EnemyRenderer>();

        // 2D 탑다운 게임이므로 NavMeshAgent가 멋대로 회전하거나 눕지 않도록 설정.
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        
        // 몬스터끼리 겹치지 않게 회피 우선순위 설정.
        navMeshAgent.avoidancePriority = Random.Range(0, 100);
    }

    public void Setup(EntityBase target)
    {
        // 코드 상의 타겟 설정.
        owner.Target = target;
        
        // Behavior Graph의 Blackboard 변수에도 타겟 등록.
        behaviorAgent.SetVariableValue("Target", target.gameObject);
    }
    
    protected void Update()
    {
        if (enemyRenderer != null && navMeshAgent != null)
        {
            // NavMeshAgent가 실제로 이동하고 있는 방향과 속도를 가져옴.
            Vector3 velocity = navMeshAgent.velocity;
            
            // 몬스터를 좌우 반전 처리함.
            if (Mathf.Abs(velocity.x) > 0.01f)
            {
                // moveInput.x 값에 따라 스프라이트를 좌우 반전시킴.
                enemyRenderer.SpriteFlipX(velocity.x);
            }
        }
    }

    public void TryAttack()
    {
        // 쿨타임 계산.
        float attackDelay = 1f / owner.Stats.GetStat(StatType.AttackSpeed).Value;
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

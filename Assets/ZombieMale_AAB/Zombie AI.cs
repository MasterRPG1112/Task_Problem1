using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class ZombieAI : MonoBehaviour
{
    [SerializeField] private PlayerDummy player;
    [SerializeField] private float runStartDistance = 5.0f;
    [SerializeField] private float attackDistance = 1.8f;
    [SerializeField] private float walkSpeed = 1.5f;
    [SerializeField] private float runSpeed = 3.5f;
    [SerializeField] private float biteAnimationLength = 2.0f;

    private NavMeshAgent agent;
    private Animator animator;
    private bool isBiting = false;

    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int IsAttackingHash = Animator.StringToHash("IsAttacking");
    private static readonly int IsPlayerDeadHash = Animator.StringToHash("IsPlayerDead");

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.stoppingDistance = Mathf.Max(0.5f, attackDistance - 0.3f);
    }

    private void Start()
    {
        if (player == null)
        {
            player = FindFirstObjectByType<PlayerDummy>();
        }
    }

    private void Update()
    {
        if (player == null) return;

        if (player.isDead)
        {
            StopAndIdle();
            return;
        }

        if (isBiting) return;

        Vector3 zombiePos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 playerPos = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        float distance = Vector3.Distance(zombiePos, playerPos);

        if (distance <= attackDistance)
        {
            StartBiting();
        }
        else
        {
            ChasePlayer(distance);
        }
    }

    private void ChasePlayer(float distance)
    {
        agent.isStopped = false;

        if (distance > runStartDistance)
        {
            agent.speed = walkSpeed;
        }
        else
        {
            agent.speed = runSpeed;
        }

        agent.SetDestination(player.transform.position);

        animator.SetBool(IsAttackingHash, false);
        animator.SetFloat(SpeedHash, agent.velocity.magnitude);
    }

    private void StartBiting()
    {
        isBiting = true;

        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        Vector3 lookDir = (player.transform.position - transform.position).normalized;
        lookDir.y = 0;
        if (lookDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(lookDir);
        }

        animator.SetFloat(SpeedHash, 0f);
        animator.SetBool(IsAttackingHash, true);

        Invoke("FinishBiting", biteAnimationLength);
    }

    private void FinishBiting()
    {
        isBiting = false;
    }

    private void StopAndIdle()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        if (agent.hasPath) agent.ResetPath();

        animator.SetFloat(SpeedHash, 0f);
        animator.SetBool(IsAttackingHash, false);
        animator.SetBool(IsPlayerDeadHash, true);
    }
}
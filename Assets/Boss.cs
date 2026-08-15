using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    [Header("=== 보스 기본 스탯 ===")]
    public float maxHealth = 1000f;
    public float currentHealth;

    [Header("=== 이동 및 거리 세팅 ===")]
    public Transform player;
    public float attackDistance = 3f;
    public float walkSpeed = 3.5f;
    public float dashSpeed = 12f;
    public float dashDistance = 5f;

    [Header("=== 쫄따구 소환 세팅 ===")]
    public GameObject zombiePrefab;
    public int mobCount = 5;
    public float spawnRadius = 3f;

    [Header("=== 스킬 쿨타임 (초 단위) ===")]
    public float summonCooldown = 12f;
    public float slamCooldown = 7f;
    public float dashCooldown = 5f;

    private float lastSummonTime = -999f;
    private float lastSlamTime = -999f;
    private float lastDashTime = -999f;

    private NavMeshAgent agent;
    private Animator anim;
    private bool isAttacking = false;

    void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        if (player == null)
        {
            GameObject p = GameObject.FindWithTag("Player");
            if (p != null)
            {
                player = p.transform;
            }
            else
            {
                Debug.LogError("아 플레이어 태그 설정 안했잖아!!!");
            }
        }

        if (agent != null)
        {
            agent.speed = walkSpeed;
            agent.stoppingDistance = attackDistance;
            agent.updatePosition = true;
            agent.updateRotation = true;
        }

        StartCoroutine(BossPatternRoutine());
    }

    void Update()
    {
        if (player == null) return;

        KeepOnGround();

        if (!isAttacking)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance > attackDistance)
            {
                agent.isStopped = false;
                agent.speed = walkSpeed;
                agent.SetDestination(player.position);

                anim.SetFloat("Speed", agent.desiredVelocity.magnitude);
            }
            else
            {
                agent.isStopped = true;
                anim.SetFloat("Speed", 0f);

                Vector3 direction = (player.position - transform.position).normalized;
                direction.y = 0;
                if (direction != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
                }
            }
        }
    }

    void KeepOnGround()
    {
        if (agent != null && agent.isOnNavMesh)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 2f, NavMesh.AllAreas))
            {
                Vector3 targetPos = transform.position;
                targetPos.y = hit.position.y;
                transform.position = targetPos;
            }
        }
    }

    IEnumerator BossPatternRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.2f);

            if (player == null) continue;

            float distance = Vector3.Distance(transform.position, player.position);

            if (!isAttacking)
            {
                if (distance <= attackDistance + 1f)
                {
                    int chosenPattern = SelectNearPattern();
                    yield return StartCoroutine(ExecutePattern(chosenPattern));
                }
                else if (distance > attackDistance && distance <= 15f)
                {
                    int chosenPattern = SelectFarPattern();
                    if (chosenPattern != 0)
                    {
                        yield return StartCoroutine(ExecutePattern(chosenPattern));
                    }
                }
            }
        }
    }

    int SelectNearPattern()
    {
        List<int> availablePatterns = new List<int>();

        availablePatterns.Add(2);

        if (Time.time >= lastSummonTime + summonCooldown)
        {
            availablePatterns.Add(1);
        }

        if (Time.time >= lastSlamTime + slamCooldown)
        {
            availablePatterns.Add(3);
        }

        if (Time.time >= lastDashTime + dashCooldown)
        {
            availablePatterns.Add(4);
        }

        int randomIndex = Random.Range(0, availablePatterns.Count);
        return availablePatterns[randomIndex];
    }

    int SelectFarPattern()
    {
        List<int> farPatterns = new List<int>();

        if (Time.time >= lastSlamTime + slamCooldown)
        {
            farPatterns.Add(3);
        }

        if (Time.time >= lastDashTime + dashCooldown)
        {
            farPatterns.Add(4);
        }

        if (farPatterns.Count == 0) return 0;

        int randomIndex = Random.Range(0, farPatterns.Count);
        return farPatterns[randomIndex];
    }

    IEnumerator ExecutePattern(int patternIndex)
    {
        switch (patternIndex)
        {
            case 1:
                yield return StartCoroutine(PatternSummon());
                break;
            case 2:
                yield return StartCoroutine(PatternSwing());
                break;
            case 3:
                yield return StartCoroutine(PatternJumpAttack());
                break;
            case 4:
                yield return StartCoroutine(PatternDash());
                break;
        }
    }

    IEnumerator PatternSummon()
    {
        Debug.Log(">>> 보스: 잡몹 소환 패턴 시작!");
        isAttacking = true;
        lastSummonTime = Time.time;
        agent.isStopped = true;
        anim.SetFloat("Speed", 0f);

        LookAtPlayer();

        anim.SetTrigger("Summon");

        yield return new WaitForSeconds(1f);

        if (zombiePrefab != null)
        {
            float angleStep = 360f / mobCount;

            for (int i = 0; i < mobCount; i++)
            {
                float angle = i * angleStep * Mathf.Deg2Rad;
                Vector3 spawnOffset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * spawnRadius;
                Vector3 targetPos = transform.position + spawnOffset;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(targetPos, out hit, 4f, NavMesh.AllAreas))
                {
                    Instantiate(zombiePrefab, hit.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(zombiePrefab, targetPos, Quaternion.identity);
                }
            }
        }
        else
        {
            Debug.LogWarning("아 인스펙터 창에서 ZombiePrefab 안 넣었음 ㅋㅋㅋ");
        }

        yield return new WaitForSeconds(1.5f);
        isAttacking = false;
    }

    IEnumerator PatternSwing()
    {
        Debug.Log(">>> 보스: 휘두르기 공격!");
        isAttacking = true;
        agent.isStopped = true;
        anim.SetFloat("Speed", 0f);

        LookAtPlayer();

        anim.SetTrigger("Swing");

        yield return new WaitForSeconds(2f);
        isAttacking = false;
    }

    IEnumerator PatternJumpAttack()
    {
        Debug.Log(">>> 보스: 원거리 점프 찍기 패턴!");
        isAttacking = true;
        lastSlamTime = Time.time;
        agent.isStopped = true;
        anim.SetFloat("Speed", 0f);

        LookAtPlayer();

        anim.SetTrigger("Slam");

        Vector3 startPos = transform.position;
        Vector3 targetPos = player.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPos, out hit, 5f, NavMesh.AllAreas))
        {
            targetPos = hit.position;
        }

        yield return new WaitForSeconds(0.3f);

        float jumpDuration = 0.8f;
        float elapsedTime = 0f;

        while (elapsedTime < jumpDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / jumpDuration;
            Vector3 currentPos = Vector3.Lerp(startPos, targetPos, t);

            NavMeshHit groundHit;
            if (NavMesh.SamplePosition(currentPos, out groundHit, 3f, NavMesh.AllAreas))
            {
                currentPos.y = groundHit.position.y;
            }

            transform.position = currentPos;
            yield return null;
        }

        yield return new WaitForSeconds(1.2f);
        isAttacking = false;
    }

    IEnumerator PatternDash()
    {
        Debug.Log(">>> 보스: 돌진 공격!");
        isAttacking = true;
        lastDashTime = Time.time;

        LookAtPlayer();

        anim.SetTrigger("Dash");

        yield return new WaitForSeconds(0.2f);

        Vector3 dashTarget = transform.position + transform.forward * dashDistance;
        NavMeshHit hit;
        if (NavMesh.Raycast(transform.position, dashTarget, out hit, NavMesh.AllAreas))
        {
            dashTarget = hit.position;
        }

        agent.stoppingDistance = 0f;
        agent.speed = dashSpeed;
        agent.isStopped = false;
        agent.SetDestination(dashTarget);

        float dashTimer = 0f;
        while (dashTimer < 0.8f && agent.remainingDistance > 0.3f)
        {
            dashTimer += Time.deltaTime;
            anim.SetFloat("Speed", agent.velocity.magnitude);
            KeepOnGround();
            yield return null;
        }

        agent.isStopped = true;
        agent.speed = walkSpeed;
        agent.stoppingDistance = attackDistance;
        anim.SetFloat("Speed", 0f);

        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }

    void LookAtPlayer()
    {
        if (player == null) return;
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;
        Debug.Log("보스 남은 체력: " + currentHealth);
    }
}
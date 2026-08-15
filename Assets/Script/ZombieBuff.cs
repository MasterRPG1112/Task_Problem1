using UnityEngine;

public class ZombieBuff : MonoBehaviour
{
    [Header("킬 수 및 타이머 관리")]
    public int totalKillCount = 0;
    private int lastTankerKillThreshold = 0;
    public float noKillTimer = 0f;

    private void Update()
    {
        noKillTimer += Time.deltaTime;

        if (noKillTimer >= 90f)
        {
            ApplyIdleBuffToZombies();
            noKillTimer = 0f;
        }
    }

    public void OnZombieKilled()
    {
        totalKillCount++;
        noKillTimer = 0f;

        if (totalKillCount - lastTankerKillThreshold >= 50)
        {
            ApplyTankerBuffToZombies();
            lastTankerKillThreshold = totalKillCount;
        }
    }

    private void ApplyTankerBuffToZombies()
    {
        ZombieAI[] zombies = FindObjectsOfType<ZombieAI>();
        foreach (ZombieAI zombie in zombies)
        {
            zombie.maxHp += 20f;
            zombie.currentHp += 20f;
            zombie.attackDamage = Mathf.Max(1f, zombie.attackDamage - 2f);
        }
        Debug.Log("모든 좀비에게 탱커 버프 적용 완료!");
    }

    private void ApplyIdleBuffToZombies()
    {
        if (Random.value < 0.5f)
        {
            ApplyDealerBuffToZombies();
        }
        else
        {
            ApplyRunnerBuffToZombies();
        }
    }

    private void ApplyDealerBuffToZombies()
    {
        ZombieAI[] zombies = FindObjectsOfType<ZombieAI>();
        foreach (ZombieAI zombie in zombies)
        {
            zombie.maxHp = Mathf.Max(10f, zombie.maxHp - 10f);
            zombie.currentHp = Mathf.Min(zombie.currentHp, zombie.maxHp);
            zombie.attackDamage += 3f;
        }
        Debug.Log("모든 좀비에게 딜러 버프 적용 완료!");
    }

    private void ApplyRunnerBuffToZombies()
    {
        ZombieAI[] zombies = FindObjectsOfType<ZombieAI>();
        foreach (ZombieAI zombie in zombies)
        {
            zombie.maxHp = Mathf.Max(10f, zombie.maxHp - 10f);
            zombie.currentHp = Mathf.Min(zombie.currentHp, zombie.maxHp);
            zombie.walkSpeed += 0.5f;
            zombie.runSpeed += 1.0f;
        }
        Debug.Log("모든 좀비에게 러너 버프 적용 완료!");
    }
}
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [Header("참조")]
    public Transform player;
    public GameObject zombiePrefab;

    [Header("레이어 설정")]
    public LayerMask roadLayer;

    [Header("스폰 조건 설정")]
    public float minSpawnDistance = 5f;
    public float maxSpawnDistance = 15f;
    public float spawnInterval = 3f;

    [Header("개수 제한 설정")]
    public int maxZombieCount = 20;

    private float timer;

    void Update()
    {
        if (player == null || zombiePrefab == null) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;

            int currentZombieCount = FindObjectsOfType<ZombieAI>().Length;

            if (currentZombieCount < maxZombieCount)
            {
                TrySpawnZombieOnRoad();
            }
            else
            {
                Debug.Log($"[ZombieSpawner] 맵에 좀비가 이미 {currentZombieCount}마리 존재하여 더 이상 스폰하지 않습니다.");
            }
        }
    }

    void TrySpawnZombieOnRoad()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle.normalized * Random.Range(minSpawnDistance, maxSpawnDistance);
            Vector3 randomPoint = player.position + new Vector3(randomCircle.x, 0f, randomCircle.y);

            Vector3 rayStart = randomPoint + Vector3.up * 10f;
            RaycastHit hit;

            if (Physics.Raycast(rayStart, Vector3.down, out hit, 20f, roadLayer))
            {
                Instantiate(zombiePrefab, hit.point, Quaternion.identity);
                Debug.Log($"[ZombieSpawner] 도로 위에 좀비 스폰 완료! (위치: {hit.point})");
                break;
            }
        }
    }
}
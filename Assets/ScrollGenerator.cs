using UnityEngine;

public class ScrollGenerator : MonoBehaviour
{
    // 生成する巻物
    public GameObject scrollPrefab;

    // 巻物が出る間隔
    public float minSpawnInterval = 3f;
    public float maxSpawnInterval = 5f;

    private float timer = 0f;
    private float spawnInterval;

    void Start()
    {
        // 最初の出現時間をランダムに決める
        spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnScroll();

            // 次の出現時間をランダムに決める
            spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);

            timer = 0f;
        }
    }

    void SpawnScroll()
    {
        float spawnY = Random.Range(-1.5f, 1f);
        
        Vector3 spawnPosition = new Vector3(10f, spawnY, 0f);

        Instantiate(scrollPrefab, spawnPosition, Quaternion.identity);
    }
}
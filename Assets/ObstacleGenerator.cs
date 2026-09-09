using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    // 生成する障害物
    public GameObject[] obstacles;

    // 次の障害物が出るまでの最小・最大時間
    public float minSpawnInterval = 1.5f;
    public float maxSpawnInterval = 3f;

    // 次の障害物が出るまでの時間
    private float timer = 0f;

    // 次に障害物が出るまでの時間
    private float spawnInterval;

    void Start()
    {
        // 最初の出現間隔をランダムに決める
        spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnObstacle();

            // 次の出現間隔をランダムに決める
            spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);

            timer = 0f;
        }
    }

    void SpawnObstacle()
    {
        // RockかShurikenをランダムに選ぶ
        int randomIndex = Random.Range(0, obstacles.Length);

        // 障害物の種類によって出す高さを変える
        float spawnY;

        if (obstacles[randomIndex].name.Contains("Shuriken"))
        {
            // 手裏剣は空中から出す
            spawnY = -0.5f;
        }
        else
        {
            // Rockは地面から出す
            spawnY = -1.95f;
        }

        // 右側から障害物を出す
        Vector3 spawnPosition = new Vector3(10f, spawnY, 0f);

        // 障害物を生成
        Instantiate(obstacles[randomIndex], spawnPosition, Quaternion.identity);
    }
}
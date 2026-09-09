using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    // 障害物が左に進む最初の速さ
    public float moveSpeed = 5f;

    // ゲームオーバーになったか
    public bool isGameOver = false;

    // 時間経過による速度上昇
    public float speedIncrease = 0.3f;

    // 障害物の最大速度
    public float maxSpeed = 14f;

    void Update()
    {
        // ゲームオーバーなら動かさない
        if (isGameOver)
        {
            return;
        }

        // 時間が経つほど少しずつ速くする
        moveSpeed += speedIncrease * Time.deltaTime;

        // 最大速度を超えないようにする
        moveSpeed = Mathf.Min(moveSpeed, maxSpeed);

        // 障害物を左に移動させる
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

        // 画面の左側まで移動したら削除する
        if (transform.position.x < -12f)
        {
            Destroy(gameObject);
        }
    }
}
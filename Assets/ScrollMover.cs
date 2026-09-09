using UnityEngine;

public class ScrollMover : MonoBehaviour
{
    // 巻物が左に進む速さ
    public float moveSpeed = 5f;

    void Update()
    {
        // 巻物を左に移動させる
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

        // 画面の左側まで移動したら削除する
        if (transform.position.x < -12f)
        {
            Destroy(gameObject);
        }
    }
}
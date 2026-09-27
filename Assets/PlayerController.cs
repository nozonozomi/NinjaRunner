using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // ジャンプの強さ
    public float jumpForce = 8f;

    // 通常時のPlayerの大きさ
    private Vector3 normalScale;
    
    // しゃがんだときのPlayerの大きさ
    private Vector3 crouchScale;

    // PlayerのRigidbody 2D
    private Rigidbody2D rb;

    // 地面にいるかどうか
    private bool isGrounded;

    // GAME OVERの文字
    public GameObject gameOverText;

    // 「もう一度やりますか？」の文字
    public GameObject retryText;
            
    // 「はい」ボタン
    public GameObject yesButton;
            
    // 「いいえ」ボタン
    public GameObject noButton;

    // 「ゲーム終了」の文字
    public GameObject endText;

    // プレイ中のスコア表示
    public GameObject scoreText;
    
    // プレイ中のハイスコア表示
    public GameObject highScoreText;

    // GAME OVER時のスコア表示
    public GameObject gameOverScoreText;
    
    // GAME OVER時のハイスコア表示
    public GameObject gameOverHighScoreText;

    // ジャンプの効果音
    public AudioSource jumpSound;

    // ゲームオーバーの効果音
    public AudioSource gameOverSound;

    // 巻物を取ったときの効果音
    public AudioSource scrollSound;

    // ランキング
    public RankingManager rankingManager;


    void Start()
    {
        // Rigidbody 2Dを取得
        rb = GetComponent<Rigidbody2D>();
        
        // 通常時の大きさを保存
        normalScale = transform.localScale;
        
        // しゃがんだときは高さを半分にする
        crouchScale = new Vector3(normalScale.x, normalScale.y * 0.5f, normalScale.z);
    }

    void Update()
    {
        // Spaceキーを押したら、地面にいる場合だけジャンプ
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

             // ジャンプ音を再生
            jumpSound.Play();
        }
        
        // ↓キーを押している間はしゃがむ
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.localScale = crouchScale;
        }
        else
        {
            transform.localScale = normalScale;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Groundに触れたら地面にいる状態にする
        if (collision.gameObject.name == "Ground")
        {
            isGrounded = true;
        }

        // RockまたはShurikenにぶつかったらGAME OVER
        if (collision.gameObject.name.Contains("Rock") ||
            collision.gameObject.name.Contains("Shuriken"))
        {
            // GAME OVERの文字を表示
            gameOverText.SetActive(true);

            // プレイ中のスコア表示を消す
            scoreText.SetActive(false);
            
            // プレイ中のハイスコア表示を消す
            highScoreText.SetActive(false);

            ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
            
            // 自己ベストとランキングを保存
            scoreManager.GameOver();
            
            // 今回のスコア
            int currentScore = scoreManager.GetScore();
            gameOverScoreText.GetComponent<TMPro.TextMeshProUGUI>().text = "今回のスコア：" + currentScore;
            
            // 現在のプレイヤーの自己ベスト
            int highScore = scoreManager.GetPlayerHighScore();
            gameOverHighScoreText.GetComponent<TMPro.TextMeshProUGUI>().text = "ハイスコア：" + highScore;
            
            // スコア表示をON
            gameOverScoreText.SetActive(true);
            gameOverHighScoreText.SetActive(true);

            // ゲームオーバー音を再生
            gameOverSound.Play();

            // 「もう一度やりますか？」の文字を表示
            retryText.SetActive(true);

            // 「はい」ボタンを表示
            yesButton.SetActive(true);

            // 「いいえ」ボタンを表示
            noButton.SetActive(true);

            // Playerを止める
            rb.velocity = Vector2.zero;

            // Playerの物理演算を止める
            rb.simulated = false;

            // ランキングを表示
            rankingManager.ShowRanking();

            // 障害物の生成を止める
            FindObjectOfType<ObstacleGenerator>().enabled = false;
            
            // 巻物の生成を止める
            FindObjectOfType<ScrollGenerator>().enabled = false;
            
            // 画面上のすべての障害物を止める
            ObstacleMover[] obstacles = FindObjectsOfType<ObstacleMover>();
            
            foreach (ObstacleMover obstacle in obstacles)
            {
                obstacle.isGameOver = true;
            }
            
            // 画面上のすべての巻物を止める
            ScrollMover[] scrolls = FindObjectsOfType<ScrollMover>();
            
            foreach (ScrollMover scroll in scrolls)
            {
                scroll.enabled = false;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Groundから離れたら空中にいる状態にする
        if (collision.gameObject.name == "Ground")
        {
            isGrounded = false;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void EndGame()
    {
        // GAME OVER画面を消す
        gameOverText.SetActive(false);
        retryText.SetActive(false);
        yesButton.SetActive(false);
        noButton.SetActive(false);
        
        // 「ゲーム終了」を表示
        endText.SetActive(true);
    }

    // 巻物を取ったとき
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Contains("Scroll"))
        {
            // 10ポイント追加
            FindObjectOfType<ScoreManager>().AddScore(10);
            
            // 巻物を取った音を再生
            scrollSound.Play();

            // 巻物を消す
            Destroy(other.gameObject);
        }
    }
}
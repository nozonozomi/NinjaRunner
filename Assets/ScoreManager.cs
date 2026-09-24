using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    private int score = 0;
    private int bonusScore = 0;
    private bool isGameOver = false;
    private float startTime;

    public void StartGame()
    {
        score = 0;
        bonusScore = 0;
        startTime = Time.time;
        isGameOver = false;

        scoreText.text = "修行ポイント：0";

        // 現在のプレイヤーの自己ベストを表示
        int highScore = GetPlayerHighScore();
        highScoreText.text = "ハイスコア：" + highScore;
    }

    void Update()
    {
        if (isGameOver) return;

        score = Mathf.FloorToInt(Time.time - startTime) + bonusScore;
        scoreText.text = "修行ポイント：" + score;
    }

    public void AddScore(int amount)
    {
        bonusScore += amount;
    }

    public void GameOver()
    {
        isGameOver = true;

        // 自己ベストを更新
        SavePersonalBest();

        // ランキングを更新
        SaveRanking();
    }

    // =========================
    // 名前ごとの自己ベスト
    // =========================

    void SavePersonalBest()
    {
        string playerName = PlayerPrefs.GetString("PlayerName", "名無し");

        string key = "PersonalBest_" + playerName;

        int oldBest = PlayerPrefs.GetInt(key, 0);

        if (score > oldBest)
        {
            PlayerPrefs.SetInt(key, score);
            PlayerPrefs.Save();
        }
    }

    public int GetPlayerHighScore()
    {
        string playerName = PlayerPrefs.GetString("PlayerName", "名無し");

        string key = "PersonalBest_" + playerName;

        return PlayerPrefs.GetInt(key, 0);
    }

    // =========================
    // ランキング保存
    // =========================

    void SaveRanking()
    {
        string playerName = PlayerPrefs.GetString("PlayerName", "名無し");

        // 現在のプレイヤーの自己ベストを取得
        int personalBest = GetPlayerHighScore();

        // すでにランキングにいるか確認
        int samePlayerRank = -1;

        for (int i = 0; i < 5; i++)
        {
            string rankName = PlayerPrefs.GetString("RankName" + i, "");

            if (rankName == playerName)
            {
                samePlayerRank = i;
                break;
            }
        }

        // すでにランキングにいる場合
        if (samePlayerRank >= 0)
        {
            // 自己ベストが更新されていなければ何もしない
            int oldScore = PlayerPrefs.GetInt("RankScore" + samePlayerRank, -1);

            if (personalBest <= oldScore)
            {
                return;
            }

            // いったんランキングから削除
            for (int i = samePlayerRank; i < 4; i++)
            {
                string nextName = PlayerPrefs.GetString("RankName" + (i + 1), "");
                int nextScore = PlayerPrefs.GetInt("RankScore" + (i + 1), -1);

                PlayerPrefs.SetString("RankName" + i, nextName);
                PlayerPrefs.SetInt("RankScore" + i, nextScore);
            }

            PlayerPrefs.DeleteKey("RankName4");
            PlayerPrefs.DeleteKey("RankScore4");
        }

        // 新しい順位を探す
        int newRank = 5;

        for (int i = 0; i < 5; i++)
        {
            int rankScore = PlayerPrefs.GetInt("RankScore" + i, -1);

            if (personalBest > rankScore)
            {
                newRank = i;
                break;
            }
        }

        // TOP5に入る場合
        if (newRank < 5)
        {
            // 下の順位を1つずつずらす
            for (int i = 4; i > newRank; i--)
            {
                string oldName = PlayerPrefs.GetString("RankName" + (i - 1), "");
                int oldScore = PlayerPrefs.GetInt("RankScore" + (i - 1), -1);

                PlayerPrefs.SetString("RankName" + i, oldName);
                PlayerPrefs.SetInt("RankScore" + i, oldScore);
            }

            // 新しいプレイヤーを登録
            PlayerPrefs.SetString("RankName" + newRank, playerName);
            PlayerPrefs.SetInt("RankScore" + newRank, personalBest);

            PlayerPrefs.Save();
        }
    }

    public int GetScore()
    {
        return score;
    }
}
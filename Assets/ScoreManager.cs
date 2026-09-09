using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    // スコア表示
    public TextMeshProUGUI scoreText;

    // ハイスコア表示
    public TextMeshProUGUI highScoreText;

    // 現在のスコア
    private int score = 0;

    // 巻物などでもらえるボーナスポイント
    private int bonusScore = 0;

    // ゲームオーバーになったか
    private bool isGameOver = false;

    // ゲーム開始時刻
    private float startTime;

    // ゲーム開始
    public void StartGame()
    {
        score = 0;
        bonusScore = 0;
        startTime = Time.time;
        isGameOver = false;

        scoreText.text = "修行ポイント：0";

        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "ハイスコア：" + highScore;
    }

    void Update()
    {
        // ゲームオーバーならスコアを更新しない
        if (isGameOver)
        {
            return;
        }

        // STARTを押してからの経過時間+ボーナスをスコアにする
        score = Mathf.FloorToInt(Time.time - startTime) + bonusScore;

        // スコアを表示
        scoreText.text = "修行ポイント：" + score;
    }

    // 巻物を取ったときにポイントを追加
    public void AddScore(int amount)
    {
        bonusScore += amount;
    }

    // ゲームオーバーにする
    public void GameOver()
    {
        isGameOver = true;

        // ハイスコアを取得
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        // 今回のスコアがハイスコアを超えていたら保存
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
        }

        // 名前とスコアをランキングに保存
        SaveRanking();
    }
    
    // 名前ごとの自己ベストを保存してランキングを作る
    void SaveRanking()
    {
        // プレイヤーの名前を取得
        string playerName = PlayerPrefs.GetString("PlayerName", "名無し");
        
        // 同じ名前がすでにランキングにいるか確認
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
        
        // 同じ名前がいる場合
        if (samePlayerRank >= 0)
        {
            int oldScore = PlayerPrefs.GetInt("RankScore" + samePlayerRank, -1);
            
            // 今回のスコアが自己ベスト以下なら何もしない
            if (score <= oldScore)
            {
                return;
            }
            
            // 今回のスコアが自己ベストを更新したら、古い記録を削除
            for (int i = samePlayerRank; i < 4; i++)
            {
                string nextName = PlayerPrefs.GetString("RankName" + (i + 1), "");
                int nextScore = PlayerPrefs.GetInt("RankScore" + (i + 1), -1);
                
                PlayerPrefs.SetString("RankName" + i, nextName);
                PlayerPrefs.SetInt("RankScore" + i, nextScore);
            }
            
            // 最後の順位を空にする
            PlayerPrefs.DeleteKey("RankName4");
            PlayerPrefs.DeleteKey("RankScore4");
        }
        
        // 今回のスコアをランキングに追加
        int newRank = 5;
        
        for (int i = 0; i < 5; i++)
        {
            int rankScore = PlayerPrefs.GetInt("RankScore" + i, -1);
            
            if (score > rankScore)
            {
                newRank = i;
                break;
            }
        }
        
        // 5位以内に入った場合
        if (newRank < 5)
        {
            // 下の順位を後ろにずらす
            for (int i = 4; i > newRank; i--)
            {
                string oldName = PlayerPrefs.GetString("RankName" + (i - 1), "");
                int oldScore = PlayerPrefs.GetInt("RankScore" + (i - 1), -1);
                
                PlayerPrefs.SetString("RankName" + i, oldName);
                PlayerPrefs.SetInt("RankScore" + i, oldScore);
            }
            
            // 今回の自己ベストを保存
            PlayerPrefs.SetString("RankName" + newRank, playerName);
            PlayerPrefs.SetInt("RankScore" + newRank, score);
            
            PlayerPrefs.Save();
        }
    }

    // 現在のスコアを取得
    public int GetScore()
    {
        return score;
    }
}
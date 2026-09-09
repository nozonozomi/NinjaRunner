using UnityEngine;
using TMPro;

public class StartManager : MonoBehaviour
{
    // スタート画面
    public GameObject startPanel;

    // 障害物生成
    public ObstacleGenerator obstacleGenerator;

    // スコア
    public ScoreManager scoreManager;

    // 巻物生成
    public ScrollGenerator scrollGenerator;

    // BGM
    public AudioSource bgm;

    // StartBGM
    public AudioSource startBgm;

    // 名前入力欄
    public TMP_InputField nameInput;
    
    // プレイヤーの名前
    public string playerName;

    // ゲーム開始
    public void StartGame()
    {
        // 入力された名前を取得
        playerName = nameInput.text;

        // 名前を保存
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.Save();
        
        // スタート画面BGMを止める
        startBgm.Stop();
    
        // スタート画面を消す
        startPanel.SetActive(false);

        // 障害物生成を開始
        obstacleGenerator.enabled = true;

        // スコアを開始
        scoreManager.enabled = true;
        scoreManager.StartGame();

        // 巻物生成を開始
        scrollGenerator.enabled = true;

        // BGMを再生
        bgm.Play();
    }
}

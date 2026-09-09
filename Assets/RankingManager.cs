using UnityEngine;
using TMPro;

public class RankingManager : MonoBehaviour
{
    // ランキング表示
    public TextMeshProUGUI rankingText;

    // ランキングを表示
    public void ShowRanking()
    {
        string text = "修行ランキング\n";

        for (int i = 0; i < 5; i++)
        {
            string name = PlayerPrefs.GetString("RankName" + i, "");
            int score = PlayerPrefs.GetInt("RankScore" + i, -1);

            // データがある場合だけ表示
            if (score >= 0)
            {
                text += (i + 1) + "位　" + name + "　" + score + " pt\n";
            }
        }

        rankingText.text = text;

        // ランキングを表示
        rankingText.gameObject.SetActive(true);
    }
}
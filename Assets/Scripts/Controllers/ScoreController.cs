using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour {

    private int scoreValue;
    private int highScore;

    public TextMeshProUGUI scoreText;

    private static ScoreController scoreController;
    public static ScoreController Instance
    {
		get
		{
			if (!scoreController)
			{
				scoreController =
					FindObjectOfType(typeof(ScoreController)) as ScoreController;

				if (!scoreController)
				{
					Debug.LogError
						 ("No ScoreController script find on a GameObject");
				}
				else
				{
					scoreController.Init();
				}
			}
			return scoreController;
		}
    }

	void Init()
	{
        // Init score value
        scoreValue = 0;

        // Load in highscore if it exists
        if (PlayerPrefs.HasKey("HighScore"))
            highScore = PlayerPrefs.GetInt("HighScore");
        else
            highScore = 0;

        UpdateScore();
	}

    void UpdateScore()
    {
        scoreText.text = "Score: " + scoreValue;
    }

    public void AddScore(int incrementalChange)
    {
        scoreValue += incrementalChange;
        UpdateScore();
    }

    public void SaveScore()
    {
        if (scoreValue >= highScore)
            highScore = scoreValue;

        PlayerPrefs.SetInt("HighScore", highScore);
    }
}

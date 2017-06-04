using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour {

	// Singleton static instance of MenuManager
	public static MenuManager Instance;

    public ColourController colourController;
    public Fade fadeController;

    public TextMeshProUGUI highscoreText;

    private void Awake()
    {
        Instance = this;
        colourController = ColourController.Instance;

        if (PlayerPrefs.HasKey("HighScore"))
            highscoreText.text =
                "HighScore: " + PlayerPrefs.GetInt("HighScore").ToString();
        else
            highscoreText.text = "HighScore: 0";
    }

    public void StartButtonClicked()
    {
        fadeController.EndScene(1);
    }
}

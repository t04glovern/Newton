using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	// Singleton static instance of GameManager
	public static GameManager Instance;

    public Fade fadeController;
    public AudioSource mainMusicSource;
    public AudioClip deathClip;

    public ColourController colourController;
    public SpawnController spawnController;
    public CameraShakeController cameraShakeController;
    public ScoreController scoreController;
    public PlayerController playerController;
    public AudioController audioController;

	void Awake() 
    {
        Instance = this;
        colourController = ColourController.Instance;
        spawnController = SpawnController.Instance;
        scoreController = ScoreController.Instance;
        playerController = PlayerController.Instance;
        audioController = AudioController.Instance;
	}

    public void GameOver()
    {
        mainMusicSource.clip = deathClip;
        mainMusicSource.loop = false;
        mainMusicSource.Play();
        scoreController.SaveScore();
        fadeController.EndScene(0);
    }
}

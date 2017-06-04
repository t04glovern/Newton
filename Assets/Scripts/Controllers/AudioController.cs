using UnityEngine;

public class AudioController : MonoBehaviour {

    public AudioClip walkFootstepClip;
    public AudioClip runFootstepClip;
    public AudioClip jumpClip;
    public AudioClip fallingClip;
    public AudioClip bonkClip;
    public AudioClip scoreClip;

    public AudioSource effectSource;

	private static AudioController audioController;
	public static AudioController Instance
	{
		get
		{
			if (!audioController)
			{
				audioController =
					FindObjectOfType(typeof(AudioController)) as AudioController;

				if (!audioController)
				{
					Debug.LogError
						 ("No AudioController script find on a GameObject");
				}
				else
				{
					audioController.Init();
				}
			}
			return audioController;
		}
	}

	void Init()
	{
        
	}

    public void Bonked()
    {
        effectSource.clip = bonkClip;
        effectSource.Play();
    }

    public void Scored()
    {
        effectSource.clip = scoreClip;
        effectSource.Play();
    }
}

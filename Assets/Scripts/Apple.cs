using UnityEngine;

public class Apple : MonoBehaviour {

    public Sprite[] normalApples;
    public Sprite[] angryApples;

    public bool isAngry = false;
    public float angryChance = 0.3f;

    private Rigidbody rigidBody;
    private AudioSource audioSource;

	void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.useGravity = false;

        if(Random.value < angryChance)
        {
            isAngry = true;
        }

        if (!isAngry)
        {
            GetComponent<SpriteRenderer>().sprite =
                normalApples[Random.Range(0, (normalApples.Length - 1))];
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite =
                angryApples[Random.Range(0, (angryApples.Length - 1))];
        }
    }

    void Start()
    {
		iTween.ShakeRotation(
        	gameObject,
        	iTween.Hash(
        		"z", 30,
        		"time", 1.0f,
        		"delay", 2.0f,
        		"onComplete", "ShakeComplete",
        		"onCompleteTarget", gameObject
        	)
        );
    }

    void OnTriggerEnter(Collider target)
	{
		if (target.gameObject.tag == "Player")
		{
            Debug.Log("Hit");

            GameManager.Instance.colourController.ChangeColours();

            if(isAngry)
            {
				GameManager.Instance.cameraShakeController.ShakeCamera(5.0f, 0.3f);
                GameManager.Instance.playerController.LifeLost();
                GameManager.Instance.audioController.Bonked();
                Destroy(gameObject);
            }
            else
            {
                GameManager.Instance.cameraShakeController.ShakeCamera(1.0f, 0.1f);
                GameManager.Instance.scoreController.AddScore(1);
                GameManager.Instance.audioController.Scored();
                Destroy(gameObject);
            }
		}
	}

    void ShakeComplete()
    {
        rigidBody.useGravity = true;
        audioSource.Play();
    }
}

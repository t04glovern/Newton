using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public Image life01;
    public Image life02;
    public Image life03;

    public Color life01Color;
    public Color life02Color;
    public Color life03Color;
    public Color deadLifeColor;

    private int liveTotal;

	private static PlayerController playerController;
	public static PlayerController Instance
	{
		get
		{
			if (!playerController)
			{
				playerController =
					FindObjectOfType(typeof(PlayerController)) as PlayerController;

				if (!playerController)
				{
					Debug.LogError
						 ("No PlayerController script find on a GameObject");
				}
				else
				{
					playerController.Init();
				}
			}
			return playerController;
		}
	}

	void Init()
	{
        liveTotal = 3;
        UpdateLives();
    }

    void UpdateLives()
    {
        life01.color = deadLifeColor;
        life02.color = deadLifeColor;
        life03.color = deadLifeColor;

        if(liveTotal > 0) {
			life01.color = life01Color;
            if (liveTotal > 1) {
				life02.color = life02Color;
                if (liveTotal > 2) {
                    life03.color = life03Color;
                }
            }
        }
        else {
            GameManager.Instance.GameOver();
        }
    }

    public void LifeLost()
    {
        liveTotal--;
        UpdateLives();
    }
}

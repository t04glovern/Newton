using UnityEngine;

public class ColourController : MonoBehaviour {

    public GameObject[] environmentItems;
    public Color[] colours;
    public Material ground;

    private static ColourController colourController;
    public static ColourController Instance 
    {
        get 
        {
            if(!colourController) 
            {
                colourController = 
                    FindObjectOfType(typeof(ColourController)) as ColourController;

				if (!colourController)
                {
                    Debug.LogError
                         ("No ColourController script find on a GameObject");
                }
                else
                {
                    colourController.Init();
                }
            }
            return colourController;
        }
    }

	void Init()
    {
        ChangeColours();
	}

    public void ChangeColours()
    {
        // Change item colours
		foreach (GameObject go in environmentItems)
		{
			go.GetComponent<SpriteRenderer>().color = GetRandomColour();
		}

        // Change background colours
        Camera.main.backgroundColor = GetRandomColour();

        // Change ground colours
        ground.color = GetRandomColour();
    }

    Color GetRandomColour()
    {
		return colours[Random.Range(0, (colours.Length))];
    }
}

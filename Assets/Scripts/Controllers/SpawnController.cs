using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {

    public GameObject[] spawnLocations;
    public GameObject objectPrefab;
    public float spawnTime = 1.0f;

    private static SpawnController spawnController;
	public static SpawnController Instance
	{
		get
		{
			if (!spawnController)
			{
				spawnController = 
                    FindObjectOfType(typeof(SpawnController)) as SpawnController;

				if (!spawnController)
				{
					Debug.LogError
                         ("No SpawnController script find on a GameObject");
				}
				else
				{
					spawnController.Init();
				}
			}
			return spawnController;
		}
	}

	void Init()
    {
		InvokeRepeating("SpawnObject", spawnTime, spawnTime);
	}

    void SpawnObject() 
    {
        // Randomly select a location to spawn apple at
        List<GameObject> spawnLocations = GetOpenSpawnLocations();
        if(GetOpenSpawnLocations().Count > 0)
        {
            GameObject spawnLocation = 
                spawnLocations[Random.Range(0, spawnLocations.Count)];

			// Spawn an abject at the randomly selected location
			GameObject apple = Instantiate(
				objectPrefab,
				spawnLocation.transform.position,
				spawnLocation.transform.rotation
			) as GameObject;

			// Set apple to be child of location object
			apple.transform.SetParent(spawnLocation.transform);
        }
    }

    List<GameObject> GetOpenSpawnLocations()
    {
        List<GameObject> openLocations = new List<GameObject>();

        foreach(GameObject location in spawnLocations)
        {
            if (location.transform.childCount == 0)
            {
                openLocations.Add(location);
            }
        }

        return openLocations;
    }
}

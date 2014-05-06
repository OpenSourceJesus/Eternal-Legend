using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour
{

	void Awake ()
	{
		DontDestroyOnLoad(GameObject.Find("Save Game Manager"));
		for (int i = 0; i < Application.levelCount; i ++)
		{
			LevelSerializer.SavedGames[LevelSerializer.PlayerName].Add(LevelSerializer.CreateSaveEntry("Replay", false));
		}
	}

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.name == "Player")
		{
			if (!other.gameObject.GetComponent<Player>().inReplay)
			{
				if (other.gameObject.GetComponent<Player>().displayTime < PlayerPrefs.GetFloat("Level " + (Application.loadedLevel + 1) + " Time", Mathf.Infinity))
				{
					PlayerPrefs.SetFloat("Level " + (Application.loadedLevel + 1) + " Time", other.gameObject.GetComponent<Player>().displayTime);
					LevelSerializer.SavedGames[LevelSerializer.PlayerName][Application.loadedLevel] = LevelSerializer.CreateSaveEntry("Replay", false);;
				}
				if (Application.loadedLevel + 1 < Application.levelCount)
					Application.LoadLevel(Application.loadedLevel + 1);
				else
					Application.LoadLevel(0);
			}
			else
			{
				Time.timeScale = 0;
				other.gameObject.GetComponent<Player>().inReplay = false;
				GameObject.Find("Replays").GetComponent<Replays>().currentElement = 0;
				GameObject.Find("Replays").GetComponent<Replays>().trms = new ArrayList();
				GameObject.Find("Replays").GetComponent<Replays>().localScaleXs = new ArrayList();
			}
		}
	}
}

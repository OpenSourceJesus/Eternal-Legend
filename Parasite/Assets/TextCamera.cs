using UnityEngine;
using System.Collections;

public class TextCamera : MonoBehaviour
{
	bool storyMode;

	void Awake ()
	{
		if (GameObject.Find("TextCamera") == null)
		{
			DontDestroyOnLoad(gameObject);
			name = "TextCamera";
		}
	}

	// Use this for initialization
	void Start ()
	{
		if (GameObject.Find("TextCamera2") != null)
			Destroy(GameObject.Find("TextCamera2"));
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (storyMode && Input.anyKeyDown)
		{
			GameMode ();
			if (GameObject.Find("Tutorial2").GetComponent<Tutorial>().currentElement == 0)
				GameObject.Find("Tutorial2").GetComponent<Tutorial>().currentElement ++;
		}
	}

	public void StoryMode (string text)
	{
		Time.timeScale = 0;
		text = text.Replace("Ω", "\n");
		GameObject.Find("Text").GetComponent<TextMesh>().text = text;
		GameObject.Find("TextCamera").camera.depth = 1;
		storyMode = true;
	}
	
	public void GameMode ()
	{
		GameObject.Find("TextCamera").camera.depth = -1;
		Time.timeScale = 1;
		storyMode = false;
	}
}

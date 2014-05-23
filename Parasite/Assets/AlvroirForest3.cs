using UnityEngine;
using System.Collections;

public class AlvroirForest3 : MonoBehaviour
{
	bool storyMode;
	GameObject player;
	Tutorial tutorial;
	TextCamera textCamera;
	public string[] text;

	// Use this for initialization
	void Start ()
	{
		player = GameObject.Find("Player");
		tutorial = GameObject.Find("Tutorial").GetComponent<Tutorial>();
		textCamera = GameObject.Find("TextCamera").GetComponent<TextCamera>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (player.transform.position.x < -25)
		{
			PlayerPrefs.SetInt("Scene", Application.loadedLevel - 1);
			player.GetComponent<Player>().FadeOut (Application.loadedLevel - 1, new Vector2(750, 58));
		}
		else if (player.transform.position.x > 250)
		{
			if (tutorial.currentElement == 7)
			{
				textCamera.StoryMode (text[0] + text[1] + text[2] + text[3] + text[4] + text[5] + text[6] + text[7] + text[8] + text[9] + text[10] + text[11]);
				if (Input.anyKeyDown)
					tutorial.currentElement ++;
			}
			else if (tutorial.currentElement == 8)
			{
				textCamera.StoryMode (text[13]);
			}
		}
	}
}

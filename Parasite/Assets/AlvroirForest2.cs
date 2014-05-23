using UnityEngine;
using System.Collections;

public class AlvroirForest2 : MonoBehaviour
{
	bool storyMode;
	GameObject player;
	Tutorial tutorial;

	// Use this for initialization
	void Start ()
	{
		player = GameObject.Find("Player");
		tutorial = GameObject.Find("Tutorial").GetComponent<Tutorial>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (player.transform.position.x < 430)
		{
			PlayerPrefs.SetInt("Scene", Application.loadedLevel);
			player.GetComponent<Player>().FadeOut (Application.loadedLevel - 1, new Vector2(410, 6));
		}
		else if (tutorial.currentElement == 6 && player.transform.position.x > 650)
		{
			GameObject.Find("TextCamera").GetComponent<TextCamera>().StoryMode ("Zerith's heart jumped as he saw two huge frogs leaping towards the group of survivors. He had only heard stories of these frogs,Ωbut had never seen one before. Accoridng to the stories, they can kill any creature in moments just by touching their poisionousΩskin to it. Unfortunatly, nobody in Zerith's group carried anything longer than a dagger.");
			tutorial.currentElement ++;
		}
		else if (player.transform.position.x > 770)
		{
			PlayerPrefs.SetInt("Scene", Application.loadedLevel + 1);
			player.GetComponent<Player>().FadeOut (Application.loadedLevel + 1, new Vector2(-5, 0));
		}
	}
}

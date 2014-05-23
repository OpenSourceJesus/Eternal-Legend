using UnityEngine;
using System.Collections;

public class AlvroirForest1 : MonoBehaviour
{
	GameObject player;
	Tutorial tutorial;

	// Use this for initialization
	void Start ()
	{
		if (GameObject.Find("Player").GetComponent<Player>().spawnLoc == (Vector2) GameObject.Find("Spawnpoint").transform.position)
			GameObject.Find("TextCamera").GetComponent<TextCamera>().StoryMode ("For what felt like an eternity, Zerith only thought of the incident, and each moment he recapped what had happened it becameΩmore horrifying. The screams of people and the crackling of flames were still ringing in his head. On the streets people were fightingΩwhile others tried to escape. People had broke down the door to Zerith's house, and soon after killed his parents. One of the onlyΩthings keeping Zerith from having panic attack was the small group of other survivors, and the fact that they were almost at the nearestΩsettlement where each of them planned to start a new life.");
		player = GameObject.Find("Player");
		tutorial = GameObject.Find("Tutorial").GetComponent<Tutorial>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (player.transform.position.x < -17.5)
		{
			GameObject.Find("TextCamera").GetComponent<TextCamera>().StoryMode ("Zerith turned back for a few moments and walked a few steps over a small hill. He saw his burning village on the horizon, and his eyesΩwatered. Then he turned again and walked back to the group.");
			player.transform.position = new Vector2(0, player.transform.position.y);
			player.transform.localScale = Vector3.one;
			player.GetComponent<Player>().facingRight = true;
		}
		else if (player.transform.position.x > 430)
		{
			PlayerPrefs.SetInt("Scene", Application.loadedLevel + 1);
			player.GetComponent<Player>().FadeOut (Application.loadedLevel + 1, new Vector2(450, 6));
			if (tutorial.GetComponent<Tutorial>().currentElement == 5)
				tutorial.GetComponent<Tutorial>().currentElement ++;
		}
	}
}

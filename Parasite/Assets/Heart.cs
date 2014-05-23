using UnityEngine;
using System.Collections;

public class Heart : MonoBehaviour
{
	public float hpAmount;

	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad(gameObject);
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Heart"))
			if (go != gameObject && go.name == name && (!renderer.enabled || !collider2D.enabled))
				Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.timeSinceLevelLoad < .0001)
			Start ();
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.name == "Player")
		{
			other.gameObject.GetComponent<Player>().maxHP += hpAmount;
			GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
			collider2D.enabled = false;
			GameObject.Find("TextCamera").GetComponent<TextCamera>().StoryMode("Congrats! You have collected a heart piece! Every two of these you collect you will gain another life, making you harder to kill. BeΩwarned, however, this heart piece will be taken away and you will have to recollect it if you die before getting to a savepoint. OnceΩyou save after collecting a heart piece you will have it permanently.");
		}
	}
}

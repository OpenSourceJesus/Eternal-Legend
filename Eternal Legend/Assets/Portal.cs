using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour
{

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
			if (GameObject.Find("Player").GetComponent<Player>().displayTime < PlayerPrefs.GetFloat("Level " + (Application.loadedLevel + 1) + "Time", Mathf.Infinity))
				PlayerPrefs.SetFloat("Level " + (Application.loadedLevel + 1) + "Time", GameObject.Find("Player").GetComponent<Player>().displayTime);
			Application.LoadLevel(Application.loadedLevel + 1);
		}
	}
}

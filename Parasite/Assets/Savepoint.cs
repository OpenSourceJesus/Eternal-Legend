using UnityEngine;
using System.Collections;

public class Savepoint : MonoBehaviour {

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
			//if (PlayerPrefs.GetInt("SpawnX", (int) other.gameObject.GetComponent<Player>().spawnLoc.x) != transform.position.x || PlayerPrefs.GetInt("SpawnY", (int) other.gameObject.GetComponent<Player>().spawnLoc.y) != transform.position.y)
			//{
				PlayerPrefs.SetFloat("MaxHP", PlayerPrefs.GetFloat("MaxHP", 1) + other.gameObject.GetComponent<Player>().maxHP);
				foreach (GameObject go in GameObject.FindGameObjectsWithTag("Heart"))
					if (go.GetComponent<SpriteRenderer>().color == new Color(0, 0, 0, 0))
						go.renderer.enabled = false;
			//}
			other.gameObject.GetComponent<Player>().spawnLoc = transform.position;
			PlayerPrefs.SetInt("SpawnX", (int) other.gameObject.GetComponent<Player>().spawnLoc.x);
			PlayerPrefs.SetInt("SpawnY", (int) other.gameObject.GetComponent<Player>().spawnLoc.y);
			PlayerPrefs.SetInt("Scene", Application.loadedLevel);
			PlayerPrefs.SetInt("Tip num", GameObject.Find("Tutorial").GetComponent<Tutorial>().currentElement);
			PlayerPrefs.SetInt("Scene of savepoint", Application.loadedLevel);
			if (Application.loadedLevel == 0)
				if (other.transform.Find("Tutorial").GetComponent<Tutorial>().currentElement == 4)
					other.transform.Find("Tutorial").GetComponent<Tutorial>().currentElement ++;
		}
	}
}

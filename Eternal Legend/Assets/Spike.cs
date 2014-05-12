using UnityEngine;
using System.Collections;

public class Spike : MonoBehaviour
{
	public bool destroyAll;

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
			Application.LoadLevel(Application.loadedLevel);
		}
		else if (destroyAll && !other.gameObject.name.Contains("Spike"))
			Destroy(other.gameObject);
	}
}

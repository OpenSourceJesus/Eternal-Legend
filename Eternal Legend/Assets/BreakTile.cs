using UnityEngine;
using System.Collections;

public class BreakTile : MonoBehaviour
{
	bool breaking;
	public int breakTimer;
	int initBreakTimer;

	// Use this for initialization
	void Start ()
	{
		initBreakTimer = breakTimer;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (breaking)
		{
			float a = GetComponent<SpriteRenderer>().color.a;
			a -= .02f;
			GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, a);
			breakTimer --;
			if (breakTimer <= 0)
				Destroy (gameObject);
		}
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.collider.gameObject.name == "Player")
		{
			breaking = true;
		}
	}
}

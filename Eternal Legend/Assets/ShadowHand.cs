using UnityEngine;
using System.Collections;

public class ShadowHand : MonoBehaviour
{
	public float moveSpeed = 1f;
	public float dropSpeed = 1f;
	bool dropping;
	public float dropTriggerDist = 1f;
	Vector2 initPlayerLoc;
	public int minHeight = 1;

	// Use this for initialization
	void Start ()
	{
		if (dropTriggerDist == -1)
			dropTriggerDist = transform.lossyScale.x;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (transform.position.y > GameObject.Find("Player").transform.position.y && Mathf.Abs(transform.position.x - GameObject.Find("Player").transform.position.x) <= dropTriggerDist)
		{
			initPlayerLoc = GameObject.Find("Player").transform.position;
			dropping = true;
		}
		if (!dropping)
		{
			Vector2 vel = GameObject.Find("ShadowHandTarget").transform.position - transform.position;
			vel *= 9999999999;
			vel = Vector2.ClampMagnitude(vel, moveSpeed);
			if (minHeight != -1 && transform.position.y < GameObject.Find("ShadowHandTarget").transform.position.y && transform.position.y > GameObject.Find("Player").transform.position.y + minHeight)
				vel.y = 0;
			rigidbody2D.velocity = vel;
		}
		else
		{
			rigidbody2D.AddForce(new Vector2(0, -dropSpeed));
			if (transform.position.y <= initPlayerLoc.y)
				dropping = false;
		}
	}
	
	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.collider.gameObject.name == "Player")
		{
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}

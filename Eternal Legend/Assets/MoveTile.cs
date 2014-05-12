using UnityEngine;
using System.Collections;

public class MoveTile : MonoBehaviour {
	public int forwardSpeed;
	public int backwardSpeed;
	bool movingForward;
	Vector2 initLoc;
	bool xMovement;

	// Use this for initialization
	void Start ()
	{
		initLoc = transform.position;
		xMovement = Mathf.RoundToInt(transform.rotation.eulerAngles.z) == 0 || Mathf.RoundToInt(transform.rotation.eulerAngles.z) == 180;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (movingForward)
		{
			if (xMovement)
			{
				rigidbody2D.velocity = new Vector2(transform.localScale.x * forwardSpeed, 0);
				transform.position = new Vector2(transform.position.x, initLoc.y);
			}
			else
			{
				rigidbody2D.velocity = new Vector2(0, transform.localScale.y * forwardSpeed);
				transform.position = new Vector2(initLoc.x, transform.position.y);
			}
		}
		else if (Vector2.Distance(transform.position, initLoc) > .01)
		{
			if (xMovement)
			{
				rigidbody2D.velocity = new Vector2(transform.localScale.x * -forwardSpeed, 0);
				transform.position = new Vector2(transform.position.x, initLoc.y);
			}
			else
			{
				rigidbody2D.velocity = new Vector2(0, transform.localScale.y * -forwardSpeed);
				transform.position = new Vector2(initLoc.x, transform.position.y);
			}
		}
		else
			rigidbody2D.velocity = Vector2.zero;
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.gameObject.name == "Player")
			movingForward = true;
	}

	void OnCollisionStay2D (Collision2D coll)
	{
		if (coll.gameObject.name == "Player")
		{
			movingForward = true;
			//coll.gameObject.SetParent(gameObject);
			//coll.gameObject.transform.rotation = Quaternion.Euler(0, 0, -transform.rotation.eulerAngles.z);
		}
	}

	void OnCollisionExit2D (Collision2D coll)
	{
		if (coll.gameObject.name == "Player")
			movingForward = false;
	}
}

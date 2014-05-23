using UnityEngine;
using System.Collections;

public class Saw : MonoBehaviour
{
	public Transform[] wayPoints;
	public float moveSpeed = 0;
	int currentWayPoint = 0;
	public float slowRate = 2f;
	public float spinRate = 0f;
	public int damage;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (wayPoints.Length == 0)
			return;
		Vector2 vel = wayPoints [currentWayPoint].position - transform.position;
		vel *= slowRate;
		vel = Vector2.ClampMagnitude(vel, moveSpeed);
		rigidbody2D.velocity = vel;
		if (Vector2.Distance(transform.position, wayPoints[currentWayPoint].position) < .1)
		{
			currentWayPoint ++;
			if (currentWayPoint == wayPoints.Length)
				currentWayPoint = 0;
		}
		if (rigidbody2D.velocity.x > 0 || rigidbody2D.velocity.y < 0)
			spinRate = -Mathf.Abs(spinRate);
		else if (rigidbody2D.velocity.x < 0 || rigidbody2D.velocity.y > 0)
			spinRate = Mathf.Abs(spinRate);
		transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + spinRate);
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.name == "Player")
		{
			other.gameObject.GetComponent<Player>().ChangeHP(-damage);
		}
	}
}

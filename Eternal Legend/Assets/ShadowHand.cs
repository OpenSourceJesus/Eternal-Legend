using UnityEngine;
using System.Collections;

public class ShadowHand : MonoBehaviour
{
	public Transform[] wayPoints;
	int currentWayPoint = 0;
	public int speed = 10;
	public float slowRate = 4f;
	public int visionRange = 25;
	RaycastHit2D hit;

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		hit = Physics2D.Raycast(transform.position, GameObject.Find("Player").transform.position - transform.position, visionRange);
		if (hit.collider != null)
		{
			transform.Find("Vision").GetComponent<LineRenderer>().SetPosition(1, hit.point - (Vector2) transform.position);
			if (hit.collider.gameObject.name == "Player")
				Application.LoadLevel(Application.loadedLevel);
		}
		else
		{
			//Quaternion dirToPlayer = Quaternion.FromToRotation(transform.position, GameObject.Find("Player").transform.position);
			Vector2 toPlayer = GameObject.Find("Player").transform.position - transform.position;
			toPlayer = Vector2.ClampMagnitude(toPlayer, visionRange);
			transform.Find("Vision").GetComponent<LineRenderer>().SetPosition(1, toPlayer);
		}
		if (wayPoints.Length == 0)
			return;
		Vector2 vel = wayPoints [currentWayPoint].position - transform.position;
		vel *= slowRate;
		vel = Vector2.ClampMagnitude(vel, speed);
		rigidbody2D.velocity = vel;
		if (Vector2.Distance(transform.position, wayPoints[currentWayPoint].position) < .1)
		{
			currentWayPoint ++;
			if (currentWayPoint == wayPoints.Length)
				currentWayPoint = 0;
		}
	}
}

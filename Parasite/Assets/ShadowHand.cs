using UnityEngine;
using System.Collections;

public class ShadowHand : MonoBehaviour
{
	public Transform[] wayPoints;
	int currentWayPoint = 0;
	public int speed = 10;
	public float slowRate = 4f;
	public float aimSpeed = .1f;
	public int visionRange = 25;
	RaycastHit2D hit;
	Vector2 shootVec;

	// Use this for initialization
	void Start ()
	{
		shootVec = GameObject.Find("Player").transform.position - transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		Vector2 toPlayer = GameObject.Find("Player").transform.position - transform.position;
		toPlayer -= shootVec;
		toPlayer = Vector2.ClampMagnitude(toPlayer, aimSpeed);
		shootVec += toPlayer;
		shootVec = Vector2.ClampMagnitude(shootVec, visionRange);
		hit = Physics2D.Raycast(transform.position, shootVec, visionRange);
		if (hit.collider != null)
		{
			transform.Find("Vision").GetComponent<LineRenderer>().SetPosition(1, hit.point - (Vector2) transform.position);
			if (hit.collider.gameObject.name == "Player")
				Application.LoadLevel(Application.loadedLevel);
		}
		else
		{
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

	void Update ()
	{
		if (Time.timeSinceLevelLoad < .0001)
			Start ();
	}
}

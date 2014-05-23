using UnityEngine;
using System.Collections;

public class Frog : MonoBehaviour
{
	public float maxSpeed = 10f;
	bool facingRight = true;
	float move = 0f;
	bool grounded = false;
	float groundCheckRadius = 0.2f;
	float wallCheckRadius = 0.2f;
	public LayerMask whatIsGround;
	public int jumpForce = 500;
	Animator anim;
	public float wallHoverAmount = .2f;
	public int jumpRate = 500;
	public int jumpTimer = 0;
	public int initJumpDelay = 0;
	public float groundHoverAmount = .0001f;
	public bool awake;

	// Use this for initialization
	void Start ()
	{
		//anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (!awake)
			return;
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("GroundCheck"))
			if (go.transform.IsChildOf(transform))
			{
				grounded = Physics2D.OverlapCircle(go.transform.position, groundCheckRadius, whatIsGround);
				if (grounded)
				{
					transform.position = new Vector2(transform.position.x, transform.position.y + groundHoverAmount);
					break;
				}
			}
		if (grounded)
		{
			jumpTimer ++;
			initJumpDelay --;
			if (jumpTimer > jumpRate && initJumpDelay < 0)
			{
				rigidbody2D.AddForce(Vector2.up * jumpForce);
				grounded = false;
				jumpTimer = 0;
			}
		}
		else
		{
			Vector2 vel = GameObject.Find("Player").transform.position - transform.position;
			vel = Vector2.ClampMagnitude(vel, maxSpeed);
			move = vel.x;
			foreach (GameObject go in GameObject.FindGameObjectsWithTag("WallCheck"))
			{
				if (go.transform.IsChildOf(transform) && Physics2D.OverlapCircle(go.transform.position, wallCheckRadius, whatIsGround))
				{
					move = 0;
					transform.position = new Vector2(transform.position.x - wallHoverAmount * transform.localScale.x, transform.position.y);
					break;
				}
			}
			rigidbody2D.velocity = new Vector2(move, rigidbody2D.velocity.y);
		}
		if ((GameObject.Find("Player").transform.position.x - transform.position.x < 0 && !facingRight) || (GameObject.Find("Player").transform.position.x - transform.position.x > 0 && facingRight))
			Flip ();
		//anim.SetFloat("Speed", Mathf.Abs(move));
		//anim.speed = anim.GetFloat("Speed");
	}
	
	void Flip ()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.collider.gameObject.name == "Player")
		{
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}
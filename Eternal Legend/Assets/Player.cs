using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public float maxSpeed = 10f;
	bool facingRight = true;
	float move = 0f;
	bool grounded = false;
	public Transform groundCheck;
	float groundCheckRadius = 0.2f;
	public LayerMask whatIsGround;
	public int jumpForce = 500;
	public Transform wallCheck1;
	public Transform wallCheck2;
	float wallCheckRadius = 0.2f;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("GroundCheck"))
		{
			grounded = Physics2D.OverlapCircle(go.transform.position, groundCheckRadius, whatIsGround);
			if (grounded)
				break;
		}
		move = Input.GetAxis("Horizontal") * maxSpeed;
		if ((move > 0 && !facingRight) || (move < 0 && facingRight))
			Flip ();
		if (move != 0 && Physics2D.OverlapCircle(wallCheck1.position, wallCheckRadius, whatIsGround) && !Physics2D.OverlapCircle(wallCheck2.position, wallCheckRadius, whatIsGround))
			transform.position = new Vector2(transform.position.x, transform.position.y + Vector2.Distance(wallCheck1.position, wallCheck2.position));
		else
			foreach (GameObject go in GameObject.FindGameObjectsWithTag("WallCheck"))
				if (move != 0 && Physics2D.OverlapCircle(go.transform.position, wallCheckRadius, whatIsGround))
				{
					move = 0;
					break;
				}
		if (move != 0)
			rigidbody2D.velocity = new Vector2(move, rigidbody2D.velocity.y);
	}

	void Update ()
	{
		if (grounded && Input.GetKeyDown(KeyCode.Space))
		{
			rigidbody2D.AddForce(Vector2.up * jumpForce);
			grounded = false;
		}
	}

	void Flip ()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public float maxSpeed = 10f;
	bool facingRight = true;
	float move = 0f;
	bool grounded = false;
	float groundCheckRadius = 0.2f;
	public LayerMask whatIsGround;
	public int jumpForce = 500;
	public Transform wallCheck1;
	public Transform wallCheck2;
	float wallCheckRadius = 0.2f;
	Animator anim;
	public float groundHoverAmount = .1f;
	//public float wallHoverAmount = .2f;
	public GUISkin guiSkin1;
	float numOfDecimalPlaces = .1f;
	public int pushOfWallForce = 500;
	public float extraXVel = 0;
	bool onWall = false;
	float time;
	public float displayTime;

	// Use this for initialization
	void Start ()
	{
		anim = GetComponent<Animator>();
		time = Time.fixedTime;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
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
		move = Input.GetAxis("Horizontal") * maxSpeed;
		if (!onWall && ((move > 0 && !facingRight) || (move < 0 && facingRight)))
			Flip ();
		if (move != 0 && Physics2D.OverlapCircle(wallCheck1.position, wallCheckRadius, whatIsGround) && !Physics2D.OverlapCircle(wallCheck2.position, wallCheckRadius, whatIsGround))
			transform.position = new Vector2(transform.position.x, transform.position.y + Vector2.Distance(wallCheck1.position, wallCheck2.position));
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("WallCheck"))
		{
			if (go.transform.IsChildOf(transform) && Physics2D.OverlapCircle(go.transform.position, wallCheckRadius, whatIsGround) && !Physics2D.OverlapCircle(go.transform.position, wallCheckRadius, whatIsGround).gameObject.name.Contains("(Wall Jump)"))
			{
				extraXVel = 0;
				move = 0;
				//transform.position = new Vector2(transform.position.x - wallHoverAmount * transform.localScale.x, transform.position.y);
				break;
			}
		}
		onWall = false;
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("WallCheck"))
		{
			if (go.transform.IsChildOf(transform) && Physics2D.OverlapCircle(go.transform.position, wallCheckRadius, whatIsGround) && Physics2D.OverlapCircle(go.transform.position, wallCheckRadius, whatIsGround).gameObject.name.Contains("(Wall Jump)"))
			{
				onWall = true;
				extraXVel = 0;
				int x = 0;
				if (transform.position.x > go.transform.position.x)
					x = 1;
				else
					x = -1;
				if (Input.GetAxisRaw("Horizontal") == x && Input.GetAxisRaw("Jump") == 1)
				{
					rigidbody2D.velocity = Vector2.zero;
					//transform.position = new Vector2(transform.position.x - wallHoverAmount * x, transform.position.y);
					rigidbody2D.AddForce(Vector2.up * jumpForce);
					//rigidbody2D.AddForce(Vector2.right * -transform.lossyScale.x * pushOfWallForce);
					extraXVel = x * pushOfWallForce;
					grounded = false;
					onWall = false;
					break;
				}
			}
		}
		extraXVel *= rigidbody2D.drag;
		if (move != 0)
			rigidbody2D.velocity = new Vector2(move + extraXVel, rigidbody2D.velocity.y);
		anim.SetFloat("Speed", Mathf.Abs(move));
		anim.speed = anim.GetFloat("Speed");
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
		//yield return new WaitForSeconds(0);
		transform.localScale = theScale;
	}

	void OnGUI ()
	{
		displayTime = Time.fixedTime - time;
		GUI.skin = guiSkin1;
		GUI.Label(new Rect(0, 0, Screen.width, 50), "" + Mathf.Round(displayTime / numOfDecimalPlaces) * numOfDecimalPlaces);
		if (Input.GetKeyDown(KeyCode.P))
		{

		}
	}
}
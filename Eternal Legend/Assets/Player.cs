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
	public GUISkin guiSkin1;
	public GUISkin guiSkin2;
	float numOfDecimalPlaces = .1f;
	public int pushOfWallForce = 500;
	public float extraXVel = 0;
	bool onWall = false;
	public float time;
	public float displayTime;
	public bool inReplay;
	public Material skybox1;
	public Material skybox2;
	int pXInput;

	void Awake ()
	{
		anim = GetComponent<Animator>();
		if (GameObject.Find("Player") == null)
		{
			DontDestroyOnLoad(gameObject);
			name = "Player";
		}
	}

	// Use this for initialization
	void Start ()
	{
		if (GameObject.Find("Player2") != null)
		{
			GameObject.Find("Player").transform.position = GameObject.Find("Player2").transform.position;
			GameObject.Find("Player").transform.localScale = Vector3.one;
			GameObject.Find("Player").GetComponent<Player>().facingRight = true;
			GameObject.Find("Player").rigidbody2D.velocity = Vector2.zero;
			Destroy(GameObject.Find("Player2"));
		}
		displayTime = 0;
		if (Application.loadedLevelName.Contains("1-"))
			transform.Find("Camera").GetComponent<Skybox>().material = skybox1;
		else if (Application.loadedLevelName.Contains("2-"))
			transform.Find("Camera").GetComponent<Skybox>().material = skybox2;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (inReplay)
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
		if (Input.GetAxisRaw("Horizontal2") == 0)
			move = Input.GetAxis("Horizontal") * maxSpeed;
		else
			move = Input.GetAxisRaw("Horizontal2") * maxSpeed;
		if (!onWall && ((move > 0 && !facingRight) || (move < 0 && facingRight)))
			Flip ();
		if (move == 0 && pXInput < 0)
			Flip ();
		if (move != 0 && Physics2D.OverlapCircle(wallCheck1.position, wallCheckRadius, whatIsGround) && !Physics2D.OverlapCircle(wallCheck2.position, wallCheckRadius, whatIsGround))
			transform.position = new Vector2(transform.position.x, transform.position.y + Vector2.Distance(wallCheck1.position, wallCheck2.position));
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("WallCheck"))
		{
			if (go.transform.IsChildOf(transform) && Physics2D.OverlapCircle(go.transform.position, wallCheckRadius, whatIsGround) && !Physics2D.OverlapCircle(go.transform.position, wallCheckRadius, whatIsGround).gameObject.name.Contains("(Wall Jump)"))
			{
				extraXVel = 0;
				move = 0;
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
				if ((move > 0 && x < 0) || (move < 0 && x > 0))
					move = 0;
				if ((Input.GetAxisRaw("Horizontal") == x || Input.GetAxisRaw("Horizontal2") == x) && Input.GetAxisRaw("Jump") == 1)
				{
					rigidbody2D.velocity = Vector2.zero;
					rigidbody2D.AddForce(Vector2.up * jumpForce);
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
		if (Time.timeSinceLevelLoad < .0001)
			Start ();
		if (Time.timeScale == 1)
			displayTime += Time.deltaTime;
		if (inReplay)
			return;
		if (grounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton3)))
		{
			rigidbody2D.AddForce(Vector2.up * jumpForce);
			grounded = false;
		}
		if (Input.GetKeyDown(KeyCode.P))
		{
			if (Time.timeScale == 0)
				Time.timeScale = 1;
			else
				Time.timeScale = 0;
		}
		pXInput = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal2"));
	}

	void Flip () 
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void OnGUI ()
	{
		GUI.skin = guiSkin2;
		if (Time.timeScale == 0)
		{
			time = Time.fixedTime - time;
			if (GUI.Button(new Rect(0, 0, 100, 25), "Resume"))
				Time.timeScale = 1;
			for (int i = 1; i <= Application.levelCount; i ++)
			{
				if (GUI.Button(new Rect(0, i * 25 + 40, 100, 25), "Level " + i))
				{
					GameObject.Find("Replays").GetComponent<Replays>().currentElement = 0;
					GameObject.Find("Replays").GetComponent<Replays>().trms = new ArrayList();
					GameObject.Find("Replays").GetComponent<Replays>().localScaleXs = new ArrayList();
					Application.LoadLevel(i - 1);
				}
				if (PlayerPrefs.GetFloat("Level " + i + " Time", Mathf.Infinity) == Mathf.Infinity)
					GUI.Label(new Rect(100, i * 25 + 40, 100, 25), "Time: Not beaten");
				else
				{
					GUI.Label(new Rect(100, i * 25 + 40, 100, 25), "Time: " + PlayerPrefs.GetFloat("Level " + i + " Time", Mathf.Infinity));
					if (GUI.Button(new Rect(200, i * 25 + 40, 100, 25), "Watch Replay"))
					{
						Application.LoadLevel(i - 1);
						LevelSerializer.LoadNow(LevelSerializer.SavedGames[LevelSerializer.PlayerName][i - 1].Data);
						inReplay = true;
						Time.timeScale = 1;
					}
				}
			}
			if (GUI.Button(new Rect(Screen.width - 100, Screen.height - 25, 100, 25), "Clear Data"))
				PlayerPrefs.DeleteAll();
		}
		else
		{
			GUI.skin = guiSkin1;
			GUI.Label(new Rect(0, 25, Screen.width, 50), "" + Mathf.Round(displayTime / numOfDecimalPlaces) * numOfDecimalPlaces);
			if (GUI.Button(new Rect(0, 0, 100, 25), "Menu"))
				Time.timeScale = 0;
		}
	}

	void OnApplicationQuit() {
		PlayerPrefs.Save();
	}
}
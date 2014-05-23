using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public float maxSpeed = 10f;
	public bool facingRight = true;
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
	float numOfDecimalPlaces = 1f;
	public int pushOfWallForce = 500;
	public float extraXVel = 0;
	bool onWall = false;
	public float time;
	public float displayTime;
	public bool inReplay;
	public Material skybox1;
	public Material skybox2;
	public string gameUpdateID;
	int pXInput;
	public bool paused;
	public Vector2 spawnLoc;
	Vector2 levelEnterLoc = new Vector2(-1, -1);
	float hp;
	public float maxHP;
	bool leftSavepointScene;
	bool showTimer;
	bool inStats;
	bool dead;
	int levelToLoad = -1;

	void Awake ()
	{
		displayTime = PlayerPrefs.GetFloat("Timer", 0);
		if (PlayerPrefs.GetInt("Playing", 1) == 0)
		{
			PlayerPrefs.SetInt("Playing", 1);
			Application.LoadLevel(PlayerPrefs.GetInt("Scene of savepoint", 0));
		}
		else
		{
			anim = GetComponent<Animator>();
			if (GameObject.Find("Player") == null)
			{
				DontDestroyOnLoad(gameObject);
				name = "Player";
			}
		}
	}

	// Use this for initialization
	void Start ()
	{
		dead = false;
		levelToLoad = -1;
		maxHP = 0;
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Frog"))
			go.GetComponent<Frog>().awake = false;
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
			if (go.name == "Player2")
				Destroy(go);
		//displayTime = 0;
		if (Application.loadedLevelName.Contains("AlvroirForest"))
			transform.Find("Camera").GetComponent<Skybox>().material = skybox1;
		else if (Application.loadedLevelName.Contains("2-"))
			transform.Find("Camera").GetComponent<Skybox>().material = skybox2;
		if (Application.loadedLevel == PlayerPrefs.GetInt("Scene", 0) && PlayerPrefs.GetInt("Scene of savepoint", 0) == Application.loadedLevel && !leftSavepointScene && levelEnterLoc == new Vector2(-1, -1))
			transform.position = new Vector2(PlayerPrefs.GetInt("SpawnX", (int) spawnLoc.x), PlayerPrefs.GetInt("SpawnY", (int) spawnLoc.y));
		else if (levelEnterLoc != new Vector2(-1, -1))
			transform.position = levelEnterLoc;
		transform.localScale = Vector3.one;
		facingRight = true;
		rigidbody2D.velocity = Vector2.zero;
		if (Application.loadedLevel != PlayerPrefs.GetInt("Scene of savepoint", 0))
			leftSavepointScene = true;
		else
			leftSavepointScene = false;
	}

	void Reset ()
	{
		dead = false;
		levelToLoad = -1;
		maxHP = 0;
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
			if (go.name == "Player2")
				Destroy(go);
		//displayTime = 0;
		if (Application.loadedLevelName.Contains("AlvroirForest"))
			transform.Find("Camera").GetComponent<Skybox>().material = skybox1;
		else if (Application.loadedLevelName.Contains("2-"))
			transform.Find("Camera").GetComponent<Skybox>().material = skybox2;
		if (Application.loadedLevel == PlayerPrefs.GetInt("Scene", 0) && PlayerPrefs.GetInt("Scene of savepoint", 0) == Application.loadedLevel && !leftSavepointScene)
			transform.position = new Vector2(PlayerPrefs.GetInt("SpawnX", (int) spawnLoc.x), PlayerPrefs.GetInt("SpawnY", (int) spawnLoc.y));
		transform.localScale = Vector3.one;
		facingRight = true;
		rigidbody2D.velocity = Vector2.zero;
		Application.LoadLevel(PlayerPrefs.GetInt("Scene of savepoint", 0));
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		if (dead || levelToLoad != -1)
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
		if (dead)
		{
			if (GameObject.Find("World Wall").GetComponent<WorldWall>().alpha >= 1f)
				Reset ();
			return;
		}
		if (levelToLoad != -1)
		{
			if (GameObject.Find("World Wall").GetComponent<WorldWall>().alpha >= 1f)
				Application.LoadLevel(levelToLoad);
			return;
		}
		if (Input.GetKeyDown(KeyCode.S))
			inStats = !inStats;
		if (Time.timeScale == 1)
		{
			displayTime += Time.deltaTime;
		}
		if (grounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton3)))
		{
			rigidbody2D.AddForce(Vector2.up * jumpForce);
			grounded = false;
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
		if (paused)
		{
			time = Time.fixedTime - time;
			if (GUI.Button(new Rect(0, 0, 100, 25), "Resume"))
			{
				Time.timeScale = 1;
				paused = false;
			}
			else if (!showTimer && GUI.Button(new Rect(0, 75, 150, 25), "Show Speedrun Timer"))
			{
				showTimer = true;
			}
			else if (showTimer && GUI.Button(new Rect(0, 75, 150, 25), "Hide Speedrun Timer"))
			{
				showTimer = false;
			}
			else if (GUI.Button(new Rect(0, 100, 100, 25), "Clear Data"))
			{
				PlayerPrefs.DeleteAll();
				Application.LoadLevel(0);
				Destroy(gameObject);
			}
		}
		else
		{
			GUI.skin = guiSkin1;
			if (showTimer)
				GUI.Label(new Rect(0, 25, Screen.width, 50), "" + Mathf.Round(displayTime / numOfDecimalPlaces) * numOfDecimalPlaces);
			if (GUI.Button(new Rect(0, 0, 100, 25), "Menu"))
			{
				Time.timeScale = 0;
				paused = true;
			}
		}
		if (inStats)
		{
			Time.timeScale = 0;
			GUILayout.BeginArea(new Rect(200, 100, Screen.width - 400, Screen.height - 200));

			GUILayout.BeginHorizontal();

			GUILayout.BeginVertical();
			GUILayout.Box("Attribute");
			GUILayout.Box("Health");
			GUILayout.Box("Damage");
			GUILayout.Box("Haggling");
			GUILayout.EndVertical();

			GUILayout.BeginVertical();
			GUILayout.Box("Amount");
			GUILayout.Box("" + PlayerPrefs.GetFloat("MaxHP", 1));
			GUILayout.Box("" + PlayerPrefs.GetInt("Damage", 0));
			GUILayout.Box("" + PlayerPrefs.GetInt("Haggling", 0));
			GUILayout.EndVertical();

			GUILayout.BeginVertical();
			GUILayout.Box("Cost");
			GUILayout.Box("" + PlayerPrefs.GetFloat("MaxHP cost", 1));
			GUILayout.Box("" + PlayerPrefs.GetInt("Damage cost", 1));
			GUILayout.Box("" + PlayerPrefs.GetInt("Haggling cost", 1));
			GUILayout.EndVertical();

			GUILayout.BeginVertical();
			GUILayout.Space(30);
			GUILayout.Button("Buy", GUILayout.Width(100), GUILayout.Height(25));
			GUILayout.Space(2f);
			GUILayout.Button("Buy", GUILayout.Width(100), GUILayout.Height(25));
			GUILayout.Space(2f);
			GUILayout.Button("Buy", GUILayout.Width(100), GUILayout.Height(25));
			GUILayout.EndVertical();

			GUILayout.EndHorizontal();

			GUILayout.EndArea();
		}
		else if (!paused)
			Time.timeScale = 1;
	}
	

	public void ChangeHP (int amount)
	{
		hp += amount;
		if (hp <= .9 && !dead)
		{
			GameObject.Find("World Wall").GetComponent<WorldWall>().fadeMultiplier = -1f;
			GameObject.Find("World Wall").GetComponent<WorldWall>().alpha = 0f;
			dead = true;
		}
		else if (hp > PlayerPrefs.GetFloat("MaxHP", 1))
			hp = 1 - (PlayerPrefs.GetFloat("MaxHP", 1) % 1);
	}

	void OnApplicationQuit()
	{
		PlayerPrefs.SetInt("Playing", 0);
		PlayerPrefs.SetInt("Scene", PlayerPrefs.GetInt("Scene of savepoint", 0));
		PlayerPrefs.SetFloat("Timer", displayTime);
		PlayerPrefs.Save();
	}

	void OnLevelWasLoaded ()
	{
		Start ();
	}

	public void FadeOut (int level, Vector2 startLoc)
	{
		if (levelToLoad == -1)
		{
			GameObject.Find("World Wall").GetComponent<WorldWall>().fadeMultiplier = -1f;
			GameObject.Find("World Wall").GetComponent<WorldWall>().alpha = 0f;
			levelToLoad = level;
			levelEnterLoc = startLoc;
		}
	}
}
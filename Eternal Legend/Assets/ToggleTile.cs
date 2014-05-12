using UnityEngine;
using System.Collections;

public class ToggleTile : MonoBehaviour {
	public bool activeNow;
	public int initDelay;
	public int appearRate;
	public float initTimeMultiplier;
	public float timeMultiplier;
	public int disappearRate;
	int appearTimer;
	int disappearTimer;

	// Use this for initialization
	void Start ()
	{
		initDelay = Mathf.RoundToInt(initDelay * initTimeMultiplier);
		appearRate = Mathf.RoundToInt(appearRate * timeMultiplier);
		disappearRate = Mathf.RoundToInt(disappearRate * timeMultiplier);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.timeScale == 0)
			return;
		initDelay --;
		if (initDelay >= 0)
			return;
		if (activeNow)
		{
			if (name.Contains("(Wall Jump)"))
				transform.Find("Active (Wall Jump)").gameObject.SetActive(true);
			else
				transform.Find("Active").gameObject.SetActive(true);
			transform.Find("Inactive").gameObject.SetActive(false);
			disappearTimer ++;
			if (disappearTimer > disappearRate)
			{
				disappearTimer = 0;
				activeNow = false;
			}
		}
		else
		{
			if (name.Contains("(Wall Jump)"))
				transform.Find("Active (Wall Jump)").gameObject.SetActive(false);
			else
				transform.Find("Active").gameObject.SetActive(false);
			transform.Find("Inactive").gameObject.SetActive(true);
			appearTimer ++;
			if (appearTimer > appearRate)
			{
				appearTimer = 0;
				activeNow = true;
			}
		}
	}
}

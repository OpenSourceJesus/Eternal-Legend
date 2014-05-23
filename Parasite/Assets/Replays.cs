using UnityEngine;
using System.Collections;

public class Replays : MonoBehaviour {
 	public int currentElement;
	public ArrayList trms = new ArrayList();
	public ArrayList localScaleXs = new ArrayList();

	// Use this for initialization
	void Start ()
	{
		if (GameObject.Find("Replays") == null)
		{
			name = "Replays";
			currentElement = 0;
			DontDestroyOnLoad(gameObject);
		}
		if (GameObject.Find("Replays2") != null)
			Destroy(GameObject.Find("Replays2"));
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (Time.timeSinceLevelLoad < .0001)
			Start ();
		if (!GameObject.Find("Player").GetComponent<Player>().inReplay)
		{
			Transform trm = (Transform) GameObject.Find("Player").transform;
			trms.Add(trm.position);
			localScaleXs.Add(trm.localScale.x);
		}
		else if (currentElement < localScaleXs.Count - 1)
		{
			GameObject.Find("Player").transform.position = (Vector3) trms[currentElement];
			GameObject.Find("Player").transform.localScale = new Vector3((float) localScaleXs[currentElement], 1, 1);
			currentElement ++;
		}
		else
		{
			GameObject.Find("Player").GetComponent<Player>().inReplay = false;
			Time.timeScale = 0;
		}
	}
}

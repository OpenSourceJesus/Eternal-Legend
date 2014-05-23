using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour
{
	public string[] tips;
	public int currentElement;

	void Awake ()
	{
		DontDestroyOnLoad(transform.root.gameObject);
	}

	// Use this for initialization
	void Start ()
	{
		currentElement = PlayerPrefs.GetInt("Tip num", 0);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (transform.lossyScale.x < 0)
			transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
		GetComponent<TextMesh>().text = tips[currentElement];
		if (currentElement == 0 && transform.root.position.x > 10)
		{
			currentElement ++;
		}
		else if (currentElement == 1 && transform.root.position.x > 100)
		{
			currentElement ++;
		}
		else if (currentElement == 2 && transform.root.position.x > 150)
		{
			currentElement ++;
		}
		else if (currentElement == 3 && transform.root.position.x > 180)
		{
			currentElement ++;
		}
	}
}

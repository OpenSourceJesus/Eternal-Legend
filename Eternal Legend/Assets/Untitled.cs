using UnityEngine;
using System.Collections;

public class Untitled : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (GetComponent<SpriteRenderer>().color.a < .6 && GetComponent<SpriteRenderer> ().color.a > .4)
		{
			name = "GrassTile1 (Back 1)";
			if (GetComponent<SpriteRenderer>().sortingOrder == 0)
				GetComponent<SpriteRenderer>().sortingOrder = -32768;
		}
		else if (GetComponent<SpriteRenderer>().color.a < .3)
		{
			name = "GrassTile1 (Back 2)";
			if (GetComponent<SpriteRenderer>().sortingOrder == 0)
				GetComponent<SpriteRenderer>().sortingOrder = -32768;
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
}

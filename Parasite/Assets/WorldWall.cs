using UnityEngine;
using System.Collections;

public class WorldWall : MonoBehaviour
{
	public float alpha = 1f;
	public float alphaChangeRate = .01f;
	public float fadeMultiplier = 1f;

	// Use this for initialization
	void Start ()
	{
		fadeMultiplier = 1f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		alpha -= alphaChangeRate * fadeMultiplier;
		renderer.material.color = new Color(0, 0, 0, alpha);
	}
}

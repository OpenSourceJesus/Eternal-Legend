using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReadSceneNames : MonoBehaviour
{
	public string[] scenes;
	#if UNITY_EDITOR
	void Awake ()
	{
		if (GameObject.Find("ReadSceneNames") == null)
		{
			DontDestroyOnLoad(gameObject);
			name = "ReadSceneNames";
		}
	}

	void Start ()
	{
		if (GameObject.Find("ReadSceneNames2") != null)
			Destroy(GameObject.Find("ReadSceneNames2"));
	}

	private static string[] ReadNames()
	{
		List<string> temp = new List<string>();
		foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
		{
			if (S.enabled)
			{
				string name = S.path.Substring(S.path.LastIndexOf('/')+1);
				name = name.Substring(0,name.Length-6);
				temp.Add(name);
			}
		}
		return temp.ToArray();
	}
	[UnityEditor.MenuItem("CONTEXT/ReadSceneNames/Update Scene Names")]
	private static void UpdateNames(UnityEditor.MenuCommand command)
	{
		ReadSceneNames context = (ReadSceneNames)command.context;
		context.scenes = ReadNames();
	}
	
	private void Reset()
	{
		scenes = ReadNames();
	}
	#endif
}
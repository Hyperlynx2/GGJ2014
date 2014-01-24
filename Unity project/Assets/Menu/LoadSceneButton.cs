using UnityEngine;
using System.Collections;

public class LoadSceneButton : MonoBehaviour
{
	public float left;
	public float top;
	public float width;
	public float height;
	public string text;
	public string sceneName;
	public GUISkin skin;

	void OnGUI()
	{
		GUI.skin = skin;
		if(GUI.Button(new Rect(left, top, width, height), text))
		{
			Application.LoadLevel(sceneName);
		}
	}

}

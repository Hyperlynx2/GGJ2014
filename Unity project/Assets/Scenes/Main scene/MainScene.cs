using UnityEngine;
using System.Collections;

public class MainScene : MonoBehaviour
{
	//pass 'em straight through to GameManager
	public string[] menuItemScenes;
	public string[] menuItemLabels;
	public GUISkin guiSkin;

	// Use this for initialization
	void Start ()
	{
		GameManager instance = GameManager.GetInstance();
		
		instance.menuItemLabels = menuItemLabels;
		instance.menuItemScenes = menuItemScenes;
		instance.skin = guiSkin;		
	}
	
	void OnGUI()
	{
		GameManager.GetInstance().OnGUI();
	}
	
	void Update()
	{
		GameManager.GetInstance().Update();
	}
	
}

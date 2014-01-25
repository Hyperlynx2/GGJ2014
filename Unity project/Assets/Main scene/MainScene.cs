using UnityEngine;
using System.Collections;

public class MainScene : MonoBehaviour
{
	//pass 'em straight through to GameManager
	public string[] menuItemScenes;
	public string[] menuItemLabels;

	// Use this for initialization
	void Start ()
	{
		GameManager instance = GameManager.GetInstance();
		
		instance.menuItemLabels = menuItemLabels;
		instance.menuItemScenes = menuItemScenes;
		
	}
	
	void OnGUI()
	{
		GameManager.GetInstance().OnGUI();
	}
}

using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
	private static GameManager _instance = null;
	
	public static GameManager GetInstance()
	{
		return _instance;
	}
	
	public float left;
	public float top;
	public float width = 100;
	public float height = 100;
	
	/// <summary>
	/// Gap between buttons
	/// </summary>
	public float separation;
	
	public string[] menuItemScenes;
	public string[] menuItemLabels;
	public GUISkin skin;
	
	/// <summary>
	/// don't show the menu while in another scene.
	/// </summary>
	private bool _showMenu;
	
	// Use this for initialization
	void Start ()
	{
		if(_instance == null)
		{
			_instance = this;
		}
		else
		{
			throw new Exception("More than one GameManager instance! Very bad!");
		}
		
		_showMenu = true;
		
		DontDestroyOnLoad(gameObject);
	}
	
	void OnGUI()
	{
		if(_showMenu)
		{
			GUI.skin = skin;
			
			//god help you if you haven't got the labels and scenes right sized!
			for(int i = 0; i < menuItemLabels.Length; ++i)
			{
				//TODO: change to be percentage of screen rather than absolute position.
				if(GUI.Button(new Rect(left, top + i*(height + separation), width, height), menuItemLabels[i]))
				{
					_showMenu = false;
					Application.LoadLevel(menuItemScenes[i]);
				}
				
			}
		}
		
		
	}
	
	
}

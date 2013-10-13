using UnityEngine;
using System.Collections;

public class GUIControlScript : MonoBehaviour {
	
	//public GameObject SelectionTab;
	
	public GameObject masterCamera;
	
	private GameObject GUICamera;
	//private GUISkin customSkin;
	
	public GUIClicker[] buttons;
	
	public System.Collections.Generic.Dictionary<string, GUIEvent> events;
		
	//private int tabX;
	//private int tabY;
	
	private string text;
	
	// Use this for initialization
	void Start () 
	{
		text = "[no selection]";
		GUICamera = this.gameObject;
	}


	void OnGUI ()
	{
		//GUI.skin = customSkin;
	}
		
	void ToggleConsole()
	{
			
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		foreach(GUIClicker button in buttons)
		{
			if(button.checkEvent() ==  true && button.checkType() == "toggleConsole")
				ToggleConsole();
			
		}
	}
}

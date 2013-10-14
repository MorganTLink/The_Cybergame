using UnityEngine;
using System.Collections;

public class GUIOverlay : MonoBehaviour {
	//renders GUI
	void OnGUI(){
		if(GUI.Button(new Rect(0,0,100,20),"hello"))
		{
			Debug.Log("botat");
		}
	}

}

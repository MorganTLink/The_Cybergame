using UnityEngine;
using System.Collections;

public class GUIObject : MonoBehaviour {
	
	public string eventName;
	
	protected GameObject eventLocation;
	
	// Use this for initialization
	void Start () 
	{
		eventLocation = this.gameObject;
	}
	
	public virtual void activateEvent()
	{
		
	}

	
	// Update is called once per frame
	void Update () 
	{
	
	}
}

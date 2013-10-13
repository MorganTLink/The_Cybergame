using UnityEngine;
using System.Collections;

public class GUIEvent : MonoBehaviour {
	
	public string eventName;
	
	protected bool active;
	
	protected bool occured;	
	protected GameObject eventLocation;

	
	// Use this for initialization
	public virtual void Start () 
	{
		occured = false;
		eventLocation = this.gameObject;
	}
	
	public virtual bool activateEvent()
	{
		if(occured)
		{
			occured = false;
			return true;
		}
		else
		{
			occured = true;
			return false;
		}
	}
	
	public virtual string checkType()
	{
		return eventName;	
		
	}
	
	public virtual bool checkEvent()
	{
		return occured;
		
	}

	
	// Update is called once per frame
	void Update () 
	{
	
	}
}

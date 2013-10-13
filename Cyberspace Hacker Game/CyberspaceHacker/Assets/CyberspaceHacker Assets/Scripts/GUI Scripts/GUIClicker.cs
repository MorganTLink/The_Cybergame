using UnityEngine;
using System.Collections;

public class GUIClicker : GUIEvent 
{
	
	private bool clicked = false;
	
	private const float timeOut = 100;
	
	private float timePassed;
	
	// Use this for initialization
	void Start () 
	{
		timePassed = 0;
		
		base.Start();
	}
	
	void onMouseover()
	{
		if(Input.GetAxis("Fire1") > 0)
		{
		 	active = true;			
		}
	}
	
	public override bool checkEvent()
	{
		return clicked;	
		
	}

	public override bool activateEvent()
	{
		if(clicked)
		{
			clicked = false;
			return true;
		}
		
		return false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(active)
		{
			timePassed += Time.deltaTime;	
			
			if(timePassed >= timeOut)
			{
				active = false;
				timePassed = 0;
				
			}
			
		}
	}
}

using UnityEngine;
using System.Collections;

public class BarrierBehavior : NodeBehavior 
{
	//new public float accuracy = 0.05f;
	
	//new public float maxOffset = 0.5f;	
	//new public float moveSpeed = 3f;
	
	//new public float bobSpeed = 0.1f;
	//new public float bobRange = 0.1f;
	
	//new public bool inactive = false;
	

	private bool delayedPosition = false;
	
	private Vector3 myPosition;

	
	// Use this for initialization
	new void Start () 
	{
		inactive = false;
		delayedPosition = false;		
		detected = false;		
	}
	
	// Update is called once per frame
	new void Update () 
	{
		if(delayedPosition == false)
		{
			myPosition = (this.GetComponent("BarrierController") as BarrierController).MyPosition();
		
			start = myPosition;
			end = new Vector3(myPosition.x, myPosition.y + maxOffset, myPosition.z);
			center = myPosition;
			
			delayedPosition = true;
			
		}
		 if(inactive == false)
		{
			Vector3 oldPos = center;
			
			//Lowing back to normal Height
			if(!detected && center != start)
			{
				
				center = start;
				leveledOff = false;
				
			}
			
			if(!leveledOff && this.transform.position.y <= center.y + accuracy && this.transform.position.y >= center.y - accuracy)
			{
				leveledOff = true;
				this.transform.position = center;
				bobUpward = true;
			}
	
			if(leveledOff == false)
			{		
				this.transform.position = Vector3.Lerp (transform.position, center, moveSpeed * Time.deltaTime);
			}
			else
			{
				
				//Bob Effect
				if(this.transform.position.y >= (center.y + bobRange - accuracy))
					bobUpward = false;
				else if (this.transform.position.y <= (center.y - bobRange + accuracy))
					bobUpward = true;
				
				if(bobUpward)
				{
					this.transform.position = new Vector3 (transform.position.x, transform.position.y + (bobSpeed * Time.deltaTime), transform.position.z);
				}
				else
				{
					this.transform.position = new Vector3 (transform.position.x, transform.position.y - (bobSpeed * Time.deltaTime), transform.position.z);
				}	
			}
		}
		
	}
}

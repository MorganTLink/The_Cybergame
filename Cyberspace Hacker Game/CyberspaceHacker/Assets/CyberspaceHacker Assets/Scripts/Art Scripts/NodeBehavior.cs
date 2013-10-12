using UnityEngine;
using System.Collections;

public class NodeBehavior : MonoBehaviour 
{
	protected bool detected;
	
	public float accuracy = 0.05f;
	
	public float maxOffset = 0.75f;	
	public float moveSpeed = 3f;
	
	public float bobSpeed = 0.3f;
	public float bobRange = 0.25f;
	
	public bool inactive = false;
	
	protected bool bobUpward = true;
	protected bool leveledOff = true;

	protected Vector3 start;
	protected Vector3 center;
	protected Vector3 end;	

	
	// Use this for initialization
	protected void Start () 
	{
		inactive = false;
		start = transform.localPosition;
		end = new Vector3(transform.localPosition.x, transform.localPosition.y + maxOffset, transform.localPosition.z);
		center = transform.localPosition;
		
		detected = false;
	}
	
	public bool isDetected()
	{
		return detected;
	}
	
	protected void OnMouseOver ()
	{
		if(inactive == false)
		{
			Vector3 oldPos = center;
			//Raising to detected Height
			detected = true;	
			if(detected && center != end)
			{
					center = end;
					leveledOff = false;
			}	
		}
	}
	
	protected void OnMouseExit ()
	{
		detected = false;
	}
	
	// Update is called once per frame
	protected void Update () 
	{
		if(inactive == false)
		{
			Vector3 oldPos = center;
			
			//Lowing back to normal Height
			if(!detected && center != start)
			{
				
				center = start;
				leveledOff = false;
				
			}
			
			if(!leveledOff && this.transform.localPosition.y <= center.y + accuracy && this.transform.localPosition.y >= center.y - accuracy)
			{
				leveledOff = true;
				this.transform.localPosition = center;
				bobUpward = true;
			}
	
			if(leveledOff == false)
			{		
				this.transform.localPosition = Vector3.Lerp (transform.localPosition, center, moveSpeed * Time.deltaTime);
			}
			else
			{
				
				//Bob Effect
				if(this.transform.localPosition.y >= (center.y + bobRange - accuracy))
					bobUpward = false;
				else if (this.transform.localPosition.y <= (center.y - bobRange + accuracy))
					bobUpward = true;
				
				if(bobUpward)
				{
					this.transform.localPosition = new Vector3 (transform.localPosition.x, this.transform.localPosition.y + (bobSpeed * Time.deltaTime), transform.localPosition.z);
				}
				else
				{
					this.transform.localPosition = new Vector3 (transform.localPosition.x, this.transform.localPosition.y - (bobSpeed * Time.deltaTime), transform.localPosition.z);
				}	
			}
		}
	}
}

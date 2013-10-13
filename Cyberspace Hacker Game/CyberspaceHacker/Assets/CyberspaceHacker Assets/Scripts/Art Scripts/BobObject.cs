using UnityEngine;
using System.Collections;

public class BobObject : MonoBehaviour
{	
	public float bobSpeed = 0.3f;
	public float bobRange = 0.25f;
	
	public float accuracy = 0.05f;
	
	private bool bobUpward = true;
	private Vector3 center;

	
	// Use this for initialization
	void Start () 
	{
		center = transform.localPosition;

	}

	
	// Update is called once per frame
	void Update () 
	{				
		//Bob Effect
		if(this.transform.localPosition.y >= (center.y + bobRange - accuracy))
			bobUpward = false;
		else if (this.transform.localPosition.y <= (center.y - bobRange + accuracy))
			bobUpward = true;
		
		if(bobUpward)
		{
			this.transform.localPosition = new Vector3 (0, this.transform.localPosition.y + (bobSpeed * Time.deltaTime), 0);
		}
		else
		{
			this.transform.localPosition = new Vector3 (0, this.transform.localPosition.y - (bobSpeed * Time.deltaTime), 0);
		}
	}
}
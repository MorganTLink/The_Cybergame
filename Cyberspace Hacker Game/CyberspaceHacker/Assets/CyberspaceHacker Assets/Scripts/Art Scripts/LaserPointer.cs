using UnityEngine;
using System.Collections;



public class LaserPointer : MonoBehaviour 
{
	
	public Transform target = null;	
	
	void Start()
	{
		
	}
	
	public void Activate()
	{
		Start ();	
		this.Update ();
	}
	
	void Update() 
	{
	    LineRenderer lineRenderer = GetComponent<LineRenderer>();
	    lineRenderer.useWorldSpace = false;
	    lineRenderer.SetVertexCount(2);
			
	    RaycastHit hit;		
		
		if(target != null)
		{
			transform.LookAt(target.position);
		}

		Physics.Raycast(transform.position, transform.forward, out hit, (1 << 11));
		
	    if(hit.collider)
		{
	    	lineRenderer.SetPosition(1,new Vector3(0,0,hit.distance));
	    }
	    else
		{
	        lineRenderer.SetPosition(1,new Vector3(0,0,5000));
	    }
	}
}



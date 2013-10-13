using UnityEngine;
using System.Collections;

public class PingPacket : MonoBehaviour 
{
	
	public GameObject entryPoint;
	
	private float oldAxis;
	private float clickTolerance = 0.05f;
	
	private GameObject sendPacket;
	
	private float delayTime = 0.5f;
	private float timeWaiting = 0f;
	
	void Start()
	{
		
		sendPacket = Resources.Load("Prefab/Your_Packet", typeof(GameObject)) as GameObject;	
	}
	
	void Update()
	{	
		
		if(timeWaiting < 0f)
		{
			RaycastHit hit;
			
			if(Input.GetAxis("Fire2") - oldAxis > clickTolerance)
			{
				Ray target = Camera.main.ScreenPointToRay(Input.mousePosition);
	
				Physics.Raycast(target, out hit);
				
				if(hit.collider && hit.collider.gameObject.tag.Equals("touchTrigger"))
				{
					GameObject newPacket = GameObject.Instantiate(sendPacket) as GameObject;
					
					(newPacket.GetComponent("DataTransfer") as DataTransfer).startTrip(entryPoint, hit.collider.gameObject.transform.parent.gameObject);
					
					timeWaiting = delayTime;
					
				}
				
			}
		}
		else
		{
			timeWaiting -= Time.deltaTime;
			
		}

		
		oldAxis = Input.GetAxis("Fire1");
		
	}
}

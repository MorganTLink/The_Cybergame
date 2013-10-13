using UnityEngine;
using System.Collections;

public class RTSCamera : MonoBehaviour 
{	
	public int maxMovementSpeed = 15;
	public int smoothingSpeed = 5;
	public bool allowSideScroll = true;
	
	public int scrollZoneSize = 25;
	public int scrollZoneSpeed = 15;
	
	
	public int mouseScrollSpeed = 5;
	public int mouseDragSpeed = 5;
	public int keyScrollSpeed = 10;
	
	public int zoomingSpeed = 10;
	
	public int zoomMax = 60;
	public int zoomFar = 50;
	public int zoomNear = 20;
	public int zoomMin = 10;
	
	public int rotationSpeed = 10;
	public int nearCameraAngle = -15;
	public int farCameraAngle = 25;
	
	private Quaternion startRotation;
	private Quaternion farRotation;
	private Quaternion nearRotation;
	
	private Vector3 totalMoveCam = Vector3.zero;
	
	// Use this for initialization
	void Start () 
	{		
		startRotation = this.transform.rotation;
		farRotation = new Quaternion(transform.rotation.x, transform.rotation.y, Mathf.Deg2Rad*farCameraAngle, transform.rotation.w);
		nearRotation = new Quaternion(transform.rotation.x, transform.rotation.y, Mathf.Deg2Rad*nearCameraAngle, transform.rotation.w);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		int keySpeed = keyScrollSpeed;
		int zoneSpeed = scrollZoneSpeed;
		int dragSpeed = mouseDragSpeed;
		int maxSpeed = maxMovementSpeed;
		
		if(this.transform.position.y > zoomFar)
		{
			maxSpeed = maxSpeed * 2;
			keySpeed = keySpeed * 2;
			zoneSpeed = zoneSpeed * 2;
			dragSpeed = dragSpeed * 2;
		}
			
		
		Vector3 translateCam = Vector3.zero;
		
		//zoom via mouse wheel
		var zoomCam = Input.GetAxis("Mouse ScrollWheel") * zoomingSpeed * Time.deltaTime;		
		if(zoomCam != 0)
			translateCam = translateCam - (Vector3.up * zoomingSpeed * zoomCam);

		
		//scroll the screen by arrow keys or WASD
		if(Input.GetAxis("Vertical") < 0)
			translateCam += new Vector3(keySpeed * Time.deltaTime, 0, 0);							
		else if (Input.GetAxis("Vertical") > 0)
			translateCam -= new Vector3(keySpeed * Time.deltaTime, 0, 0);
		
		if (Input.GetAxis("Horizontal") > 0)
			translateCam += new Vector3(0, 0, keySpeed * Time.deltaTime);	
		else if (Input.GetAxis("Horizontal") < 0)
			translateCam -= new Vector3(0, 0, keySpeed * Time.deltaTime);
		
		//Scroll the Screen by middle mouse click
		if(Input.GetAxis("Fire3") != 0)
		{
			translateCam = translateCam - new Vector3(Input.GetAxis("Mouse Y") * dragSpeed * Time.deltaTime,
														0,
														-1 * Input.GetAxis("Mouse X") * dragSpeed * Time.deltaTime);
			
		}
		//scroll the screen by being near the edges
		else
		{
			if(allowSideScroll)
			{
				if(Input.mousePosition.x <= scrollZoneSize)
					translateCam -= Vector3.forward * zoneSpeed * Time.deltaTime;
				if(Input.mousePosition.x >= Screen.width - scrollZoneSize)
					translateCam += Vector3.forward * zoneSpeed * Time.deltaTime;
				
				if(Input.mousePosition.y >= scrollZoneSize)
					translateCam -= Vector3.right * zoneSpeed * Time.deltaTime;
				if(Input.mousePosition.y <= Screen.height - scrollZoneSize)
					translateCam += Vector3.right * zoneSpeed * Time.deltaTime;		
			}
		}
		
		Vector3 oldPosition = transform.position;
			
		//Add the new movement input to the "Stored" movement so the camera accelerates naturally
		Vector3 targetVect = totalMoveCam + translateCam;
		
		//Limit movment speed (use pythagorian theorm to find actual distance moved)
		if(Mathf.Sqrt((targetVect.x * targetVect.x)+(targetVect.z * targetVect.z)) <= maxSpeed)
		{
			totalMoveCam = targetVect;
		}
		//Stop movement accumulation and clear stored movement when the player gives no commands
		else if(translateCam == Vector3.zero)
			totalMoveCam = new Vector3(0, totalMoveCam.y, 0);

		
		//Limit Zoom Distance
		Vector3 newPosition = transform.position + totalMoveCam;
		if(newPosition.y > zoomMax)
		{
			totalMoveCam.y = 0;
			newPosition.y = zoomMax;
		}
		else if (newPosition.y < zoomMin)
		{
			totalMoveCam.y = 0;
			newPosition.y = zoomMin;
		}
		
		//Tilt the Camera when moved beyond the "Far" or "Near" distances
		if(newPosition.y > zoomFar)
		{			
			transform.rotation = Quaternion.Lerp(transform.rotation, farRotation, Time.deltaTime * rotationSpeed);
		}
		else if(newPosition.y < zoomNear)
		{
			transform.rotation = Quaternion.Lerp(transform.rotation, nearRotation, Time.deltaTime * rotationSpeed);			
		}
		else
		{
			transform.rotation = Quaternion.Lerp (transform.rotation, startRotation, Time.deltaTime * rotationSpeed);	
		}		
		
		//Lerp movement by expending "stored" moevment
		if(totalMoveCam != Vector3.zero)
  		 	transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * smoothingSpeed);
		
		//Add any unused movment to the "store" to be expended later
		totalMoveCam -= transform.position - oldPosition;

	
	
	}
}

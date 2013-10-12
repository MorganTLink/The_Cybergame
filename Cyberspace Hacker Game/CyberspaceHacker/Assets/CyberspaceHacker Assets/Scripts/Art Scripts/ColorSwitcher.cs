using UnityEngine;
using System.Collections;

public class ColorSwitcher : MonoBehaviour 
{	
	public GameObject blueIcon;
	public GameObject greenIcon;
	public GameObject redIcon;
	public GameObject yellowIcon;
	public GameObject greyIcon;
	
	private NodeController.SecurityClass securityClass = NodeController.SecurityClass.BLUE;
	private GameObject currentObject = null;
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	public void Shutdown()
	{
		Destroy (currentObject);
		currentObject = greyIcon;
		
		currentObject = (GameObject)(GameObject.Instantiate(greyIcon, transform.position, Quaternion.identity));
		currentObject.transform.parent = this.transform;
		
	}
	
	public void ColorSelect (NodeController.SecurityClass newClass)
	{
			securityClass = newClass;
		
			GameObject icon = blueIcon;
		
			if(securityClass == NodeController.SecurityClass.GREEN)
				icon = greenIcon;
			if(securityClass == NodeController.SecurityClass.RED)
				icon = redIcon;
			if(securityClass == NodeController.SecurityClass.GREY)
				icon = greyIcon;
			
			
			Destroy(currentObject);	
			currentObject = icon;
			
			currentObject = (GameObject)(GameObject.Instantiate(currentObject, this.transform.position, Quaternion.identity));
			currentObject.transform.parent = this.transform;		
	}
	
	// Update is called once per frame
	void Update () 
	{		
		if(currentObject == null)
			ColorSelect(securityClass);
	
	}
}

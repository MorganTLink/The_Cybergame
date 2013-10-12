using UnityEngine;
using System.Collections;

public class SwitchController : MonoBehaviour 
{

	public bool isLocked = true;
	
	public Material blueClass;
	public Material redClass;
	public Material greenClass;
	public Material greyClass;

	public GameObject inputNode;
	public GameObject[] outputNodes;	
	
	
	private Vector3 centerPoint;	
	private NodeController Base;	
	private NodeController.SecurityClass classification;
	
	// Use this for initialization
	void Start () 
	{
		if(this.gameObject.AddComponent(typeof(NodeController)) == null)
			Base = this.gameObject.AddComponent(typeof(NodeController)) as NodeController;
		else
			Base = this.gameObject.GetComponent(typeof(NodeController)) as NodeController;
		
		SetSecurityColor();

		Base.inputLink = inputNode;
		Base.outputLinks = outputNodes;
		
		Base.nodeType = NodeController.NodeType.SWITCH;
		
		Base.defaultSecurity = this.classification;
		
		Base.nodeIcon = null;
		Base.nodePedestal = null;
		
		//Base.ForceRedraw();
		
		this.Redraw();
	}
	
	private void SetSecurityColor()
	{
		Material nodeMaterial;
		
		ArrayList secClasses = new ArrayList();
		
		secClasses.Add((inputNode.GetComponent(typeof(NodeController)) as NodeController).getSecurityClass());
		
		foreach(GameObject node in outputNodes)			
		{
			secClasses.Add((node.GetComponent(typeof(NodeController)) as NodeController).getSecurityClass());			
		}
		
		bool isRed = false;
		bool isGrey = false;
		bool isGreen = true;
		foreach(NodeController.SecurityClass secClass in secClasses)
		{
			if(secClass == NodeController.SecurityClass.RED)
				isRed = true;
			
			if(secClass == NodeController.SecurityClass.GREY)
				isGrey = true; 
			
			if(secClass != NodeController.SecurityClass.GREEN)
				isGreen = false;
		}
		
		if(!isRed && isGreen && !isGrey)
		{
			classification = NodeController.SecurityClass.GREEN;
			nodeMaterial = greenClass;
		}
		else if(isRed && !isGreen && !isGrey)
		{
			classification = NodeController.SecurityClass.RED;
			nodeMaterial = redClass;
		}
		else if(!isRed && !isGreen && isGrey)
		{
			classification = NodeController.SecurityClass.GREY;
			nodeMaterial = greyClass;
		}
		else
		{
			classification = NodeController.SecurityClass.BLUE;
			nodeMaterial = blueClass;
		}

		this.renderer.material = nodeMaterial;
		
	}
	
	public Vector3 MyPosition()
	{
		return centerPoint;		
	}
	
	public void NewPosition(Vector3 position)
	{
		centerPoint = position;
		Redraw();
		
	}
	
	public void ForceRedraw()
	{
		Redraw();		
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
	
	void Redraw()
	{		
			
	}
}

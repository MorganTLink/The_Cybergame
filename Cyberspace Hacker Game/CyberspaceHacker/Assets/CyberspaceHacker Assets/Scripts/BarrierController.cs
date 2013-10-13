using UnityEngine;
using System.Collections;

public class BarrierController : MonoBehaviour 
{	
	public bool isLocked = true;
	
	public GameObject barrierClosed_Icon;
	public GameObject barrierOpen_Icon;
	public GameObject template_Icon;
	
	public GameObject inputNode;
	public GameObject outputNode;	
	
	private Vector3 centerPoint;
	
	private NodeController Base;	
	private GameObject myIcon;	
	private NodeController.SecurityClass classification;
	
	// Use this for initialization
	void Start () 
	{
		if(this.gameObject.AddComponent(typeof(NodeController)) == null)
			Base = this.gameObject.AddComponent(typeof(NodeController)) as NodeController;
		else
			Base = this.gameObject.GetComponent(typeof(NodeController)) as NodeController;
		
		Destroy(this.template_Icon);
		
		myIcon = barrierClosed_Icon;
		myIcon.transform.position = this.transform.position;

		
		classification = (inputNode.GetComponent("NodeController") as NodeController).getSecurityClass();
		centerPoint = inputNode.transform.position + ((outputNode.transform.position - inputNode.transform.position)/2);
		
		(this.GetComponent(typeof(LineRenderer)) as LineRenderer).material = SetSecurityColor();

		Base.inputLink = inputNode;
		Base.outputLinks = new GameObject[1];
		Base.outputLinks[0] = outputNode;
		
		Base.nodeType = NodeController.NodeType.BARRIER;
		
		Base.defaultSecurity = this.classification;
		
		Base.nodeIcon = null;
		Base.nodePedestal = null;
		
		//Base.ForceRedraw();
		
		this.Redraw();
	}
	
	Material SetSecurityColor()
	{
		
		NodeController.SecurityClass inputClass = (inputNode.GetComponent(typeof(NodeController)) as NodeController).getSecurityClass();
		
		if(inputClass == NodeController.SecurityClass.BLUE)
		{
			return (Resources.Load("Link_Materials/Link_Glow_Blue", typeof(Material)) as Material);			
		}
		else if(inputClass == NodeController.SecurityClass.GREEN)
		{
			return (Resources.Load("Link_Materials/Link_Glow_Green", typeof(Material)) as Material);	
		}
		else if(inputClass == NodeController.SecurityClass.RED)
		{
			return (Resources.Load("Link_Materials/Link_Glow_Red", typeof(Material)) as Material);				
		}
		else if(inputClass == NodeController.SecurityClass.GREY)
		{		
			return (Resources.Load("Link_Materials/Link_Glow_Grey", typeof(Material)) as Material);				
		}
		else
			return null;	
		
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
		if(myIcon.transform.parent != this.transform)
				myIcon.transform.parent = this.transform;

	}
	
	void Redraw()
	{		
		if(isLocked)
			myIcon = GameObject.Instantiate(barrierClosed_Icon) as GameObject;
		else
			myIcon = GameObject.Instantiate(barrierOpen_Icon) as GameObject;
		
		myIcon.transform.position = this.transform.position;
		myIcon.transform.parent = this.transform;
		
		this.transform.position = centerPoint;
		this.transform.LookAt(inputNode.transform);
			
	}
}

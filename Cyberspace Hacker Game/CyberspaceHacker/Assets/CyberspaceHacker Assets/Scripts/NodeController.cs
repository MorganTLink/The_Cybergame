using UnityEngine;
using System.Collections;

public class NodeController : MonoBehaviour 
{		
	public enum SecurityClass {BLUE, GREEN, RED, GREY};
	public enum NodeType {NODE, BARRIER, SWITCH, CLOUD};
	
	public bool poweredOn = true;
	public NodeType nodeType = NodeType.NODE;

	public SecurityClass defaultSecurity = SecurityClass.BLUE;
	
	public GameObject nodeIcon;
	public GameObject nodePedestal;

	public GameObject inputLink;
	public GameObject[] outputLinks;
	public GameObject cloudLink = null;
	
	private Light nodeLight;
	private BaseCubeGenerator baseGen;
	
	private float linkSpacer = 0.70f;
	private float barrierSpacer = 0.00001f;
	private float switchSpacer = 0.5f;
	protected float spacer;
	
	protected float delayLength = 0.01f;
	
	private GameObject ringHolder;
	private bool selected;
	private float timeDelay;
	
	protected GameObject selectionRing;	
	protected GameObject downwardLink;
		
	private GameObject upwardLink;
	
	protected float oldAxis;
	protected float clickTolerance = 0.1f;
		
	protected SecurityClass classification;	
	
	// Use this for initialization
	protected void Start () 
	{
		
		nodeLight = this.gameObject.GetComponentInChildren(typeof(Light)) as Light;
		baseGen = this.gameObject.GetComponent("BaseCubeGenerator") as BaseCubeGenerator;
		Initialize();
		
		if(nodeType == NodeType.BARRIER)
			spacer = barrierSpacer;
		else if (nodeType == NodeType.SWITCH)
			spacer = switchSpacer;
		else
			spacer = linkSpacer;
		
		downwardLink = Resources.Load("Prefab/Link_Decending", typeof(GameObject)) as GameObject;
		upwardLink = Resources.Load ("Prefab/Link_Uplink", typeof(GameObject)) as GameObject;
		selectionRing = Resources.Load("Prefab/Selection Circle", typeof(GameObject)) as GameObject;

		timeDelay = 0f;
		selected = false;	
		Redraw();
		
	}
	
	public void StartMe ()
	{
		Start ();	
	}
	
	public void Initialize()
	{
		if(!poweredOn)
			classification = SecurityClass.GREY;
		else
			classification = defaultSecurity;
		
	}
	
	public SecurityClass getSecurityClass()
	{
		return classification;	
		
	}
	
	public bool isSelected()
	{
		return selected;
	}
	
	public void shutdownNode()
	{
		poweredOn = false;
		Initialize();

		Redraw();
	}
	
	public void powerupNode()
	{
		poweredOn = true;
		Initialize();

		Redraw();
		
	}
	
		
	public void ForceRedraw()
	{
		Redraw();		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if(poweredOn && nodeType != NodeType.CLOUD && nodeType != NodeType.SWITCH)
		{
			if(nodeType == NodeType.BARRIER || nodeIcon == null)
			{
				nodeIcon = this.gameObject;	
				
			}

			if((nodeIcon.GetComponent("NodeBehavior") as NodeBehavior).inactive == true)
				(nodeIcon.GetComponent("NodeBehavior") as NodeBehavior).inactive = false;
				
			bool active = ((NodeBehavior)(nodeIcon.GetComponent("NodeBehavior"))).isDetected();
	
			bool click;
			if(Input.GetAxis("Fire1") - oldAxis > clickTolerance)
			{
				click = true;
			}
			else
			{
				click = false;
			}
			
			if(click && active && !selected)
			{
				ringHolder = (GameObject)(GameObject.Instantiate(selectionRing, transform.position, Quaternion.identity));
				ringHolder.transform.parent = this.transform;	
				selected = true;
				timeDelay = delayLength;
				
				if(nodeLight != null)
					nodeLight.enabled = true;					
				
				if(baseGen != null)
					baseGen.selectMyCubes();
				
			}
			else if(click && active && selected)
			{
				Destroy(ringHolder);
				selected = false;
				timeDelay = delayLength;
				
				if(nodeLight != null)
					nodeLight.enabled = false;					
				
				if(baseGen != null)
					baseGen.deselectMyCubes();
			}
			
			oldAxis = Input.GetAxis("Fire1");
		}
		else
		{
		
			if(nodeIcon != null && (nodeIcon.GetComponent("NodeBehavior") as NodeBehavior).inactive == false)
				(nodeIcon.GetComponent("NodeBehavior") as NodeBehavior).inactive = true;	
		}
	}
	
	protected void Redraw()
	{
		
		if(nodeType == NodeType.CLOUD && cloudLink != null)
		{
			GameObject uplink = GameObject.Instantiate(upwardLink, transform.position, Quaternion.identity) as GameObject;
			uplink.transform.parent = this.transform;
			
			(uplink.GetComponent("LaserPointer") as LaserPointer).Activate();
			uplink.transform.LookAt(cloudLink.transform);
			
			(uplink.GetComponent("LaserPointer") as LaserPointer).target = cloudLink.transform;						
		}
		else
		{
			if(nodeType == NodeType.NODE)
			{
				if(nodeIcon != null)
					((ColorSwitcher)(nodeIcon.GetComponent("ColorSwitcher"))).ColorSelect(classification);
				
				if(nodePedestal != null)
					((PedestalColorChanger)(nodePedestal.GetComponent("PedestalColorChanger"))).ColorSelect(classification);

			}
			
			GameObject newLink;
			SecurityClass targetClass;
			

			if(outputLinks != null && outputLinks.Length > 0)
			{
				foreach(GameObject node in outputLinks)
				{
					if(node != null && node.GetComponent("NodeController") != null)
					{
						(node.GetComponent("NodeController") as NodeController).Initialize();
						targetClass = (node.GetComponent("NodeController") as NodeController).classification;
						
						
						newLink = GameObject.Instantiate(downwardLink, transform.position, Quaternion.identity) as GameObject;
						newLink.transform.parent = this.transform;
						
						(newLink.GetComponent("LaserPointer") as LaserPointer).Activate();
						newLink.transform.LookAt(node.transform);
						newLink.transform.localPosition = newLink.transform.localPosition + 
							(new Vector3(newLink.transform.forward.x, 0, newLink.transform.forward.z) * spacer);
						
						((LaserPointer)(newLink.GetComponent("LaserPointer"))).target = node.transform;
						
						if(classification == SecurityClass.GREY) //If Grey, all links are grey
						{
							(newLink.GetComponent("LinkColorChanger") as LinkColorChanger).ColorSelect(SecurityClass.GREY);		
						}
						else if(targetClass == SecurityClass.GREY)
						{
							(newLink.GetComponent("LinkColorChanger") as LinkColorChanger).ColorSelect(SecurityClass.GREY);
							
						}
						else if (targetClass == SecurityClass.GREEN)
						{
							if(classification == SecurityClass.GREEN)
								(newLink.GetComponent("LinkColorChanger") as LinkColorChanger).ColorSelect(SecurityClass.GREEN);
							else
								(newLink.GetComponent("LinkColorChanger") as LinkColorChanger).ColorSelect(SecurityClass.BLUE);
						}
						else if(targetClass == SecurityClass.RED)
						{
							if(classification == SecurityClass.RED)
								(newLink.GetComponent("LinkColorChanger") as LinkColorChanger).ColorSelect(SecurityClass.RED);
							else
								(newLink.GetComponent("LinkColorChanger") as LinkColorChanger).ColorSelect(SecurityClass.BLUE);
						}
						else //If Blue, then make a blue link
						{
								(newLink.GetComponent("LinkColorChanger") as LinkColorChanger).ColorSelect(SecurityClass.BLUE);					
						}	
					}
				}
			}
		}
		
		
		
	}
}

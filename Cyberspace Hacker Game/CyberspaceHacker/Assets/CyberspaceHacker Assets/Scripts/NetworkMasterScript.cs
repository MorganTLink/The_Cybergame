using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkMasterScript : MonoBehaviour 
{
	public GameObject[] networkNodes;
	public int numOfCloudLinks = 1;
	public int networkDepth = 1;
	public int maxComplexity = 0;
	
	
	private Netspace network;
	private bool blankNet = true;
	
	// Use this for initialization
	public void Start () 
	{
		try
		{
				
			if(numOfCloudLinks < 1)
			{
				throw(new Exception_NotEnoughClouds());	
			}
			
			if(networkDepth < 1)
			{
				throw(new Exception_InvalidNetworkDepth());	
				
			}
			if(maxComplexity < 0)
			{
				throw(new Exception_InvalidNetworkComplexity());	
			}
			
			if(networkNodes != null && networkNodes.Length > 0)
			{
				blankNet = false;
				attachNetwork ();
			}
			
			constructNetwork();
		}
		catch(Exception_NotEnoughClouds exceptCld)
		{
			Debug.Log ("Network Creation Error, The given number of external links in numOfCloudLinks must be a positive non-zero integer");
			Debug.LogException(exceptCld);
			
		}
		catch(Exception_InvalidNetworkDepth exceptDpth)
		{
			Debug.Log ("Network Creation Error, The given network depth must be one or higher, it cannot be zero or negative!");
			Debug.LogException(exceptDpth);
			
		}
		catch(Exception_InvalidNetworkComplexity exceptCpx)
		{
			Debug.Log ("Network Creation Error, The given maximum network complexity must be a positive integer!");
			Debug.LogException(exceptCpx);
			
		}
		catch (System.Exception except)
		{
			Debug.Log ("Network Creation Error, the error type was a non-specific System.Exception");
			Debug.LogException(except);
		}
	}
	
	// Update is called once per frame
	public void Update () 
	{
	
	}
	
	private void attachNetwork()
	{
		List<NetNode> newNodes = new List<NetNode>();
		
		foreach(GameObject obj in networkNodes)
		{
			if(obj.GetComponent<NodeController>() != null)
			{				
				newNodes.Add(new NetNode(obj));				
			}					
		}
		
		network = new Netspace(newNodes.ToArray());
	}
	
	private void constructNetwork()
	{
		if(blankNet)
		{
			//establish entry points				
		}
		else
		{
			//establish additional entry points
		
			//locate established nodes
			
			//arrage established nodes
			
			//Repeat till no expansion points are found
				//locate expansion points
				
				//check if depth is bottomed out
					//if not then include additional expansion points in expansions
				
				//fill in expansion points
				
				//check if new expanded area includes new expansion points
			//Repat till no expansion points are found
			
			
			//arrage expansion points
			
			//insure quest goal is placed
				//if not then add quest goal
		
		}
	}
	
	private NetNode[] constructWorkstation()
	{
		List<NetNode> workstation = new List<NetNode>();
		
		return workstation.ToArray();
		
		
	}
	
	private NetNode[] constructMobilestation()
	{
		List<NetNode> mobile = new List<NetNode>();
		
		return mobile.ToArray();
		
	}
	
	private NetNode[] constructHypervisor()
	{
		List<NetNode> hypervisor = new List<NetNode>();
		
		return hypervisor.ToArray();		
	}
	
	private NetNode[] constructTerminal()
	{
		List<NetNode> terminal = new List<NetNode>();
		
		
		return terminal.ToArray();		
	}
	
	private NetNode[] constructServer()
	{
		List<NetNode> server = new List<NetNode>();
		
		return server.ToArray();
	}
	
}

// --------------- NETSPACE ----------------------------------------------------------------------------------------------------------------------------------

public class Netspace
{

	
	private Dictionary<NetNode, List<NetNode>> networkNodes;
	
	public Netspace()
	{
		networkNodes = new Dictionary<NetNode, List<NetNode>>();
	}
	
	public Netspace(NetNode[] nodes)
	{
		networkNodes = new Dictionary<NetNode, List<NetNode>>();
		Add(nodes);
		
	}

	public void Add(NetNode[] nodes)
	{
		foreach(NetNode node in nodes)
		{
			Add (node);	
		}
		
	}
	
	public void Clear()
	{
			networkNodes.Clear();		
	}
	
	public void Add(NetNode node)
	{
		List<NetNode> linksList = new List<NetNode>();
		
		if(node.HasLinks())
		{
			foreach(NetNode link in node.GetLinks())
			{
					linksList.Add (link);
			}
		}
		
		networkNodes.Add(node, linksList);
		
	}	
	
}

// -------------------- NETNODE ----------------------------------------------------------------------------------------

public class NetNode
{
	private GameObject nodeObject;	
	private NodeController nodeScript;	
	private string nodeName;
	private List<NetNode> links;
	private NodeType nodeType;
	private List<NodeContent> content;
	
	public Transform transform;
	
	public enum NodeType
	{
		//EXPANSION TYPES
		WORKSTATION = -19,
		TERMINAL = -18,
		MOBILE_STATION = -17,
		DMZ_NET = -16,
		INNER_NET = -15,
		SERVER_FARM = -14,
		COMM_SERVER = -13,
		CNC_SERVER = -12,
		JOB_SERVER = -11,
		STORAGE_SERVER = -10,
		NET_SERVER = -9,
		
		
		//BLANK TYPES
		JOB_SERVICE = -8,
		STORAGE_SERVICE = -7,
		SECURITY_SERVICE = -6,
		INTERFACE = -5,
		PORT = -4,
		COMM_HARDWARE = -3,
		SECURITY_HARDWARE = -2,
		JOB_HARDWARE = -1,		
		
		NONE = 0,
		
		//CLOUD TYPES
		CLOUD_WAN = 1,
		CLOUD_WWW = 2,
		
		//CLOUD UPLINKS
		UPLINK_MODEM = 3,
		UPLINK_DIRECT = 4,
		UPLINK_SAT = 5,
		UPLINK_BROADCAST = 6,
		
		//INTERFACES
		ROUTER = 7,
		SERVER_PORT = 8,
		WORKSTATION_PORT = 9,
		TERMINAL_PORT = 10,
		HYPERVISOR_PORT = 11,
		VPN_PORT = 12,
		WIRELESS_ACCESS = 13,
		MOBILE_PORT = 14,
		
		//BARRIERS
		FIREWALL_BARRIER = 15,
		ADMIN_BARRIER = 16,
		USER_BARRIER = 17,
		
		//SERVICES
		DATABASE_SERVICE = 18,
		FILE_TRANSFER = 19,
		MAIL_EXCHANGE = 20,
		GAME_SERVICE = 21,
		SUPERCOMPUTING = 22,
		VPN_SERVICE = 23,
		WEB_HOSTING = 24,
		AI_SERVICE = 25,
		HARWARE_TASKING = 26,
		VR_SERVICE = 27,
		MEDIA_SERVICE = 28,
		INTRUSION_DETECTION = 29,
		
		//SOFTWARE
		FILE_SYS = 30,
		RUNTIME_ENVR = 31,
		ANTI_MALWARE = 32,
		KERNEL = 33,

		//HARDWARE
		HARDWARE_SECURITY_CAMERA = 34,
		HARDWARE_SECURITY_SENSOR = 35,
		HARDWARE_SECURITY_LOCK = 36,
		HARDWARE_FAX = 37,
		HARDWARE_PHONE = 38,
		HARDWARE_UNSAFE = 39,
		HARDWARE_VALUABLE = 40,
		HARDWARE_VITAL = 41
		
	}
	
	public NetNode(string name, NodeType type)
	{
		nodeName = name;
		nodeType = type;

		GameObject instance = GameObject.Instantiate(new GameObject()) as GameObject;
		instance.AddComponent(typeof(NodeController));
		
		nodeObject = instance;
		nodeScript = nodeObject.GetComponent(typeof(NodeController)) as NodeController;
		transform = nodeObject.transform;
		
	}
	
	public NetNode(string name, NodeType type, GameObject node, NodeController script)
	{
		nodeName = name;
		nodeType = type;
		nodeObject = node;
		nodeScript = script;
		links = new List<NetNode>();
				
		if(nodeScript.inputLink != null)
			links.Add(new NetNode(nodeScript.inputLink));
		
		if(nodeScript.outputLinks.Length > 0)
		{
			foreach(GameObject link in nodeScript.outputLinks)
			{
				links.Add (new NetNode(link));
			}
		}
		
		transform = nodeObject.transform;
		
	}
	
	public NetNode(string name, NodeType type, GameObject node)
	{
		nodeName = name;
		nodeType = type;
		nodeObject = node;
		nodeScript = node.GetComponent(typeof(NodeController)) as NodeController;
		links = new List<NetNode>();
				
		if(nodeScript.inputLink != null)
			links.Add(new NetNode(nodeScript.inputLink));
		
		if(nodeScript.outputLinks.Length > 0)
		{
			foreach(GameObject link in nodeScript.outputLinks)
			{
				links.Add (new NetNode(link));
			}
		}
		
		transform = nodeObject.transform;
		
	}
	
	public NetNode(GameObject node)
	{
		nodeName = node.name;
		nodeObject = node;
		nodeScript = node.GetComponent(typeof(NodeController)) as NodeController;
				
		transform = nodeObject.transform;
		
	}
	
	public string GetName()
	{
		return nodeName;	
	}
	
	public NodeType GetType()
	{
		return nodeType;	
		
	}
	
	public bool HasLinks()
	{
		return (links.Count > 0);
		
	}
	
	public NetNode[] GetLinks()
	{
		return links.ToArray();
		
	}
	
	public void StartNode()
	{
		nodeScript.StartMe();
	}
	
	public void Initialize()
	{
		nodeScript.Initialize();
	}
	
	public NodeController.SecurityClass getSecurityClass()
	{
		return nodeScript.getSecurityClass();
		
	}
	
	public bool IsSelected()
	{
		return nodeScript.isSelected();
	}
	
	public void ShutdownNode()
	{
		nodeScript.shutdownNode();
	}
	
	public void PowerupNode()
	{
		nodeScript.powerupNode();
	}
	
		
	public void ForceRedraw()
	{
		nodeScript.ForceRedraw();	
	}
	
	
	
}

public class NodeContent
{
	private string contentName;
	private ContentType contentType;
	private bool avalible = true;
	
	public enum ContentType
	{
		HARDWARE,
		SOFTWARE,
		SERVICE,
		FILES,
		PROGRAMS,
		INTERFACE,
		DEFENSE,
		KERNEL,
		UPLINK,
		CLOUD,
		BARRIER,
		UNKNOWN,
		BLANK,
		NULL,
		NONE
		
	}
	
	public NodeContent(NetNode node)
	{
		contentName = node.GetName();
		contentType = SortNodeToContent(node.GetType());
		
	}
	
	public NodeContent(string name, NetNode.NodeType nodeType)
	{
		contentName = name;
		contentType = SortNodeToContent(nodeType);
	}
	
	public NodeContent(string name, ContentType content)
	{
		contentName = name;
		contentType = content;
	}
	
	public virtual void AccessContent()
	{
		
	}
	
	protected ContentType SortNodeToContent(NetNode.NodeType type)
	{
		if((int)(type) < (int)(NetNode.NodeType.WORKSTATION))
		{
			return ContentType.UNKNOWN;	
		}
		else if((int)(type) < 0)
		{
			return ContentType.BLANK;
		}
		else if(type == NetNode.NodeType.NONE)
		{
			return ContentType.NONE;	
		}
		else if (type == NetNode.NodeType.CLOUD_WAN || type == NetNode.NodeType.CLOUD_WWW)
		{
			return ContentType.CLOUD;	
		}
		else if ((int)(type) >= (int)(NetNode.NodeType.UPLINK_MODEM) || (int)(type) <= (int)(NetNode.NodeType.UPLINK_BROADCAST))
		{
			return ContentType.UPLINK;
		}
		else if ((int)(type) >= (int)(NetNode.NodeType.ROUTER) || (int)(type) <= (int)(NetNode.NodeType.MOBILE_PORT))
		{
			return ContentType.INTERFACE;
		}
		else if ((int)(type) >= (int)(NetNode.NodeType.FIREWALL_BARRIER) || (int)(type) <= (int)(NetNode.NodeType.USER_BARRIER))
		{
			return ContentType.BARRIER;
		}
		else if ((int)(type) >= (int)(NetNode.NodeType.DATABASE_SERVICE) || (int)(type) <= (int)(NetNode.NodeType.MEDIA_SERVICE))
		{
			return ContentType.SERVICE;
		}
		else if (type == NetNode.NodeType.INTRUSION_DETECTION)
		{
			return ContentType.DEFENSE;	
		}
		else if(type == NetNode.NodeType.FILE_SYS)
		{
			return ContentType.FILES;
		}
		else if(type == NetNode.NodeType.RUNTIME_ENVR)
		{
			return ContentType.PROGRAMS;
		}
		else if(type == NetNode.NodeType.ANTI_MALWARE)
		{
			return ContentType.DEFENSE;
		}
		else if ((int)(type) >= (int)(NetNode.NodeType.HARDWARE_SECURITY_CAMERA) || (int)(type) <= (int)(NetNode.NodeType.HARDWARE_VITAL))
		{
			return ContentType.HARDWARE;
		}
		else if((int)(type) > (int)(NetNode.NodeType.WORKSTATION))
		{
			return ContentType.UNKNOWN;	
		}
		else
		{
			return ContentType.NULL;	
		}
		
	}
	
	
}

// EXCEPTIONS ---------------------------------------------------------------------------------------------------------------------

public class Exception_NotEnoughClouds : System.Exception
{

	
}

public class Exception_InvalidNetworkDepth : System.Exception
{

	
}

public class Exception_InvalidNetworkComplexity : System.Exception
{
	
	
}
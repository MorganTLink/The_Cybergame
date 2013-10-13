using UnityEngine;
using System.Collections;

public class CubeBehavior : MonoBehaviour 
{

	public int gridX;
	public int gridY;
	public NodeController.SecurityClass color;
	
	public GameObject[] influences;
	
	private GameObject[] adjacent;
	
	private float naturalDisinfectChance = 0.30f;
	private float gridSize = NetGridSpace.gridSize;
	
	private bool isInfected = false;
	private Material currentColor;
	private int numOfInfections;
	
	private Material blue;
	private Material green;
	private Material red;
	private Material grey;
	
	private Material infectedBlue;
	private Material infectedRed;
	private Material infectedGreen;
	
	private ArrayList cmdNodes = new ArrayList();
	private ArrayList malware = new ArrayList();

	// Use this for initialization
	void Start () 
	{
		adjacent = new GameObject[4];
		isInfected = false;
		
		blue = Resources.Load("Cube_Materials/Cube_Blue", typeof(Material)) as Material;
		red = Resources.Load("Cube_Materials/Cube_Red", typeof(Material)) as Material;
		green = Resources.Load("Cube_Materials/Cube_Green", typeof(Material)) as Material;
		grey = Resources.Load("Cube_Materials/Cube_Grey", typeof(Material)) as Material;
		
		infectedBlue = Resources.Load ("Cube_Materials/Cube_Infected_Blue", typeof(Material)) as Material;
		infectedRed = Resources.Load ("Cube_Materials/Cube_Infected_Blue", typeof(Material)) as Material;
		infectedGreen = Resources.Load ("Cube_Materials/Cube_Infected_Blue", typeof(Material)) as Material;
		
		foreach(GameObject go in influences)
			cmdNodes.Add(go.GetComponent(typeof(BaseCubeGenerator)) as BaseCubeGenerator);	
		
		currentColor = blue;
		color = NodeController.SecurityClass.BLUE;
		
	}
	
		
	// Update is called once per frame
	public void checkCube() 	
	{		
		bool isSelected = false;
		
		foreach(BaseCubeGenerator bcg in cmdNodes)
		{
			if(bcg.isSelected())
				isSelected = true;			
		}
		
		if(isSelected)
			(this.GetComponent("Data_Cube_Bobing") as Data_Cube_Bobing).isSelected = true;
		else
			(this.GetComponent("Data_Cube_Bobing") as Data_Cube_Bobing).isSelected = false;
		
	}		
	
	public void forceStart()
	{
		this.Start();	
	}
	
	public void Redraw()
	{	
		Material myColor;
		Material infectedColor;
		
		
		if(malware.Count > 0)
			isInfected = true;
		else
			isInfected = false;
		
		if(color == NodeController.SecurityClass.BLUE)
		{
			myColor = blue;
			infectedColor = infectedBlue;
		}
		else if (color == NodeController.SecurityClass.GREEN)
		{
			myColor = green;	
			infectedColor = infectedGreen;
		}
		else if (color == NodeController.SecurityClass.RED)
		{
			myColor = red;
			infectedColor = infectedRed;			
		}
		else
		{
			myColor = grey;
			infectedColor = grey;
		}
		
		if(!isInfected)
			this.gameObject.renderer.material = myColor;	
		else
			this.gameObject.renderer.material = infectedColor;
		
	}
	
	public GameObject[] getAdjacent()
	{
		return adjacent;		
	}
	
	public void Initialize(int x, int y)
	{
		gridX = x;
		gridY = y;
		
		
		Collider[] overlap;
		int offX = 0;
		int offY = 0;
		
		for(int i = 0; i < adjacent.Length; i++)
		{
			if(i == 0)
			{
				offX = 1; offY = 0;
			}
			else if(i == 1)
			{
				offX = 0; offY = 1;
			}
			else if(i == 2)
			{
				offX = -1; offY = 0;	
			}
			else if(i == 3)
			{
				offX = 0; offY = -1;
			}
			
			overlap = Physics.OverlapSphere(getWorldPos(x + offX ,y + offY), (gridSize/2));
			
			foreach(Collider hit in overlap)
			{
				adjacent[i] = hit.gameObject;
			}
		
		}
	
		
		
		this.Start();		
	}
	
	public void addInfluence(BaseCubeGenerator newInfluence)
	{
		cmdNodes.Add(newInfluence);

		if(cmdNodes.Count > 0)
		{
			int rng = Random.Range(0, cmdNodes.Count);			
			color = (cmdNodes[rng] as BaseCubeGenerator).getMyColor();			
		}
		
		Redraw();
	}
	
	public void disinfect()
	{		
	}
	
	public BaseCubeGenerator[] myInfluences()
	{
		return (cmdNodes.ToArray() as BaseCubeGenerator[]);	
	}
	
	private Vector3 getWorldPos(int x, int y)
	{
		return new Vector3((float)(x*gridSize), (float)(influences[0].GetComponent("BaseCubeGenerator") as BaseCubeGenerator).groundLevel.position.y, (float)(y*gridSize));	
	}


}

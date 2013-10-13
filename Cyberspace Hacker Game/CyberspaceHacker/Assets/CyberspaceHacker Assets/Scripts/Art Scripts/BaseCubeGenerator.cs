using UnityEngine;
using System.Collections;

public class BaseCubeGenerator : MonoBehaviour 
{
	public int size;
	public Transform groundLevel;
	
	private float gridSize = NetGridSpace.gridSize;
	private GameObject dataCube;
	
	private float centerHeight = 4.0f;
	private float normalHeight = 0.0f;
	private float heightVariance = 2.0f;

	private Vector2[] adjGridPos;
	private Vector2 gridPos;

	private NodeController.SecurityClass myColor = NodeController.SecurityClass.BLUE;
	private bool selected = false;
	private ArrayList myCubes = new ArrayList();

	// Use this for initialization
	void Start () 
	{	
		dataCube = Resources.Load("Prefab/Data_Cube", typeof(GameObject)) as GameObject;		
		gridPos = getGridPos(transform.position.x, transform.position.z);

		generateBase(size);
		
	}
	
	public void selectMyCubes()
	{
		selected = true;
		
		foreach(CubeBehavior cube in myCubes)
		{
			cube.checkCube();			
		}
		
	}
	
	public void deselectMyCubes()
	{
		selected = false;
		
		foreach(CubeBehavior cube in myCubes)
		{
			cube.checkCube();	
		}
		
	}
	
	public bool isSelected()
	{
		return selected;		
	}
	
	public NodeController.SecurityClass getMyColor()
	{
		return myColor;	
	}
	
	public Vector2 getGridPos(float x, float z)
	{
		Vector2 myGridSpace = locSpace(new Vector3(x, 0, z));
		return myGridSpace;
	}
	
	private Vector3 getWorldPos(int x, int y)
	{
		return new Vector3((float)(x*gridSize), groundLevel.position.y, (float)(y*gridSize));	
	}

	private GameObject isFilled(int x, int y)
	{

		Collider[] overlap = Physics.OverlapSphere(getWorldPos(x,y), (gridSize/2));
		
		foreach(Collider hit in overlap)
		{
			return hit.gameObject;			
		}
		
		return null;	
	}
	
	private Vector2 locSpace(Vector3 loc)
	{
		int x = Mathf.FloorToInt(loc.x / gridSize);
		int y = Mathf.FloorToInt(loc.z / gridSize);
		
		return new Vector2(x,y);	
	}
	
	public void generateBase(int baseSize)
	{		
		GameObject instance;
		
		//if(myColor != (this.gameObject.GetComponent("NodeController") as NodeController).getSecurityClass())
		//	myColor = (this.gameObject.GetComponent("NodeController") as NodeController).getSecurityClass();
		
		size = baseSize;
		
		if(size <= 0)
			size = 1;		
		
		ArrayList validChoices = new ArrayList();
		
		Vector2 randomSpace;
		int sizeRemaining = size;
		int range = 0;
		int totalValidSpaces = 0;
		
		float rngF;
		int rngI;
		
		if(!isFilled((int)gridPos.x, (int)gridPos.y))
		{
			instance = GameObject.Instantiate(dataCube) as GameObject;
			(instance.GetComponent("CubeBehavior") as CubeBehavior).forceStart();
			(instance.GetComponent("CubeBehavior") as CubeBehavior).color = myColor;
			(instance.GetComponent("CubeBehavior") as CubeBehavior).addInfluence(this);
			
			instance.transform.Translate(getWorldPos((int)gridPos.x, (int)gridPos.y).x, 
										(groundLevel.position.y + centerHeight), 
										getWorldPos((int)gridPos.x, (int)gridPos.y).z);		
			
			myCubes.Add((instance.GetComponent("CubeBehavior") as CubeBehavior));
			
			instance = null;
		}
		
		sizeRemaining--;	
		
		totalValidSpaces = 0;

		while(sizeRemaining > 0)
		{
			if(totalValidSpaces <= 0)
			{
				range++;
				totalValidSpaces = 4*range;
				
				for(int i = 0; i < totalValidSpaces; i++)
					validChoices.Add(i);		
			}

			rngF = Random.Range(normalHeight - heightVariance, normalHeight + heightVariance);			
			rngI = Random.Range (0, totalValidSpaces);
			
			GameObject newObj = getSpace((int)(validChoices[rngI]), range);
			randomSpace = findSpace((int)(validChoices[rngI]), range);

			if(newObj != null)
			{
				CubeBehavior cube = newObj.GetComponent(typeof(CubeBehavior)) as CubeBehavior;
				cube.addInfluence(this);
				
				myCubes.Add (cube);
				
				totalValidSpaces--;
				validChoices.Remove(validChoices[rngI]);
				
			}			
			else
			{				
				newObj = GameObject.Instantiate(dataCube) as GameObject;
				newObj.transform.Translate(new Vector3(getWorldPos((int)(randomSpace.x), (int)(randomSpace.y)).x, 
																(groundLevel.position.y + rngF), 
																getWorldPos((int)(randomSpace.x), (int)(randomSpace.y)).z));
				
				CubeBehavior cube = newObj.GetComponent(typeof(CubeBehavior)) as CubeBehavior;
				

				
				cube.color = myColor;
				
				cube.gridX = (int)randomSpace.x;
				cube.gridY = (int)randomSpace.y;
				
				cube.forceStart();				
				cube.addInfluence(this);
				
				myCubes.Add (cube);
				
				sizeRemaining--;
				
				totalValidSpaces--;					
				validChoices.Remove(validChoices[rngI]);
			}
			
			if(range > 100)
				sizeRemaining = 0;
		}	
		
	}
	
	Vector2 findSpace(int target, int range)
	{
		int adjX = range;
		int adjY = 0;
		
		for(int i = 0; i < (target); i++)
		{	
			if(i < range)
			{
				adjX--;
				adjY++;
			}
			else if (i < range * 2)
			{
				adjX--;
				adjY--;
			}
			else if (i < range * 3)
			{
				adjX++;
				adjY--;
				
			}
			else
			{
				adjX++;
				adjY++;
			}			
		}
		
		return new Vector2((int)(gridPos.x + adjX), (int)(gridPos.y + adjY));
		
	}
					
	GameObject getSpace(int choice, int range)
	{
		int adjX = range;
		int adjY = 0;
		
		for(int i = 0; i < (choice); i++)
		{	
			if(i < range)
			{
				adjX--;
				adjY++;
			}
			else if (i < range * 2)
			{
				adjX--;
				adjY--;
			}
			else if (i < range * 3)
			{
				adjX++;
				adjY--;
				
			}
			else
			{
				adjX++;
				adjY++;
			}			
		}
		
		return isFilled((int)(gridPos.x + adjX), (int)(gridPos.y + adjY));

			
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}


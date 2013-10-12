using UnityEngine;
using System.Collections;

public class DataTransfer : MonoBehaviour 
{
	public GameObject startNode = null;
	public GameObject endNode = null;
	
	public GameObject currentNode = null;
	public GameObject targetNode = null;
	
	public bool returnTrip = false;
	public int pathIndex = 0;
	
	private System.Collections.Generic.List<GameObject> directions;	
	private Hashtable previous = new Hashtable();
	private System.Collections.Generic.List<GameObject> visited = new System.Collections.Generic.List<GameObject>();
	
	private float travelSpeed = 20.0f;
	private float accuracy = 0.5f;
	
	
	
	public void startTrip(GameObject startNode, GameObject endNode)
	{
		returnTrip = false;
		pathIndex = 0;
		this.endNode = endNode;
		this.startNode = startNode;
		
		
		directions = getPath(startNode, endNode);
		
		currentNode = startNode;
		targetNode = directions[pathIndex];
		
		this.transform.position = startNode.transform.position;
		
		
	}
	
	System.Collections.Generic.List<GameObject> getPath(GameObject start, GameObject end)
	{
		System.Collections.Generic.List<GameObject> path = new System.Collections.Generic.List<GameObject>();
		System.Collections.Generic.Queue<GameObject> q = new System.Collections.Generic.Queue<GameObject>();
		
		GameObject current = start;
		
		q.Enqueue(current);
		visited.Add(current);
		
		while(q.Count > 0)
		{
			current = q.Dequeue();
			
			if(current.Equals(end))
			{
				break;				
			}
			else
			{
				if((current.GetComponent("NodeController") as NodeController).nodeType == NodeController.NodeType.CLOUD)
				{
					GameObject node = (current.GetComponent("NodeController") as NodeController).cloudLink;
					
					if(!visited.Contains(node))
					{
						q.Enqueue(node);
						visited.Add(node);
						previous.Add(node, current);
					}
					
				}
				
				foreach(GameObject node in (current.GetComponent("NodeController") as NodeController).outputLinks)
				{
					if(!visited.Contains(node))
					{
						q.Enqueue(node);
						visited.Add(node);
						previous.Add (node, current);
					}
					
				}
				
			}
			
		}
		
		if(!current.Equals(end))
		{
			path = null;
		}
		
		for(GameObject node = end; node != null; node = (previous[node] as GameObject))
			path.Add(node);
		
		path.Reverse();		
		
		return path;
	}
	
	GameObject nextTarget()
	{
		if(returnTrip)
		{
			pathIndex--;
		}
		else
		{
			pathIndex++;
		}
		
		if(pathIndex < directions.Count && pathIndex >= 0)
			return (directions[pathIndex]);
		else if (pathIndex >= directions.Count)
			return directions[directions.Count - 1];
		else
			return directions[0];

		
	}
	
	bool closeEnough()
	{
			return((transform.position.x < targetNode.transform.position.x + accuracy && transform.position.x > targetNode.transform.position.x - accuracy)
				&& (transform.position.y < targetNode.transform.position.y + accuracy && transform.position.y > targetNode.transform.position.y - accuracy)
				&& (transform.position.z < targetNode.transform.position.z + accuracy && transform.position.z > targetNode.transform.position.z - accuracy));
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		this.transform.position = Vector3.MoveTowards(this.transform.position, targetNode.transform.position, Time.deltaTime * travelSpeed);	
		
		if(closeEnough())
		{

			if(currentNode == endNode)
			{
				returnTrip = true;
			}
			
			if(returnTrip == true)
			{
				if(currentNode == startNode)
				{
					Destroy(this.gameObject);
				}
			}
			
			currentNode = targetNode;
			this.transform.position = currentNode.transform.position;
			targetNode = nextTarget();
		}
		
	}
}

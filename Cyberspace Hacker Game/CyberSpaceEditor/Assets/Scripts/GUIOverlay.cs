using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GUIOverlay : MonoBehaviour {
	
	private enum SelectedDirection{x,y,z};
	
	SelectedDirection _selectedDirection;
	private List<Transform> _selectedObjects = new List<Transform>();
	private bool _positionObjects = true;
	
	private Vector2 _startingMousePosition = new Vector2(0,0);
	private Vector3 _startingObjectPosition = new Vector3(0,0,0);
	
	//renders GUI
	void OnGUI(){
		if(GUI.Button(new Rect(Camera.main.pixelWidth - 100,0,100,20),"export"))
		{
		}
		
		
		if(_positionObjects)
		{
			if(GUI.Button(new Rect(0,0,100,20),"position objects"))
			{
				_positionObjects = false;
			}

		}
		else
		{
			if(GUI.Button(new Rect(0,0,100,20),"nodify"))
			{
				_positionObjects = true;
			}
				
		}
		
		if(GUI.Button(new Rect(0,20,100,20),"Add Node"))
		{
			
			Object lnode =Instantiate(Resources.Load("Prefab/Node"),new Vector3(0,0,0),new Quaternion(0,0,0,0));
			lnode.name = "Node";
		}
		
		
		if(Input.GetMouseButtonDown(0))
		{
			Ray lcamerRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit lhitinfo;
			
			if(Physics.Raycast(lcamerRay,out lhitinfo,10000))
			{
				if(lhitinfo.transform.name == "Node")
				{
					if(_positionObjects)
						_unSelectNodes();
					
					// add node to list
					_addSelectedNode(lhitinfo.transform);
					
					_startingMousePosition = Input.mousePosition;
					_startingObjectPosition = new Vector3(lhitinfo.transform.position.x,lhitinfo.transform.position.y,lhitinfo.transform.position.z);
					
					if(_positionObjects)
					{
						foreach(Transform children in lhitinfo.transform)
						{
							if(children.collider.Raycast(lcamerRay,out lhitinfo,10000))
							{
								
								if(children.name == "X")
								{
									_selectedDirection = SelectedDirection.x;
									break;
								}
								else if(children.name == "Y")
								{
									_selectedDirection = SelectedDirection.y;
									break;
								}
								else if(children.name == "Z")
								{
									_selectedDirection = SelectedDirection.z;
									break;
								}
							}
						}

					}
				}
				else
				{
					_unSelectNodes();
				}
			}
			else
			{
				_unSelectNodes();
			}
		}

		
		if(_positionObjects)
		{
			
			if(_selectedObjects.Count > 0)
			{
				switch(_selectedDirection)
				{
					case SelectedDirection.x:
					_selectedObjects[0].position = new Vector3(_startingObjectPosition.x - (Input.mousePosition.y - _startingMousePosition.y),_startingObjectPosition.y,_startingObjectPosition.z);
					
						break;
					case SelectedDirection.y:
					_selectedObjects[0].position = new Vector3(_startingObjectPosition.x,_startingObjectPosition.y + (Input.mousePosition.y - _startingMousePosition.y),_startingObjectPosition.z);
						break;
					case SelectedDirection.z:
					_selectedObjects[0].position = new Vector3(_startingObjectPosition.x,_startingObjectPosition.y,_startingObjectPosition.z + (Input.mousePosition.x- _startingMousePosition.x));
						break;
				}
			}
		}
		else{
		}
		


	}
	
	private void _unSelectNodes()
	{
		for(int x = 0; x < _selectedObjects.Count; x++)
		{
			
			((NodeBehavior)_selectedObjects[x].GetComponent("NodeBehavior")).ToggleNode();
		}
		_selectedObjects.Clear();
	}
	
	private bool _addSelectedNode(Transform gm)
	{
		for(int x = 0; x < _selectedObjects.Count; x++)
		{
			if(_selectedObjects[x] == gm)
			{
				return false;
			}
		}
		((NodeBehavior)gm.GetComponent("NodeBehavior")).ToggleNode();
		_selectedObjects.Add(gm);
		return true;
	}
	

}






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
			else
			{
				this._sceneSelect();
			}
		}
		else
		{
			if(GUI.Button(new Rect(0,0,100,20),"nodify"))
			{
				_positionObjects = true;
			}
			else
			{
			
				this._sceneSelect();
			}
		}
		


	}
	
	private void _unSelectNodes()
	{
	}
	
	private void _sceneSelect()
	{
	
		
		if(Input.GetMouseButtonDown(0))
		{

			Ray lcamerRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit lhitinfo;

			if(Physics.Raycast(lcamerRay,out lhitinfo,10000))
			{
					//game is set to position so only one object can be selected at a time
					if(_positionObjects)
					_selectedObjects.Clear();
				
					if(lhitinfo.transform.name == "Node")
					{
					
					_startingMousePosition = Input.mousePosition;
					
					//add object to selected
					_selectedObjects.Add(lhitinfo.transform);
					_startingObjectPosition = new Vector3(lhitinfo.transform.position.x,lhitinfo.transform.position.y,lhitinfo.transform.position.z);
	
					foreach(Transform children in lhitinfo.transform)
					{
						if(children.collider.Raycast(lcamerRay,out lhitinfo,10000))
						{
							if(children.name == "X")
							{
								_selectedDirection = SelectedDirection.x;
							}
							else if(children.name == "Y")
							{
								_selectedDirection = SelectedDirection.y;
							}
							else if(children.name == "Z")
							{
								_selectedDirection = SelectedDirection.z;
							}
						}
					}
				}
			}
			else
			{
				
				_selectedObjects.Clear();
			}
		}
		else if(Input.GetMouseButtonUp(0))
		{
			//clear the nodes on selection mode with the mouse is released
			if(_positionObjects)
				_selectedObjects.Clear();
			
		}
		
		if(_positionObjects)
		{
			
			if(_selectedObjects.Count > 0){
				
				
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
			if(_selectedObjects.Count >= 2){
			}
		}
	
		
	}


}

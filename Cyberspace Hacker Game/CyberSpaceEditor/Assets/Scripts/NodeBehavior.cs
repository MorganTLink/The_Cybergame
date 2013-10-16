using UnityEngine;
using System.Collections;

public class NodeBehavior : MonoBehaviour {
	
	private bool _isToggled = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	

	
	}
	
	public void ToggleNode()
	{
		if(_isToggled)
		{
			foreach(Transform children in this.transform)
			{
				if(children.name == "Sphere" )
				{
					children.renderer.material.SetColor("_Color",Color.black);
				}
				_isToggled = false;
				
			}
		}
		else
		{
			foreach(Transform children in this.transform)
			{
				if(children.name == "Sphere" )
				{
					children.renderer.material.SetColor("_Color",Color.green);
					
				}
				_isToggled = true;
			}
		}
		
	}
}

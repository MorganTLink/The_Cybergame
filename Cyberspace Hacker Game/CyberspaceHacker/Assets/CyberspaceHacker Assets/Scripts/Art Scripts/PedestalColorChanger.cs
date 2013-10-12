using UnityEngine;
using System.Collections;

public class PedestalColorChanger : MonoBehaviour 
{
	
	private Material blueTexture;
	private Material greenTexture;
	private Material redTexture;
	//private Material yellowTexture;
	private Material greyTexture;
	

	private NodeController.SecurityClass currentClass;
	
	// Use this for initialization
	void Start () 
	{
		blueTexture = Resources.Load("Base_Materials/Node_Base_Blue", typeof(Material)) as  Material;
		greenTexture = Resources.Load ("Base_Materials/Node_Base_Green", typeof(Material)) as Material;
		redTexture = Resources.Load ("Base_Materials/Node_Base_Red", typeof(Material)) as Material;
		//yellowTexture = Resources.Load("Node_Base_Yellow", typeof(Material)) as Material;
		greyTexture = Resources.Load ("Base_Materials/Node_Base_Grey", typeof(Material)) as Material;
	}
	
	public void ColorSelect(NodeController.SecurityClass newClass)
	{
		if(currentClass != newClass)
		{
			currentClass = newClass;
			
			if(currentClass == NodeController.SecurityClass.BLUE)
			{
				this.renderer.material = blueTexture;				
			}
			else if(currentClass == NodeController.SecurityClass.GREEN)
			{
				this.renderer.material = greenTexture;
				
			}
			else if(currentClass == NodeController.SecurityClass.RED)
			{
				this.renderer.material = redTexture;
			}
			else if(currentClass == NodeController.SecurityClass.GREY)
			{
				this.renderer.material = greyTexture;
			}
			else
			{
				this.renderer.material = blueTexture;
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
}

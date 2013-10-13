using UnityEngine;
using System.Collections;
	
public class LinkColorChanger : MonoBehaviour 
{
	
	private Material blueTexture;
	private Material greenTexture;
	private Material redTexture;
	//private Material yellowTexture;
	private Material greyTexture;
	
	private NodeController.SecurityClass currentClass = NodeController.SecurityClass.BLUE;
	
	// Use this for initialization
	void Start () 
	{
		blueTexture = Resources.Load("Link_Materials/Link_Glow_Blue", typeof(Material)) as  Material;
		greenTexture = Resources.Load ("Link_Materials/Link_Glow_Green", typeof(Material)) as Material;
		redTexture = Resources.Load ("Link_Materials/Link_Glow_Red", typeof(Material)) as Material;
		//yellowTexture = Resources.Load("Link_Glow_Yellow", typeof(Material)) as Material;
		greyTexture = Resources.Load ("Link_Materials/Link_Glow_Grey", typeof(Material)) as Material;
		
		ColorSelect(currentClass);
	}
	
	public void ColorSelect(NodeController.SecurityClass newClass)
	{
		currentClass = newClass;

		if(currentClass == NodeController.SecurityClass.BLUE)
		{
			((LineRenderer)(this.GetComponent("LineRenderer"))).material = blueTexture;			
		}
		else if(currentClass == NodeController.SecurityClass.GREEN)
		{
			((LineRenderer)(this.GetComponent("LineRenderer"))).material = greenTexture;
			
		}
		else if(currentClass == NodeController.SecurityClass.RED)
		{
			((LineRenderer)(this.GetComponent("LineRenderer"))).material = redTexture;
		}
		else if(currentClass == NodeController.SecurityClass.GREY)
		{
			((LineRenderer)(this.GetComponent("LineRenderer"))).material = greyTexture;
		}
		else
		{
			((LineRenderer)(this.GetComponent("LineRenderer"))).material = blueTexture;
		}

	}
	
	// Update is called once per frame
	void Update () 
	{

	}
}

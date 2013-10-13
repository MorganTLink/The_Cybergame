using UnityEngine;
using System.Collections;

public class NetGridSpace : MonoBehaviour
{	
	public const float gridSize = 2.5f;
	
	public Transform groundLevel;
		
	public static System.Collections.Hashtable grid = new System.Collections.Hashtable();
	
	public bool exists = false;


	
	// Update is called once per frame
	void Update ()
	{
		if(this.renderer.enabled == false)
			this.renderer.enabled = true;
	}
}



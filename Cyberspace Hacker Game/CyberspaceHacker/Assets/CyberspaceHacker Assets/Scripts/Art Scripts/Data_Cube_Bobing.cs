using UnityEngine;
using System.Collections;

public class Data_Cube_Bobing : MonoBehaviour 
{
	public bool isSelected = false;
	
	public float bobSelectedSpeed = 1.0f;
	public float bobSelectedHeightIncrease = 4.0f;

	
	public float bobPokeSpeed = 1.0f;
	public float bobPokeHeightDecrease = 2.0f;
	
	public float bobNormalSpeed = 0.5f;
	public float bobNormalRange = 0.25f;
	
	public float accuracy = 0.025f;
	
	private Vector3 normalPos;
	private Vector3 selectedPos;
	private Vector3 pokePos;
	private Vector3 pokeAndSelectedPos;
	
	private bool bobUpward = true;
	private bool mousePoke = false;
	private Vector3 center;

	
	// Use this for initialization
	void Start () 
	{		
		center = transform.localPosition;
		
		normalPos = center;
		selectedPos = normalPos + new Vector3(0.0f, bobSelectedHeightIncrease, 0.0f);
		pokePos = normalPos - new Vector3(0.0f, bobPokeHeightDecrease, 0.0f);
		pokeAndSelectedPos = selectedPos - new Vector3(0.0f, bobPokeHeightDecrease, 0.0f);
		
	}
	
	void OnMouseOver ()
	{
		if(Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
		{
			mousePoke = true;
		}
	}

	
	// Update is called once per frame
	void Update () 
	{
		float speed;
		
				
		//Bob Effect
		if(this.transform.localPosition.y >= (center.y + bobNormalRange - accuracy))
			bobUpward = false;
		else if (this.transform.localPosition.y <= (center.y - bobNormalRange + accuracy))
			bobUpward = true;
		
		
		
		if(!isSelected)
		{
			if(mousePoke) //not selected and mouse over movement
			{
				speed = bobPokeSpeed;
				center.y = pokePos.y;
				
				if(this.transform.localPosition.y < pokePos.y + accuracy)
					mousePoke = false;
			}
			else //not selected and no mouse over movement
			{
				speed = bobNormalSpeed;
				center.y = normalPos.y;
			}
				
		}
		else
		{
			if(center.y < pokeAndSelectedPos.y - accuracy) 	//selected but not raised to correct height yet
			{
				center.y = selectedPos.y;
				speed = bobSelectedSpeed;
				bobUpward = true;
			}
			else
			{
				if(mousePoke) 	//selected, has raised to correct height and mouse over movement
				{
					speed = bobPokeSpeed;					
					center.y = pokeAndSelectedPos.y;
					
					if(this.transform.localPosition.y < pokeAndSelectedPos.y + accuracy)
						mousePoke = false;
				}
				else   	//selected, has raised to correct height but no mouse over movement
				{
					speed = bobNormalSpeed;					
					center.y = selectedPos.y;	
				}
			}
			
			
			
		}

		
		if(bobUpward)
			this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, new Vector3(center.x, center.y + bobNormalRange, center.z), speed * Time.deltaTime);
		else
			this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, new Vector3(center.x, center.y - bobNormalRange, center.z), speed * Time.deltaTime);
		
	}
	
}

	




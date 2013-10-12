using UnityEngine;
using System.Collections;

public class LightingControler : MonoBehaviour 
{
	
	public bool isAlertMode = false;
	
	public Material overlay;
	
			
	private float pulseSpeedRelaxed = 1f;
	private float pulseSpeedIntense = 5f;
	private float colorShiftTime = 5f;
	private float rapidShiftTime = 1f;
	
	
	public Color normalColor = Color.green;
	public Color altColor = Color.black;
	public Color alertColor = Color.red;
	

	private Color baseColor;
	
	private bool hitBaseColor = true;
	private float pulseSpeed;
	private Color currentColor;
	private Color targetColor;
	private float shiftTime;

	
	private float timeElapsed = 0f;
	
	// Use this for initialization
	void Start () 
	{
		targetColor = altColor;
		pulseSpeed = pulseSpeedRelaxed;
		baseColor = overlay.GetColor("_Color");
		currentColor = baseColor;	
		shiftTime = colorShiftTime;
	}
	
	// Update is called once per frame
	void Update () 
	{
		timeElapsed += Time.deltaTime;
		
		if(isAlertMode)
		{
			shiftTime = rapidShiftTime;
			pulseSpeed = pulseSpeedIntense;
			baseColor = alertColor;
			targetColor = altColor;
		}
		else
		{
			shiftTime = colorShiftTime;
			pulseSpeed = pulseSpeedRelaxed;
			baseColor = normalColor;
			targetColor = altColor;
		}
		
		if(hitBaseColor == true && currentColor != targetColor)
		{
			currentColor = Color.Lerp(currentColor, targetColor, pulseSpeed * Time.deltaTime);	
			
		}
		else if(currentColor != baseColor)
		{
			currentColor = Color.Lerp(currentColor, baseColor, pulseSpeed * Time.deltaTime);
			
		}
		
		if(hitBaseColor == true && timeElapsed > shiftTime)
		{
			hitBaseColor = false;
			timeElapsed = 0;
		}
		else if(hitBaseColor == false && timeElapsed > shiftTime)
		{
			hitBaseColor = true;
			timeElapsed = 0;
		}
		

		overlay.SetColor("_Color", currentColor);
	}
}

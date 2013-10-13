using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIConsole : GUIEvent 
{
		
	public string inputCmd = "";
	
	public float waitTime = 0.01f;
	public float slowWaitTime = 0.05f;
	public float hangTime = 0.1f;

	
	public float lineHeight = 15.35f;	
	public float cmdSpacer = 14f;
	public float cmdSize = 21f;
	public float screenLocH = 0.255f;
	public float cmdOffset = 8f;
	
	private int maxLines = 16;
	private float hSize = Screen.width;
	private float hPos = 1f;
	private float vPos = 400f;
	private float vSize = 275f;
	
	private bool consoleOn = false;
	private bool consoleWait = true;
	private bool consoleFreeze = true;
	private bool localConsole = false;
	
	private string consoleText;
	private float timer;
	
	public enum Commands
	{
		Void,
		Unknown,
		DontPrint,
		_Check,
		Activate,
		Deactivate,
		Exit,
		Dir,
		Scan,
		RunNet 
	}
	
	private string consoleInput;	
	private List<string> consolePast;
	private Queue<KeyValuePair<string, Commands>> consoleOut;

	//---------------------------------------------------------------------------------------------------------------------------------------------------
	
	private Dictionary<Commands, List<string>> commandList;
	
		// Use this for initialization
	public override void Start () 
	{
		consolePast = new List<string>();
		consoleOut = new Queue<KeyValuePair<string, Commands>>();
		

		
		commandList = new Dictionary<Commands, List<string>>();
		
		//EXIT
		List<string> cmd_Exit = new List<string>();
		cmd_Exit.Add ("exit"); cmd_Exit.Add ("quit"); cmd_Exit.Add ("shutdown"); cmd_Exit.Add("camp"); cmd_Exit.Add ("kill");
		commandList.Add (Commands.Exit, cmd_Exit);
		
		//DIR
		List<string> cmd_Dir = new List<string>();
		cmd_Dir.Add ("dir");
		commandList.Add (Commands.Dir, cmd_Dir);
		
		EnterCommand ("Anix Kernel 2.0.1.74,  Update Pak III");										//01
		EnterCommand ("Initilizing linknet IP sockets");											//02
		EnterCommand ("visavis: mode is protected IV, length=1200, pages=16");						//03
		EnterCommand ("visavis: mapped to 0xe390f000, memory logged at d190:001f");					//04
		EnterCommand ("gru: RGB color registered");													//05
		EnterCommand ("gru: switching to framebuffer at 0x00e10000");								//06
		EnterCommand ("tnytim: XEPA Generic 110 assumed; Override grinching");						//07
		EnterCommand ("Uni-Platform Multi-form ARC driver Version 9.1");							//08
		EnterCommand ("RAMDISK driver initialized: 32 RAM disks of 4096k size 2048 blocksize");		//09
		EnterCommand ("Zeus Ltd: Asyncronous P2P, id at 0xe050, 0x1f1, 0xe00 on rig 9");			//10
		EnterCommand ("WEB10 Anix TCP/IP v6 for WEB9 or higher");									//11
		EnterCommand ("BRWN: Hashtables hashed at configuration 1A");								//12
		EnterCommand ("BLITZ HHD Format checking...");	
		EnterCommand ("\t10%");
		EnterCommand ("\t12%");
		EnterCommand ("\t18%");
		EnterCommand ("\t28%");
		EnterCommand ("\t30%");
		EnterCommand ("\t32%");
		EnterCommand ("\t33%");
		EnterCommand ("\t35%");
		EnterCommand ("\t45%");
		EnterCommand ("\t60%");
		EnterCommand ("\t75%");
		EnterCommand ("\t83%");
		EnterCommand ("\t88%");
		EnterCommand ("\t91%");
		EnterCommand ("\t97%");
		EnterCommand ("\t98%");
		EnterCommand ("\t100%");
		EnterCommand ("BLITZ HDD check OK");
		EnterCommand ("PIXE: USP/Megabus controller at BCI slot 00:02.4");
		EnterCommand ("PIXE: firmare version 1.02.18, no updates avalible");
		EnterCommand ("PIXE: WARNING, a non-standard enviroment detected");
		EnterCommand ("AXIS Mass-CnC driver is online, checking for link...");
		EnterCommand ("...");
		EnterCommand ("...");
		EnterCommand ("...");
		EnterCommand ("AXIS Mass-CnC uplink found at port 660000");
		EnterCommand ("Boot directory logged at NULL:NULL");
		EnterCommand ("Shell command lib loaded");
		EnterCommand ("Shell advanced lib loaded");
		EnterCommand ("Shell custom libs loaded");
		EnterCommand("\n");
		EnterCommand("\n");
		EnterCommand("==================================");
		EnterCommand("   ###    ##    ## #### ##     ##	");
		EnterCommand("  ## ##   ###   ##  ##   ##   ##  ");
		EnterCommand(" ##   ##  ####  ##  ##    ## ##  	");
		EnterCommand("##     ## ## ## ##  ##     ###    ");
		EnterCommand("####### ##  ####  ##    ## ## 	");
		EnterCommand("##     ## ##   ###  ##   ##   ##	");
		EnterCommand("##     ## ##    ## #### ##     ## ");
		EnterCommand("==================================");
		EnterCommand("\n");						
		EnterCommand ("Anix config files loaded and checking integrity");
		EnterCommand ("Anix config files validated");
		EnterCommand("Anix Shell booted, Shell logs recorded at NULL:NULL");
		EnterCommand ("Bootdisk detected, running found config commands...");
		EnterCommand("linknet -l badid 2013:0f8d:a133:100f:0817:3c19:87e2:1337");
		EnterCommand("netsurfer badid -a jollyRoger");
		EnterCommand("\tenter login passcode: ******");
		EnterCommand("sudo jollyRoger -K -S");
		EnterCommand(">login badid");
		EnterCommand("\t**************");
		EnterCommand(">loadgui");
		EnterCommand(">loadnet");
		EnterCommand("\tconnecting to IP 2013:0f8d:a133:100f:0817:3c19:87e2:1337");
		EnterCommand("\tconnection completed, ping time: 12 ms");
	
		EnterCommand("", Commands.Activate);
		consoleFreeze = false;
		
		base.Start();
	}
	
	
	public void EnterCommand(string text)
	{
		Commands cmd;
		if(consoleOn)
			cmd = Commands._Check;
		else
			cmd = Commands.Void;
			
		consoleOut.Enqueue(new KeyValuePair<string, Commands>(text, cmd));
		if(consoleOn)
			consoleInput = text;	
	}
	
	public void EnterCommand(string text, Commands cmd)
	{
		consoleOut.Enqueue(new KeyValuePair<string, Commands>(text, cmd));
		if(consoleOn)
			consoleInput = text;	
		
	}
	
	public void activateCommand(Commands cmd, string text)
	{
		string origText = text;
		string[] parts = text.Split('>');
		text = parts[1];
		
		if(cmd != Commands.Void && cmd != Commands.Unknown)
		{
			if(cmd == Commands.Exit)
			{
				if(localConsole)
				{
					EnterCommand ("ANIX Shell shutting down... ", Commands.Void);
					EnterCommand("Shutting down system, logging record at NULL:NULL", Commands.Exit);
				}
				else
					EnterCommand ("\tShutting down scanner: LinkNet Version NULL, Netpack jollyRoger", Commands.Exit);
			}
			
		}
		else if(cmd == Commands.Unknown)
		{
			EnterCommand("\tUnknown Command \"" + text + "\"", Commands.Void);
		}
	}
	
	private Commands validateCommand(string text)
	{
		string origText = text;
		string[] parts = text.Split('>');
		text = parts[1];
		string textNormal = text.ToLower();
		
		foreach(KeyValuePair<Commands, List<string>> kvp in commandList)
		{			
			if(kvp.Value.Contains(textNormal) || kvp.Value.Contains(text))
			{
				return kvp.Key;
			}
		}

		return Commands.Unknown;
		
	}
	
	public void displayConsole()
	{			
		int limit;	
		Stack<string> consolePrint = new Stack<string> ();
		Stack<string> consoleFlip = new Stack<string> ();
			
		if(consolePast.Count > maxLines)
			limit = maxLines;
		else
			limit = consolePast.Count;
		
		if(limit > 0)
		{

			foreach(string cmd in consolePast)
			{
					consoleFlip.Push(cmd);	
			}
			
			for(int i = 0; i < limit; i++)
			{
					consolePrint.Push(consoleFlip.Pop());
				
			}

					
			consoleText = "";
			for(int i = 0; i < limit; i++)
			{
				string text = consolePrint.Pop();

				consoleText += text + "\n";

			}
		}
		
	}
	
	void OnGUI()
	{
		
		bool enterPressed = false;
		
		if(Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
			enterPressed = true;
		
		GUI.skin.box.imagePosition = ImagePosition.TextOnly;		
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		
		GUI.skin.textField.active.background = null;
		GUI.skin.textField.normal.background = null;
		GUI.skin.textField.hover.background = null;
		GUI.skin.textField.focused.background = null;


		GUI.Label(new Rect(hPos, vPos, hSize, vSize), consoleText);
		
		if(consoleOn && !consoleWait)
		{
						
			if (enterPressed)
			{
				EnterCommand((">" + inputCmd));	
				inputCmd = "";
				timer = 0;
			}
			
			GUI.Label(new Rect(hPos, vPos+vSize, hSize, cmdSize), ">", "Label");		
			inputCmd = GUI.TextField(new Rect(hPos + cmdOffset, vPos+vSize, hSize, cmdSize), inputCmd, "Label");			
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		vPos = Screen.height - (Screen.height * screenLocH);
		float baseSize = ((Screen.height * screenLocH) - cmdSpacer);
		maxLines = (int)(baseSize / lineHeight);
		vSize = maxLines * lineHeight;
		
		consoleWait = consoleFreeze;
		
		if(consoleFreeze == false && consoleOut.Count > 0)
		{
			consoleWait = true;
			if(timer > waitTime)
			{
				bool hasInput;					
				KeyValuePair<string, Commands> output = consoleOut.Dequeue();
				
				string text = output.Key;
				Commands cmd = output.Value;
				
				hasInput = false;
				foreach(char c in text.ToCharArray())
				{
					if(char.IsWhiteSpace(c) != true)
						hasInput = true;
					
				}
				
				
				if(cmd != Commands.DontPrint)
				{
					if(hasInput)
						consolePast.Add (text);
								
					if(cmd != Commands.Void)
					{
						if(cmd == Commands._Check)
						{

							
							if(hasInput)
								activateCommand(validateCommand(text), text);
						}
						else
							runCommand (cmd);
					}
				}
				
				timer = 0;
			}
			else
				timer += Time.deltaTime;

		}
		
		displayConsole();
			
	}
	
		
	private void runCommand(Commands cmd)
	{
		if(cmd == Commands.Activate)
			consoleOn = true;
		else if (cmd == Commands.Deactivate)
			consoleOn = false;
		//NETWORK ONLY COMMANDS
		else if (cmd == Commands.Scan)
		{
			EnterCommand("You used the command Scan, but it had no effect! <this is a test string>", Commands.Void);
			
		}
		//MIXED PURPOSE COMMANDS
		else if (cmd == Commands.Exit)
		{
			if(localConsole)
				Application.Quit();
			else
				localConsole = true;
		}
		//LOCAL ONLY COMMMANDS
		else if (cmd == Commands.Dir)
		{
			EnterCommand("You used the command Dir, but it had no effect! <this is a test string>", Commands.Void);
		}
		else if (cmd == Commands.RunNet && localConsole)
		{
			if(localConsole)
				localConsole = false;	
		}
		
	}
}

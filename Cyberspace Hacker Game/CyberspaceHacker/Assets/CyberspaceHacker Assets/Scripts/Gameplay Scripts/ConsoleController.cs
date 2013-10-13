using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConsoleController : MonoBehaviour 
{		
	public bool consoleOn = false;
	public bool consoleWait = true;
	public bool consoleFreeze = true;
	public bool localConsole = false;

	private Dictionary<Commands, List<string>> commandList;
	private Dictionary<string, Commands> commandOut;

		
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
	
	
	// Use this for initialization
	void Start () 
	{		
		commandList = new Dictionary<Commands, List<string>>();
		commandOut = new Dictionary<string, Commands>();
		
		//EXIT
		List<string> cmd_Exit = new List<string>();
		cmd_Exit.Add ("exit"); cmd_Exit.Add ("quit"); cmd_Exit.Add ("shutdown"); cmd_Exit.Add("camp"); cmd_Exit.Add ("kill");
		commandList.Add (Commands.Exit, cmd_Exit);
		
		//DIR
		List<string> cmd_Dir = new List<string>();
		cmd_Dir.Add ("dir");
		commandList.Add (Commands.Dir, cmd_Dir);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public Dictionary<string, Commands> giveCommand(Commands cmd, string text)
	{							
			if(cmd != ConsoleController.Commands.Void)
			{
				if(cmd == ConsoleController.Commands._Check)
				{
					activateCommand(validateCommand(text), text);
				}
				else
					runCommand (cmd);
			}
			
			Dictionary<string, Commands> cmdOut = new Dictionary<string, Commands>();
			foreach(KeyValuePair<string, Commands> kvp in commandOut)
				cmdOut.Add(kvp.Key, kvp.Value);
			commandOut.Clear();
			return cmdOut;

	}
		
	private void activateCommand(Commands cmd, string text)
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
					OutputCommand ("ANIX Shell shutting down... ", Commands.Void);
					OutputCommand("Shutting down system, logging record at NULL:NULL", Commands.Exit);
				}
				else
					OutputCommand ("\tShutting down scanner: LinkNet Version NULL, Netpack jollyRoger", Commands.Exit);
			}
			
		}
		else if(cmd == Commands.Unknown)
		{
			OutputCommand("\tUnknown Command \"" + text + "\"", Commands.Void);
		}
	}
	
	private void OutputCommand(string text, Commands cmd)
	{
		commandOut.Add(text, cmd);		
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
	
	private void runCommand(Commands cmd)
	{
		if(cmd == Commands.Activate)
			consoleOn = true;
		else if (cmd == Commands.Deactivate)
			consoleOn = false;
		//NETWORK ONLY COMMANDS
		else if (cmd == Commands.Scan)
		{
			OutputCommand("You used the command Scan, but it had no effect! <this is a test string>", Commands.Void);
			
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
			OutputCommand("You used the command Dir, but it had no effect! <this is a test string>", Commands.Void);
		}
		else if (cmd == Commands.RunNet && localConsole)
		{
				localConsole = false;	
		}
		
	}
	
}

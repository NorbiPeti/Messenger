using System;
using MSGer.tk;

public class Test : IScript
{
	public void Load()
	{
		Console.WriteLine("Script Loaded!");
	}
	public void Unload()
	{
		Console.WriteLine("Script Unloaded!");
	}
}

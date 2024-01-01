using Godot;
using System;

public partial class Main : Node2D
{
	public override void _Ready()
	{
		// Config.SaveGame();
		Config.LoadGame();
	}
	private void OnPlayPressed()
	{
		GetTree().ChangeSceneToFile("res://world.tscn");
	}

	private void OnQuitPressed()
	{
		GetTree().Quit();
	}
}

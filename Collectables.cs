using Godot;
using System;

public partial class Collectables : Node2D
{
	private const int YPosGround = 425;
	private readonly PackedScene Cherry = GD.Load<PackedScene>("res://Collectables/Cherry.tscn");	
	private int _cherrySeeds = 10;

	public void OnTimerTimeout()
	{
		if (_cherrySeeds > 0)
		{
			--_cherrySeeds;
			RandomNumberGenerator rng = new RandomNumberGenerator();
			int xPosRand = rng.RandiRange(50, 2250);
			Node2D newCherry = (Node2D)Cherry.Instantiate();
			newCherry.GlobalPosition = new Vector2(xPosRand, YPosGround);
			AddChild(newCherry);
		}
	}
}

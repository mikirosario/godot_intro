using Godot;
using System;

public partial class BG : ParallaxBackground
{
	private const float ScrollingSpeed = 100;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		ScrollOffset = new Vector2(ScrollOffset.X - ScrollingSpeed * (float)delta, ScrollOffset.Y);
	}
}

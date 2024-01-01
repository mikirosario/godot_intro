using Godot;
using System;

public abstract partial class Mob : CharacterBody2D, IMob, IDamager
{
	public const float DefaultSpeed = 50f;
	public float Speed { get; set; } = DefaultSpeed;
	public float Gravity { get; set; }
	public uint Attack { get; protected set; } = 0;
	public AnimatedSprite2D Animator { get; protected set; }
	protected Player PlayerCharacter { get; private set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PlayerCharacter = (Player)GetNode(Player.PlayerNodeLocation);
		Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

using Godot;
using System;

public partial class Cherry : Area2D
{
	public AnimatedSprite2D Animator { get; private set; }

	public override void _Ready()
	{
		Animator = (AnimatedSprite2D)GetNode("AnimatedSprite2D");
		Animator.Play("Idle");
	}

	public void OnBodyEntered(Node2D body)
	{
		if (body.Name == "Player")
		{			
			((Player)body).ReceiveGold(5);
			Tween positionTween = CreateTween();
			Tween visibilityTween = CreateTween();
			positionTween.TweenProperty(this, "global_position", GlobalPosition - new Vector2(0,30), 0.3);
			visibilityTween.TweenProperty(this, "modulate:a", 0, 0.3);
			positionTween.TweenCallback(Callable.From(QueueFree));
		}
	}
}

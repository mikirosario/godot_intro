using Godot;
using System;

public partial class Frog : Mob
{
	private enum State
	{
		IDLE,
		PATROL,
		CHASE,
		DEATH
	}
	private const uint BaseAttack = 3;

	private State _state = State.IDLE;

	public override void _Ready()
	{
		base._Ready();
		Attack = BaseAttack;
		Animator = (AnimatedSprite2D)GetNode("AnimatedSprite2D");
		Animator.Play("Idle");
	}

	private void OnPlayerDetectionBodyEntered(Node2D body)
	{
		if (body.Name == "Player")
		{
			Animator.Play("Jump");
			_state = State.CHASE;
		}
	}

	private void OnPlayerDetectionBodyExited(Node2D body)
	{
		if (body.Name == "Player")
		{
			Animator.Play("Idle");
			Velocity = Vector2.Zero;
			_state = State.IDLE;
		}
	}

	private void OnPlayerDeathBodyEntered(Node2D body)
	{
		if (body.Name == "Player")
		{
			PlayerCharacter.ReceiveGold(1);
			Death();
		}
	}

	private void OnPlayerCollisionBodyEntered(Node2D body)
	{
		if (body.Name == "Player" && _state != State.DEATH)
		{
			PlayerCharacter.ReceiveDamage(this);
			Death();	
		}
	}

	private void Death()
	{
		void DeleteSelf()
		{
			GD.Print("Frog croaked");
			Animator.AnimationFinished -= DeleteSelf;
			this.QueueFree();
		}
		if (_state != State.DEATH)
		{
			Config.SaveGame();
			Animator.AnimationFinished += DeleteSelf;
			Animator.Play("Death");
			Velocity = Vector2.Zero;
			_state = State.DEATH;
		}
	}
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add Gravity
		velocity.Y += Gravity * (float)delta;

		if (_state == State.CHASE)
		{
			Vector2 directionToPlayer = (PlayerCharacter.GlobalPosition - this.GlobalPosition).Normalized();
			velocity.X = directionToPlayer.X * Speed;			
			Animator.FlipH = directionToPlayer.X > 0;
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}

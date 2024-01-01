using Godot;
using System;
using System.Dynamic;

public partial class Player : CharacterBody2D, IDamageable
{
	public const string PlayerNodeLocation = "/root/World/Player/Player";
	public const uint MaxHealth = 10;
	public const uint MaxGold = 9999999;

	private enum State
	{
		IDLE,
		RUN,
		JUMP,
		FALL
	}

	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	private AnimationPlayer _animator;
	private State _state = State.IDLE;


	public uint Health { get => PlayerData.Health; private set => PlayerData.Health = value; }
	public uint Gold { get => PlayerData.Gold; private set => PlayerData.Gold = value; }

	private State GetNewState(Vector2 velocity)
	{
		switch (_state)
		{
			case State.IDLE:				
				if (velocity.Y < 0)
					return State.JUMP;
				else if (velocity.Y > 0)
					return State.FALL;
				else
					return State.RUN;
			case State.RUN:
				if (velocity.Y < 0)
					return State.JUMP;
				else if (velocity.Y > 0)
					return State.FALL;
				else
					return State.IDLE;
			case State.JUMP:
				if (!IsOnFloor())
					return State.FALL;
				else if (velocity.X == 0)
					return State.IDLE;
				else
					return State.RUN;
			case State.FALL:
				if (!IsOnFloor())
					return State.JUMP;
				else if (velocity.X == 0)
					return State.IDLE;
				else
					return State.RUN;
			default:
				return _state;
		}
	}

	private bool IsStateChangeFrame(Vector2 velocity)
	{
		switch (_state)
		{
			case State.IDLE:
			return velocity.X != 0 || velocity.Y != 0;
			case State.RUN:
			return velocity.X == 0 || !IsOnFloor();
			case State.JUMP:
			return velocity.Y >= 0;
			case State.FALL:
			return velocity.Y <= 0;
			default:
			return false;
		}
	}

	public uint ReceiveDamage(IDamager damager)
	{		
		Health -= Math.Min(damager.Attack, Health);
		LoseGold(1);
		return Health;
	}

	public uint ReceiveGold(uint receivedGold)
	{
		uint maxReceiveableGold = MaxGold - Gold;
		Gold += Math.Min(receivedGold, maxReceiveableGold);
		return Gold;
	}

	public uint LoseGold(uint lostGold)
	{
		Gold -= Math.Min(lostGold, Gold);
		return Gold;
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		// Add Gravity
		if (!IsOnFloor())
		{
			velocity.Y += gravity * (float)delta;
		}
		// Handle Jump
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{			
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		if (velocity.X < 0)
			((AnimatedSprite2D)GetNode("AnimatedSprite2D")).FlipH = true;
		else if (velocity.X > 0)
			((AnimatedSprite2D)GetNode("AnimatedSprite2D")).FlipH = false;

		if (IsStateChangeFrame(velocity))
		{
			GD.Print("State Change");
			_state = GetNewState(velocity);
			GD.Print(_state);
			switch (_state)
			{
				case State.IDLE:
				_animator.Play("Idle");
				break;
				case State.RUN:
				_animator.Play("Run");
				break;
				case State.JUMP:
				_animator.Play("Jump");
				break;
				case State.FALL:
				_animator.Play("Fall");
				break;
			}
		}
		Velocity = velocity;
		MoveAndSlide();
		if (Health == 0)
		{
			GD.Print("Changing scene");
			GetTree().ChangeSceneToFile("res://main.tscn");
		}
	}

    public override void _Ready()
    {
        _animator = (AnimationPlayer)GetNode("AnimationPlayer");
    }
}

using Godot;
using System;

public interface IMob
{
    float Speed { get; set; }
    float Gravity { get; set; }
    AnimatedSprite2D Animator { get; }
}

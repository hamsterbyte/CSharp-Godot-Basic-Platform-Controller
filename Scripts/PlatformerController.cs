using Godot;
using System;

public partial class PlatformerController : CharacterBody2D
{
    
    #region Process

    /// <summary>
    /// Called every physics update; default: 60fps
    /// </summary>
    /// <param name="delta"></param>
    public override void _PhysicsProcess(double delta){
        CalculateVelocity((float)delta);
        MoveAndSlide();
    }
    #endregion
    
    #region Gravity

    [Export] private float _gravity = 800f;
    [Export] private float _terminalVelocity = 512f;

    /// <summary>
    /// Applies the gravity to the character body
    /// </summary>
    /// <param name="vel"></param>
    /// <param name="delta"></param>
    private void ApplyGravity(ref Vector2 vel, float delta){
        if (IsOnFloor()) return;
        _previousVelocityY = vel.Y;
        _newVelocityY = vel.Y + _gravity * delta;
        _newVelocityY = Mathf.Clamp(_newVelocityY, -Mathf.Inf, _terminalVelocity);
        vel.Y = (_previousVelocityY + _newVelocityY) * 0.5f;
    }

    #endregion
    
    #region Velocity

    private Vector2 _velocity;
    private float _previousVelocityY;
    private float _newVelocityY;

    /// <summary>
    /// Calculate the bi-directional of the character body and apply it
    /// </summary>
    /// <param name="delta"></param>
    private void CalculateVelocity(float delta){
        _velocity = Velocity;
        CalculateVelocityY(ref _velocity, delta);
        Velocity = _velocity;
    }

    /// <summary>
    /// Calculates the Y velocity of the character body
    /// </summary>
    /// <param name="vel"></param>
    /// <param name="delta"></param>
    private void CalculateVelocityY(ref Vector2 vel, float delta){
        ApplyGravity(ref vel, delta);
    }

    #endregion

}

using Godot;
using System;

public partial class PlatformerController : CharacterBody2D
{
    
    #region Process
    
    public override void _PhysicsProcess(double delta){
        CalculateVelocity((float)delta);
        MoveAndSlide();
    }
    #endregion
    
    #region Gravity

    [Export] private float _gravity = 800f;
    [Export] private float _terminalVelocity = 600f;

    /// <summary>
    /// Apply gravity to the character body velocity
    /// </summary>
    /// <param name="vel"></param>
    /// <param name="delta"></param>
    private void ApplyGravity(ref Vector2 vel, float delta){
        if (IsOnFloor()) return;
        _previousVelocityY = vel.Y;
        _newVelocityY = vel.Y + _gravity * delta;
        _newVelocityY = Mathf.Clamp(_newVelocityY, -Mathf.Inf, _terminalVelocity);
        vel.Y = (_previousVelocityY + _newVelocityY) * .5f;
    }

    #endregion
    
    #region Velocity

    private Vector2 _velocity;
    private float _previousVelocityY, _newVelocityY;

    /// <summary>
    /// Calculate the bi-directional velocity and apply it to the character body
    /// </summary>
    /// <param name="delta"></param>
    private void CalculateVelocity(float delta){
        _velocity = Velocity;
        CalculateVelocityY(ref _velocity, delta);
        Velocity = _velocity;
    }

    private void CalculateVelocityY(ref Vector2 vel, float delta){
        ApplyGravity(ref vel, delta);
    }

    #endregion

}
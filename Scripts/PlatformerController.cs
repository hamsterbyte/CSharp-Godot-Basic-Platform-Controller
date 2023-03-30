using Godot;
using System;

public partial class PlatformerController : CharacterBody2D{
    #region PROCESS

    /// <summary>
    /// Called every physics frame; default: 60fps
    /// </summary>
    /// <param name="delta">Time passed since last physics frame</param>
    public override void _PhysicsProcess(double delta){
        CalculateVelocity((float)delta);
        MoveAndSlide();
    }

    #endregion

    #region GRAVITY

    [Export]private float _gravity = 800f;

    /// <summary>
    /// Apply gravity to the character body
    /// </summary>
    /// <param name="vel"></param>
    /// <param name="delta"></param>
    private void ApplyGravity(ref Vector2 vel, float delta){
        _previousVelocityY = vel.Y;
        _newVelocityY += vel.Y + _gravity * delta;
        vel.Y = (_previousVelocityY + _newVelocityY) * .5f;
    }

    #endregion

    #region VELOCITY

    private Vector2 _velocity;
    private float _previousVelocityY;
    private float _newVelocityY;

    /// <summary>
    /// Calculate and apply bi-directional velocity to the character body
    /// </summary>
    /// <param name="delta"></param>
    private void CalculateVelocity(float delta){
        _velocity = Velocity;
        CalculateVelocityY(ref _velocity, delta);
        Velocity = _velocity;
    }

    /// <summary>
    /// Calculate the vertical velocity of the character body
    /// </summary>
    /// <param name="vel"></param>
    /// <param name="delta"></param>
    private void CalculateVelocityY(ref Vector2 vel, float delta){
        ApplyGravity(ref vel, delta);
    }
    #endregion
}
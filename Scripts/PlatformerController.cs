using Godot;
using System;
using BasicPlatformController.Scripts;

public partial class PlatformerController : CharacterBody2D
{
    
    #region Process

    /// <summary>
    /// Called ever frame
    /// </summary>
    /// <param name="delta"></param>
    public override void _Process(double delta){
        GatherInput();
    }
    
    /// <summary>
    /// Called every physics frame; default: 60fps
    /// </summary>
    /// <param name="delta"></param>
    public override void _PhysicsProcess(double delta){
        CalculateVelocity((float)delta);
        MoveAndSlide();
    }
    #endregion
    
    #region Input

    private Vector2 _input;
    //Cache input StringName's to prevent excessive calls to Godot API
    private StringName _up = new StringName("Up"), _left = new StringName("Left"), _down = new StringName("Down"), _right = new StringName("Right");

    private void GatherInput(){
        _input = Input.GetVector(_left, _right, _up, _down).GetRaw();
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
        CalculateVelocityX(ref _velocity);
        Velocity = _velocity;
    }

    /// <summary>
    /// Calculate vertical velocity of the character body
    /// </summary>
    /// <param name="vel"></param>
    /// <param name="delta"></param>
    private void CalculateVelocityY(ref Vector2 vel, float delta){
        ApplyGravity(ref vel, delta);
    }

    private void CalculateVelocityX(ref Vector2 vel){
        vel.X = _input.X * 100;
    }

    #endregion

}
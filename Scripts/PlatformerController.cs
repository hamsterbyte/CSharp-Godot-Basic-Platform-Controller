using System;
using Godot;

public partial class PlatformerController : CharacterBody2D{
    #region Initialize

    /// <summary>
    /// Called once at start
    /// </summary>
    public override void _Ready(){
        GatherRequirements();
        SetupSprite();
    }

    /// <summary>
    /// Gather required nodes for script function
    /// </summary>
    private void GatherRequirements(){
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    #endregion

    #region Process

    /// <summary>
    /// Called ever frame
    /// </summary>
    /// <param name="delta"></param>
    public override void _Process(double delta){
        GatherInput();
        FlipSprite();
        InterpolateSprite();
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

    #region Interpolation

    private Vector2 _spriteOffset;

    /// <summary>
    /// Set the sprite offset based on relative anchor position
    /// </summary>
    private void SetupSprite(){
        _sprite.TopLevel = false;
        _spriteOffset = 16 * Vector2.Up;
    }

    /// <summary>
    /// Interpolate sprite position based on character body position
    /// </summary>
    private void InterpolateSprite(){
        _sprite.GlobalPosition = _sprite.GlobalPosition.Lerp(GlobalPosition + _spriteOffset,
            (float)Engine.GetPhysicsInterpolationFraction());
    }

    #endregion

    #region Input

    [Export] private bool _useRawInput = true;
    private Vector2 _input;
    private bool _didJump;

    /// <summary>
    /// Gather all required input from the player
    /// </summary>
    private void GatherInput(){
        _input = Input.GetVector(InputNames.Left, InputNames.Right, InputNames.Up, InputNames.Down);
        if (_useRawInput) _input = _input.GetRaw();
        if (Input.IsActionJustPressed(InputNames.Jump)){
            _didJump = true;
        }
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

    [Export] private float _moveSpeed = 200f;
    [Export] private float _acceleration = 7f;
    [Export] private float _deceleration = 10f;

    private Vector2 _velocity;
    private float _targetVelocityX;
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
        HandleJump(ref vel);
        ApplyGravity(ref vel, delta);
    }

    /// <summary>
    /// Calculate the X velocity of the character body
    /// </summary>
    /// <param name="vel"></param>
    private void CalculateVelocityX(ref Vector2 vel){
        _targetVelocityX = _input.X * _moveSpeed;
        vel.X = Mathf.MoveToward(vel.X, _targetVelocityX,
            Mathf.Sign(_input.X) == Mathf.Sign(_velocity.X) ? _acceleration : _deceleration);
    }

    #endregion

    #region Jump

    [Export] private float _jumpVelocity = -300f;

    /// <summary>
    /// Apply jump force to the character velocity
    /// </summary>
    private void HandleJump(ref Vector2 vel){
        if (!IsOnFloor() || !_didJump) return;
        _didJump = false;
        vel.Y = _jumpVelocity;
    }

    #endregion

    #region Animation

    private AnimatedSprite2D _sprite;

    /// <summary>
    /// Flip the character sprite based on input direction
    /// </summary>
    private void FlipSprite(){
        _sprite.FlipH = _input.X switch{
            < 0 => true,
            > 0 => false,
            _ => _sprite.FlipH
        };
    }

    #endregion
    
}
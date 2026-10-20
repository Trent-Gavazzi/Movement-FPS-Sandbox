using Godot;

public partial class PlayerUpdated : CharacterBody3D
{
    public enum PlayerState { Standing, Jumping, Crouching }

    public PlayerState CurrentMoveState { get; private set; } = PlayerState.Standing;

    [Export] public float MoveSpeed { get; set; } = 5.0f;
    [Export] public float TurnSpeed { get; set; } = 10.0f;
    [Export] public float JumpVelocity { get; set; } = 4.5f;
    [Export] public Node3D Model { get; set; }
    [Export] public Node3D CameraPivot { get; set; }

    private float _gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;
        if (!IsOnFloor())
            velocity.Y -= _gravity * (float)delta;
        Velocity = velocity;

        PlayerState newState = HandleNewState(CurrentMoveState);
        if (newState != CurrentMoveState)
        {
            EnterState(newState);
            CurrentMoveState = newState;
        }

        MoveAndSlide();
    }

    private PlayerState HandleNewState(PlayerState curState)
    {
        switch (curState)
        {
            case PlayerState.Standing:
                if (Input.IsActionPressed("crouch")) return PlayerState.Crouching;
                if (Input.IsActionJustPressed("jump") && IsOnFloor()) return PlayerState.Jumping;
                return curState;

            case PlayerState.Jumping:
                if (IsOnFloor() && Velocity.Y <= 0f) return PlayerState.Standing;
                return curState;

            case PlayerState.Crouching:
                if (Input.IsActionJustReleased("crouch")) return PlayerState.Standing;
                return curState;

            default:
                GD.PushWarning($"Unhandled state: {curState}");
                return PlayerState.Standing;
        }
    }

    // Runs once on the frame a state is entered.
    private void EnterState(PlayerState newState)
    {
        GD.Print($"{CurrentMoveState} -> {newState}");

        switch (newState)
        {
            case PlayerState.Jumping:
                Velocity = new Vector3(Velocity.X, JumpVelocity, Velocity.Z);
                break;
        }
    }
}
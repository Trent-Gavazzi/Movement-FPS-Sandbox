using Godot;

public partial class Player : CharacterBody3D
{
    [Export] public float MoveSpeed { get; set; } = 5.0f;
    [Export] public float TurnSpeed { get; set; } = 10.0f;   // How quickly the model rotates to face movement
    [Export] public Node3D CameraPivot { get; set; }         // Drag CameraPivot here
    [Export] public Node3D Model { get; set; }               // Drag the visible mesh here

    private float _gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public override void _PhysicsProcess(double delta)
    {
        float dt = (float)delta;
        Vector3 velocity = Velocity;

        // Simple gravity so the player stays on the ground
        if (!IsOnFloor())
            velocity.Y -= _gravity * dt;

        // WASD input (set these actions up in Project Settings > Input Map)
        Vector2 input = Input.GetVector("move_left", "move_right", "move_forward", "move_back");

        // Flatten camera directions so looking up/down doesn't affect movement
        Basis camBasis = CameraPivot.GlobalTransform.Basis;

        Vector3 forward = -camBasis.Z;
        forward.Y = 0f;
        forward = forward.Normalized();

        Vector3 right = camBasis.X;
        right.Y = 0f;
        right = right.Normalized();

        Vector3 direction = right * input.X - forward * input.Y;

        if (direction.LengthSquared() > 0.0001f)
        {
            velocity.X = direction.X * MoveSpeed;
            velocity.Z = direction.Z * MoveSpeed;

            // Turn the model (not the body) to face the movement direction
            if (Model != null)
            {
                float targetAngle = Mathf.Atan2(-direction.X, -direction.Z);
                Vector3 modelRotation = Model.Rotation;
                modelRotation.Y = Mathf.LerpAngle(modelRotation.Y, targetAngle, TurnSpeed * dt);
                Model.Rotation = modelRotation;
            }
        }
        else
        {
            velocity.X = 0f;
            velocity.Z = 0f;
        }

        Velocity = velocity;
        MoveAndSlide();
    }
}
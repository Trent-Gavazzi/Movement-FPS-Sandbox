using Godot;

public partial class CameraController : Node3D
{
    [Export] public float MouseSensitivity { get; set; } = 0.003f;
    [Export] public float Distance { get; set; } = 5.0f;
    [Export] public float MinPitch { get; set; } = -60f;   // Degrees, looking down
    [Export] public float MaxPitch { get; set; } = 30f;    // Degrees, looking up

    private SpringArm3D _springArm;

    public override void _Ready()
    {
        _springArm = GetNode<SpringArm3D>("SpringArm3D");
        _springArm.SpringLength = Distance;

        // Stop the camera from colliding with the player's own capsule
        if (GetParent() is CollisionObject3D player)
            _springArm.AddExcludedObject(player.GetRid());

        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion motion && Input.MouseMode == Input.MouseModeEnum.Captured)
        {
            // Left/right turns the pivot
            Vector3 pivotRotation = Rotation;
            pivotRotation.Y -= motion.Relative.X * MouseSensitivity;
            Rotation = pivotRotation;

            // Up/down tilts the spring arm
            Vector3 armRotation = _springArm.Rotation;
            armRotation.X -= motion.Relative.Y * MouseSensitivity;
            armRotation.X = Mathf.Clamp(armRotation.X, Mathf.DegToRad(MinPitch), Mathf.DegToRad(MaxPitch));
            _springArm.Rotation = armRotation;
        }
        // Escape frees the cursor, clicking the game window captures it again
        else if (@event.IsActionPressed("ui_cancel"))
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }
        else if (@event is InputEventMouseButton button && button.Pressed)
        {
            Input.MouseMode = Input.MouseModeEnum.Captured;
        }
    }
}
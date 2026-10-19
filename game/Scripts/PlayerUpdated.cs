using Godot;

public class PlayerUpdated : CharacterBody3D
{
    private enum PlayerStates{STATE_STANDING, STATE_JUMPING, STATE_CROUCHING};
    public PlayerStates CurrentMoveState;

    [Export] public float MoveSpeed{get; set;} = 5.0f;
    [Export] public float TurnSpeed{get; set;} = 10.0f;
    [Export] public Node3D Model{get; set;};
    [Export] public Node3D CameraPivot{get; set;}

    private float _gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public override void _PhysicsProcess(double delta)
    {
        
    }

    public PlayerStates HandleNewState(PlayerStates CurState)
    {
        switch (CurrentMoveState)
        {
            case STATE_STANDING:
                
                GD.Print("We are standing!");
            case STATE_JUMPING:
                GD.Print("We are jumping!");
            case STATE_CROUCHING:
                GD.Print("We are crouching!");
            default:
                break;
        }
    }
}
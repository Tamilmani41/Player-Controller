using System.Runtime.ConstrainedExecution;
using UnityEngine.EventSystems;

/// <summary>
/// Player Movemenr module Compeltely C#, doesn't know about unity.
/// Handles player's Horizontal movement only. Needs Player controller to construct it when it starts.
/// </summary>
public class MovementModule
{
    private readonly PlayerSettings settings;    // Reference to player settings for movement parameters.
    public float FinalMove { get; private set; }

    public MovementModule(PlayerSettings settings)  // Constructor that takes PlayerSettings as a parameter to initialize the movement module.
    {
        this.settings = settings;
    }

    public void Tick(float moveDir)
    {
        Move(moveDir);
    }
    private void Move(float moveDir)
    {
        // Implement movement logic here using moveDir and settings.moveSpeed.
        FinalMove = moveDir * settings.moveSpeed;
    }
}

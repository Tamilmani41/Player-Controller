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
    private readonly float moveDir; // Direction of movement input, typically -1 for left, 1 for right, and 0 for no movement.

    public MovementModule(PlayerSettings settings, float moveDir)  // Constructor that takes PlayerSettings as a parameter to initialize the movement module.
    {
        this.settings = settings;
        this.moveDir = moveDir;
    }

    public void Tick()
    {
        Move();
    }
    private void Move()
    {
        // Implement movement logic here using moveDir and settings.moveSpeed.
        FinalMove = moveDir * settings.moveSpeed;
    }
}

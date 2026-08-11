using UnityEngine;
using UnityEngine.InputSystem;
/// <Summary>
/// Player : Unity's new input system reads the input from unity and gives required input to the playerController. *Unity dependent*
/// </Summary>

public class PlayerInput : MonoBehaviour
{
    private PlayerInputActions playerAction;
    public Vector2 Move { get; private set; }
    public bool JumpQueued { get; private set; } = false;
    public void Tick()
    {
        Move = playerAction.Gameplay.Move.ReadValue<Vector2>();

        if (playerAction.Gameplay.Jump.WasPressedThisFrame())
        {
            JumpQueued = true;
        }
    }
}

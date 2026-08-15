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
    public bool DebugLogPrint { get; private set; } = false;

    private void Awake()
    {
        playerAction = new PlayerInputActions();    // Initialize the PlayerInputActions instance.
        playerAction.Gameplay.Enable(); // Enable the Gameplay action map when the script is initialized.
        playerAction.Debug.Enable(); // Enable the Debug action map when the script is initialized.
        Debug.Log("PlayerInput initialized and Gameplay action map enabled.");
    }
    public void Tick()
    {
        Move = playerAction.Gameplay.Move.ReadValue<Vector2>();

        if (playerAction.Gameplay.Jump.WasPressedThisFrame())
        {
            JumpQueued = true;
        }
        //if (playerAction.Debug.DebugLogPrint.WasPressedThisFrame())
        //{
        //    Debug.Log("Debug Log Print action triggered.");
        //    DebugLogPrint = true;
        //}
        //else
        //{
        //    DebugLogPrint = false;
        //}
    }
}

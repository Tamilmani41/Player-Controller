using UnityEngine;

/// <summary>
///  Player controller tick, used to orchestrate every module
///  and produce final ourput Unity's rigid body.
/// </summary>

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput input;
    [SerializeField] private PlayerSettings settings;   // Reference to player settings for parameters.
    private Grounddetector grounddetector;
    private MovementModule movementModule;
    private JumpModule jumpModule;

    private float moveDir;
    public float FinalSpeed { get; private set; }   // Final movement speed calculated by the movement module.

    private void Awake()
    {
        if (input == null)
            input = gameObject.GetComponent<PlayerInput>();
        // Initialize all modules here.
        grounddetector = new Grounddetector();
        movementModule = new MovementModule(settings);
        jumpModule = new JumpModule();
    }
    
    // For now Player controller uses the Update method to tick the modules, but in the future, it can be changed to a more sophisticated system if needed.
    private void Update()
    {
        Tick();
    }
    public void Tick()
    {
        // Call Module Ticks's || tells the modules to operate at the exact order.
        input.Tick();   // Update input module to read the latest player inputs.
        moveDir = input.Move.x; // Get the horizontal movement direction from player input.

       //grounddetector.Tick();
        movementModule.Tick(moveDir);   // Update movement module with the current move direction.

        FinalSpeed = movementModule.FinalMove;   // Get the final movement speed calculated by the movement module.

        jumpModule.Tick();
    }

}

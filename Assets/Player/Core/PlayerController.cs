using UnityEngine;

/// <summary>
///  Player controller tick, used to orchestrate every module
///  and produce final ourput Unity's rigid body.
/// </summary>

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput input;
    [SerializeField] private PlayerSettings settings;   // Reference to player settings for parameters.
    [SerializeField] private RigidBodyAdapter rigidBodyAdapter;
    private Grounddetector grounddetector;
    private PhysicsQueries physicsQueries;
    private MovementModule movementModule;
    private JumpModule jumpModule;

    private float moveDir;
    public float FinalSpeed { get; private set; }   // Final movement speed calculated by the movement module.

    private bool isGrounded = false;
    private bool jumpQueue = false;
    private bool debugLogPrint = false;
    private void Awake()
    {
        if (input == null)
            input = gameObject.GetComponent<PlayerInput>();
        // Initialize all modules here.
        physicsQueries = new PhysicsQueries(rigidBodyAdapter);  // Initialize physics queries with the rigid body adapter.
        grounddetector = new Grounddetector(settings, physicsQueries);  // Initialize ground detector with player settings and physics queries.
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
        InputDetection();   // Check player input.
        
        GroundDetection();   // Check if the player is grounded.

        MovementCalculation(); // Calculate movement based on input and grounded state.

        jumpModule.Tick();
    } 
    private void InputDetection()
    {
        input.Tick();   // Update input module to read the latest player inputs.
        moveDir = input.Move.x; // Get the horizontal movement direction from player input.
        jumpQueue = input.JumpQueued; // Get the jump input state from player input.
        debugLogPrint = input.DebugLogPrint;
    }
    private void GroundDetection()
    {
        grounddetector.Tick();
        isGrounded = grounddetector.IsGrounded;
    }
    private void MovementCalculation()
    {
        movementModule.Tick(moveDir);   // Update movement module with the current move direction.
        FinalSpeed = movementModule.FinalMove;   // Get the final movement speed calculated by the movement module.
    }
    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(transform.position, Vector3.down * settings.groundCheckDistance, Color.red);
    }
}

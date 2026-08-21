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
    private GravityModule gravityModule;

    private float moveDir;
    private float deltatime;
    private bool jumpProcessed = false;
    private bool jumpApproved = false;
    public bool Jumprequested { get; private set; }
    public float FinalSpeed { get; private set; }   // Final movement speed calculated by the movement module.
    public float Jumpforce { get; private set; }
    public bool JumpQueue { get; private set; } = false;
    public bool IsGrounded { get; private set; } = false;
    public float FinalGravity { get; private set; } = 0f; // This property can be used to track the final gravity value applied to the player, which can be useful for debugging or other gameplay mechanics.
    private bool debugLogPrint = false;

    // Debuging purpose exposing jump module variables to the inspector, can be removed later.
    [Header("Jump Debug")]
    [SerializeField] private float BufferTimer; // Expose the buffer timer from the jump module for debugging purposes.
    [SerializeField] private float CoyoteTimer; // Expose the coyote timer from the jump module for debugging purposes.
    [SerializeField] private bool NormalInputConsumed; // Expose the normal input consumed flag from the jump module for debugging purposes.
    [SerializeField] private bool CoyoteInputConsumed; // Expose the coyote input consumed flag from the jump module for debugging purposes.
    [SerializeField] private bool BufferInputConsumed; // Expose the buffer input consumed flag from the jump module for debugging purposes.
    [SerializeField] private bool BufferApproved; // Expose the buffer approved flag from the jump module for debugging purposes.

    public float VelocityY { get; private set; } = 0f; // This property can be used to track the vertical velocity of the player, which can be useful for animations or other gameplay mechanics.
    private void Awake()
    {
        if (input == null)
            input = gameObject.GetComponent<PlayerInput>();
        // Initialize all modules here.
        physicsQueries = new PhysicsQueries(rigidBodyAdapter);  // Initialize physics queries with the rigid body adapter.
        grounddetector = new Grounddetector(settings, physicsQueries);  // Initialize ground detector with player settings and physics queries.
        movementModule = new MovementModule(settings);
        jumpModule = new JumpModule(settings);
        gravityModule = new GravityModule();
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

        JumpCalculation();

        GravityCalculation();   
    } 
    private void InputDetection()
    {
        input.Tick();   // Update input module to read the latest player inputs.
        moveDir = input.Move.x; // Get the horizontal movement direction from player input.
        debugLogPrint = input.DebugLogPrint;
    }
    private void GroundDetection()
    {
        grounddetector.Tick();
        IsGrounded = grounddetector.IsGrounded;
    }
    private void MovementCalculation()
    {
        movementModule.Tick(moveDir);   // Update movement module with the current move direction.
        FinalSpeed = movementModule.FinalMove;   // Get the final movement speed calculated by the movement module.
    }
    private void JumpCalculation()
    {
        if (input.IsJumpQueued())
        {
            JumpQueue = true;
        }
        deltatime = Time.deltaTime;

        // Call the jump module's Tick method to process jump logic based on the current state and input.
        // The out parameters are used to determine if a jump has been processed and if it has been approved for execution.
        jumpModule.Tick(JumpQueue, IsGrounded, deltatime, out jumpProcessed, out jumpApproved);

        if (jumpProcessed)
        {
            ResetJumpQueue(); // Reset the jump queue after processing the jump to prevent continuous jumping.
        }
        if (jumpApproved)
        {
            Jumprequested = true; // Set jumpRequested to true to indicate that a jump has been processed ana awaiting to be applied in the next physics update.
        }
        Jumpforce = jumpModule.F_Jumpforce;
        // debug log for jump module variables, can be removed later.
        BufferTimer = jumpModule.bufferTimer; // Update the buffer timer for debugging purposes.
        CoyoteTimer = jumpModule.coyoteTimer;
        NormalInputConsumed = jumpModule.p_NormalInputConsumed; // Update the normal input consumed flag for debugging purposes.
        CoyoteInputConsumed = jumpModule.p_CoyoteInputConsumed;
        BufferInputConsumed = jumpModule.p_BufferInputConsumed;
        BufferApproved = jumpModule.bufferApprove;
    }
    // Reset the jump queue after processing the jump to prevent continuous jumping.
    public bool ResetJumpQueue()
    {
        return JumpQueue = false;
    }
    public bool ResetJumpRequest()
    {
        return Jumprequested = false;
    }
    public bool ResetJumpApproved()
    {
        return jumpApproved = false;
    }
    private void GravityCalculation()
    {
        VelocityY = rigidBodyAdapter.GetVelocity(); // Update the vertical velocity property based on the Rigidbody's current velocity.
        gravityModule.Tick(IsGrounded,VelocityY);   // Update gravity module based on whether the player is grounded.
        FinalGravity = gravityModule.GravityValue; // Update the final gravity property with the calculated gravity value.
    }
    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(transform.position, Vector3.down * settings.groundCheckDistance, Color.red);
    }
}

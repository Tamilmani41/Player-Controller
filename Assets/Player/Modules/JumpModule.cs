/// <Summary>
/// Player's Jump Module Handels Player Jump calculations, includes : Cayotie, Buffer timer logics etc...
/// </Summary>
public class JumpModule
{
    private readonly float jumpForce;
    // exposing variables to the player controller for debugging purposes, can be modified later to private.
    public float F_Jumpforce { get; private set; }
    public float bufferTimer = 0f; // Timer to track the jump buffer duration.
    public float coyoteTimer = 0f; // Timer to track the coyote time duration.
    private readonly float bufferTime; // Default buffer time, can be set from PlayerSettings if needed.
    private readonly float coyoteTime; // Time window to allow jump input after leaving the ground (Coyote Time).

    // p_ - processed, these bools are used to track the validity of input jumpqueue, and later used to reset the jump queue in the PlayerController after processing the jump.
    public bool p_NormalInputConsumed;
    public bool p_CoyoteInputConsumed;
    public bool p_BufferInputConsumed;

    // Flag to indicate if the jump buffer has been approved for jumping.
    public bool bufferApprove; 
    // Constructor to initialize the JumpModule with player settings.
    public JumpModule(PlayerSettings settings)
    {
        jumpForce = settings.jumpforce;
        bufferTime = settings.jumpBufferTime;
        coyoteTime = settings.coyoteTime;
    }
    public bool Tick(bool jumpQueued, bool isGrounded, float deltatime, out bool jumpProcessed, out bool jumpApproved)
    {
        // jump approved is uesd to determine if the jump has been approved by any of the jump conditions,
        // which is later used to set jump appreoved in player controller to true, which is later used to apply jump force in the rigidbody adapter.
        jumpApproved = false;

        // elseif ladder used to check for jump conditions in order of priority: Normal Jump, Coyote Jump, and Jump Buffer.
    
        if (NormalJump(jumpQueued, isGrounded, out p_NormalInputConsumed))  // If a normal jump is valid, reset both the buffer and coyote timers.
        {
            bufferTimer = 0f; // Reset the buffer timer when a normal jump is valid.
            coyoteTimer = 0f; // Reset the coyote timer when a normal jump is valid.
            jumpApproved = true; // Set jumpApproved to true when a normal jump is valid.
        }
        else if (CoyoteTick(jumpQueued, isGrounded, deltatime, coyoteTime, out p_CoyoteInputConsumed)) // If a coyote jump is valid, reset the buffer timer.
        {
            bufferTimer = 0f; // Reset the buffer timer when a coyote jump is valid.
            jumpApproved = true; // Set jumpApproved to true when a coyote jump is valid.
        }
        else if (JumpBufferTick(jumpQueued, isGrounded, deltatime, bufferTime, out p_BufferInputConsumed , out bufferApprove))    // If a jump buffer is valid, reset the coyote timer.
        {
            coyoteTimer = 0f; // Reset the coyote timer when a jump buffer is valid.
            jumpApproved = bufferApprove; // Set jumpApproved to true if the jump buffer is valid and approved.
        }

        // jump processed is used to determine if any jump condition has been met and processed.
        // Which later used to reset the jump queue in the PlayerController after processing the jump.
        jumpProcessed = p_NormalInputConsumed || p_CoyoteInputConsumed || p_BufferInputConsumed;   
        return jumpProcessed;
    }
    private bool NormalJump(bool jumpQueued, bool isGrounded, out bool p_NormalJumpValid)
    {
        p_NormalJumpValid = false;   // Initialize the normal jump flag to false.
        if (jumpQueued && isGrounded) // If jump is queued and player is grounded, apply jump force.
        {
            F_Jumpforce = jumpForce;
            p_NormalJumpValid = true;    // Set the normal jump flag to true.
        }
        return p_NormalJumpValid;    // Return the normal jump flag.
    }
    private bool JumpBufferTick(bool jumpQueued, bool isGrounded, float deltatime, float bufferTime, out bool p_JumpBuffer, out bool bufferApprove)
    {
        p_JumpBuffer = false;
        bufferApprove = false;
        if (jumpQueued && !isGrounded && bufferTimer <= 0)  // If jump is queued but player is not grounded and buffer timer is not active, start the buffer timer.
        {
            bufferTimer = bufferTime; // Reset the buffer timer when jump is queued but player is not grounded.
            // Set the jump buffer flag to true to indicate that a jump has been queued while the player is in the air.
            // which also resets the jump queue in the PlayerController after processing the jump.
            p_JumpBuffer = true;   
        }
        else if (bufferTimer > 0)   // If the buffer timer is active, decrease it over time.
        {
            bufferTimer -= deltatime; // Decrease the buffer timer over time.
        }
        if (bufferTimer > 0 && isGrounded)
        {
            F_Jumpforce = jumpForce;
            bufferApprove = true;   // Set the buffer approve flag to true to indicate that a jump has been approved from the jump buffer.
            bufferTimer = 0f; // Reset the buffer timer when a jump has been approved from the jump buffer.
        }
        return bufferApprove;    // Return the buffer approve flag to indicate if a jump has been approved from the jump buffer.
    }
    private bool CoyoteTick(bool jumpQueued, bool isGrounded, float deltatime, float coyoteTime, out bool p_CoyoteJump)
    {
        p_CoyoteJump = false;
        if (isGrounded)
        {
            coyoteTimer = coyoteTime; // Reset the coyote timer when player is grounded.
        }
        else if (coyoteTimer > 0)   // If the coyote timer is active, decrease it over time.
        {
            coyoteTimer -= deltatime; // Decrease the coyote timer over time.
        }
        if (coyoteTimer > 0 && !isGrounded && jumpQueued) // If the coyote timer is active and player is not grounded, allow jump.
        {
            F_Jumpforce = jumpForce;
            p_CoyoteJump = true;    // Set the coyote jump flag to true.
        }
        return p_CoyoteJump;    // Return the coyote jump flag.
    }
}

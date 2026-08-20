using System.Text;

/// <Summary>
/// Player's Jump Module Handels Player Jump calculations, includes : Cayotie, Buffer timer logics etc...
/// </Summary>
public class JumpModule
{
    private float jumpForce;
    public float F_Jumpforce { get; private set; }
    private float bufferTimer = 0f; // Timer to track the jump buffer duration.
    private float coyoteTimer = 0f; // Timer to track the coyote time duration.
    private float bufferTime; // Default buffer time, can be set from PlayerSettings if needed.
    private float coyoteTime; // Time window to allow jump input after leaving the ground (Coyote Time).

    private bool p_NormalJumpValid;
    private bool p_CoyoteJump;
    private bool p_JumpBuffer;

    // Constructor to initialize the JumpModule with player settings.
    public JumpModule(PlayerSettings settings)
    {
        jumpForce = settings.jumpforce;
        bufferTime = settings.jumpBufferTime;
        coyoteTime = settings.coyoteTime;
    }
    public bool Tick(bool jumpQueued, bool isGrounded, float deltatime, out bool jumpProcessed)
    {
        p_NormalJumpValid = NormalJump(jumpQueued, isGrounded, out p_NormalJumpValid);
        p_JumpBuffer = JumpBufferTick(jumpQueued, isGrounded, deltatime, bufferTime, out p_JumpBuffer); // Use the buffer time from setting
        p_CoyoteJump = CoyoteTick(jumpQueued, isGrounded, deltatime, coyoteTime, out p_CoyoteJump   ); // Use the coyote time from setting

        if (p_NormalJumpValid)  // If a normal jump is valid, reset the buffer and coyote timers.
        {
            bufferTimer = 0f; // Reset the buffer timer when a normal jump is valid.
            coyoteTimer = 0f; // Reset the coyote timer when a normal jump is valid.
        }
        else if (p_JumpBuffer)  // If a jump buffer is valid, reset the coyote timer.
        {
            coyoteTimer = 0f; // Reset the coyote timer when a jump buffer is valid.
        }
        else if (p_CoyoteJump)  // If a coyote jump is valid, reset the buffer timer.
        {
            bufferTimer = 0f; // Reset the buffer timer when a coyote jump is valid.
        }
        jumpProcessed = p_NormalJumpValid || p_CoyoteJump || p_JumpBuffer;   // If any of the jump conditions are met, set jumpProcessed to true.
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
    private bool JumpBufferTick(bool jumpQueued, bool isGrounded, float deltatime, float bufferTime, out bool p_JumpBuffer)
    {
        p_JumpBuffer = false;
        if (jumpQueued && !isGrounded)  // If jump is queued but player is not grounded, start the buffer timer.
        {
            bufferTimer = bufferTime; // Reset the buffer timer when jump is queued but player is not grounded.
        }
        else if (bufferTimer > 0)   // If the buffer timer is active, decrease it over time.
        {
            bufferTimer -= deltatime; // Decrease the buffer timer over time.
        }
        if (bufferTimer > 0 && isGrounded)
        {
            F_Jumpforce = jumpForce;
            p_JumpBuffer = true;    // If the buffer timer is active and player is grounded, allow jump.
        }
        return p_JumpBuffer;    // Return the jump buffer flag.
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

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

    // Constructor to initialize the JumpModule with player settings.
    public JumpModule(PlayerSettings settings)
    {
        jumpForce = settings.jumpforce;
        bufferTime = settings.jumpBufferTime;
        coyoteTime = settings.coyoteTime;
    }
    public void Tick(bool jumpQueued, bool isGrounded, float deltatime)
    {
        if (jumpQueued && isGrounded)
        {
            F_Jumpforce = jumpForce;
        }

        JumpBufferTick(jumpQueued, isGrounded, deltatime, bufferTime); // Use the buffer time from setting
        CoyoteTick(jumpQueued, isGrounded, deltatime, coyoteTime); // Use the coyote time from setting
    }
    public void JumpBufferTick(bool jumpQueued, bool isGrounded, float deltatime, float bufferTime)
    {
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
        }
    }
    public void CoyoteTick(bool jumpQueued, bool isGrounded, float deltatime, float coyoteTime)
    {
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
        }
    }
}

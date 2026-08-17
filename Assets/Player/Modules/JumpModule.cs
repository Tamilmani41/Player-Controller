/// <Summary>
/// Player's Jump Module Handels Player Jump calculations, includes : Cayotie, Buffer timer logics etc...
/// </Summary>
public class JumpModule
{
    private float jumpForce;
    public float F_Jumpforce { get; private set; }
    private float bufferTimer = 0f; // Timer to track the jump buffer duration.
    private float bufferTime; // Default buffer time, can be set from PlayerSettings if needed.

    // Constructor to initialize the JumpModule with player settings.
    public JumpModule(PlayerSettings settings)
    {
        jumpForce = settings.jumpforce;
        bufferTime = settings.jumpBufferTime; 
    }
    public void Tick(bool jumpQueued, bool isGrounded)
    {
        if (jumpQueued && isGrounded)
        {
            F_Jumpforce = jumpForce;
        }

        JumpBufferTick(jumpQueued, isGrounded, bufferTime); // Use the buffer time from setting
    }
    public void JumpBufferTick(bool jumpQueued, bool isGrounded, float bufferTime)
    {
        if (jumpQueued && !isGrounded)  // If jump is queued but player is not grounded, start the buffer timer.
        {
            bufferTimer = bufferTime; // Reset the buffer timer when jump is queued but player is not grounded.
        }
        else if (bufferTimer > 0)   // If the buffer timer is active, decrease it over time.
        {
            bufferTimer -= UnityEngine.Time.deltaTime; // Decrease the buffer timer over time.
        }
        if (bufferTimer > 0 && isGrounded)
        {
            F_Jumpforce = jumpForce;
        }
    }
}

/// <Summary>
/// Player's Jump Module Handels Player Jump calculations, includes : Cayotie, Buffer timer logics etc...
/// </Summary>
public class JumpModule
{
    private float jumpForce;
    public float F_Jumpforce { get; private set; }
    public JumpModule(PlayerSettings settings)
    {
        jumpForce = settings.jumpforce;
    }
    public void Tick(bool jumpQueued)
    {
        if (jumpQueued)
        {
            F_Jumpforce = jumpForce;
        }
    }
}

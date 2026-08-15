/// <summary>
///  Ground detector module, used to detect if the player is grounded or not.
/// </summary>
public class Grounddetector
{
    public bool IsGrounded { get; private set; } = false;
    private float groundCheckDistance;
    private PhysicsQueries queries;
    public Grounddetector(PlayerSettings settings, PhysicsQueries queries)
    {
        this.groundCheckDistance = settings.groundCheckDistance;
        this.queries = queries;
    }
    public void Tick()
    {
        Raycast();
    }
    private bool Raycast()
    {
        // Perform a raycast to check if the player is grounded, ask the PhysicsQueries class to handle the raycasting logic.
        return IsGrounded = queries.RayCast(groundCheckDistance)? true : false;
    }
}

using Unity.Android.Gradle.Manifest;

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
        // Request a raycast to the rigidbody adapter, if a modules asks.
        if (queries != null)
        {
            return IsGrounded = queries.RayCast(groundCheckDistance);
        }
        else
        {
            return IsGrounded = false;
        }
    }
}

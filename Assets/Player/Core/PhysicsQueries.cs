/// <summary>
/// This class is responsible for handling physics queries in the game, 
/// such as raycasting, collision detection, and other physics-related calculations. 
/// It provides methods to perform these queries efficiently and accurately, allowing for better interaction with the game world and its objects.
/// </summary>

// For initial implementaion this one only used for raycast, request a raycast to the rigidbody adapter, if a modules asks.
public class PhysicsQueries
{
    private RigidBodyAdapter rigidBodyAdapter;

    // Constructor that takes a RigidBodyAdapter as a parameter, allowing the PhysicsQueries class to interact with the player's Rigidbody component.
    public PhysicsQueries(RigidBodyAdapter rigidBodyAdapter)    
    {
        this.rigidBodyAdapter = rigidBodyAdapter;
    }

    public bool RayCast(float distance)
    {
        return rigidBodyAdapter.RayCast(distance);
    }
}

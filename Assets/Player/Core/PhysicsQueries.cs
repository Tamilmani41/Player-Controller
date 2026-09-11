
// For initial implementaion this one only used for raycast, request a raycast to the rigidbody adapter, if a modules asks.
using UnityEngine;

/// <summary>
/// This class is responsible for handling physics queries in the game, 
/// such as raycasting, collision detection, and other physics-related calculations. 
/// It provides methods to perform these queries efficiently and accurately, allowing for better interaction with the game world and its objects.
/// </summary>
public class PhysicsQueries
{
    private RigidBodyAdapter rigidBodyAdapter;
    private Collider2D collider;

    // Constructor that takes a RigidBodyAdapter as a parameter, allowing the PhysicsQueries class to interact with the player's Rigidbody component.
    public PhysicsQueries(RigidBodyAdapter rigidBodyAdapter)    
    {
        this.rigidBodyAdapter = rigidBodyAdapter;
    }

    public Collider2D RayCast(float distance)
    {
        // Perform a raycast using the RigidBodyAdapter to check for collisions within the specified distance.
        return collider = rigidBodyAdapter.RayCast(distance);
    }
    public MovingPlatform GetPlatform()
    {
        if (collider == null)
            return null;

        return collider.GetComponent<MovingPlatform>();
    }
    
}

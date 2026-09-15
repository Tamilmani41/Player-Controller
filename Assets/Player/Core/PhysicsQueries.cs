
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
    public Vector2Data PlatformVelocity()   // method for getting platforms velocity
    {
        MovingPlatform platform = GetPlatform();

        if (platform == null)
            return new Vector2Data(0, 0);   // return (0, 0) velociy if no moving platform is there. Which simply means no velocity.
        Vector2 velocity = platform.GetCurrentVelocity();   // Get the current unity Vector2 notation, ie... Velocity.

        return new Vector2Data(velocity.x, velocity.y);     // give it to our Vector 2 notation.
    }
}

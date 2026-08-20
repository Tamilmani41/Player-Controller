using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Rigidbody2D))]

public class RigidBodyAdapter : MonoBehaviour   // This class serves as an adapter for Unity's Rigidbody component, allowing for easier manipulation and control of the player's physics-based movement.
{
    [SerializeField] private PlayerController player;
    [SerializeField] private LayerMask groundMask;

    [SerializeField] private Rigidbody2D rb;   // Reference to the Rigidbody component attached to the player GameObject.
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();   // Get the Rigidbody component from the GameObject this script is attached to.
        player = GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        // Apply the final movement speed calculated by the PlayerController to the Rigidbody's velocity.

        rb.linearVelocity = new Vector2(player.FinalSpeed, rb.linearVelocity.y);

        if (player.JumpQueue)  // Check if a jump input has been queued in the PlayerController.
        {
            Jump();   // If a jump input has been queued, call the Jump method to apply the jump force.
        }
        rb.gravityScale = player.FinalGravity;   // Apply the final gravity value calculated by the PlayerController to the Rigidbody's gravity scale.
    }
    private void Jump()
    {
        rb.AddForce(new Vector2(0f, player.Jumpforce), ForceMode2D.Impulse);   // Apply an impulse force to the Rigidbody for jumping, based on the jump force calculated by the PlayerController.
    }
    // Method to perform a raycast downwards to check if the player is grounded.
    public bool RayCast(float distance)
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.down, distance, groundMask);   // Perform a raycast downwards from the Rigidbody's position to check for ground collision within the specified distance.
        return hit.collider != null; // Return true if the raycast hit something (indicating the player is grounded), otherwise return false.
    }

    // Helper method to get the current velocity of the Rigidbody.
    public float GetVelocity()
    {
        float velocity = rb.linearVelocityY;   // Output the current velocity of the Rigidbody.
        return velocity;
    }
}

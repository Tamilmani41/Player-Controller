using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Rigidbody2D))]

public class RigidBodyAdapter : MonoBehaviour   // This class serves as an adapter for Unity's Rigidbody component, allowing for easier manipulation and control of the player's physics-based movement.
{
    [SerializeField] private PlayerController player;

    [SerializeField] private Rigidbody2D rb;   // Reference to the Rigidbody component attached to the player GameObject.
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();   // Get the Rigidbody component from the GameObject this script is attached to.
        player = GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        // Apply the final movement speed calculated by the PlayerController to the Rigidbody's velocity.
        rb.linearVelocity = new Vector2(player.FinalSpeed, rb.linearVelocity.y);   // Set the Rigidbody's horizontal velocity based on the player's final speed, while preserving the vertical velocity.
    }
}

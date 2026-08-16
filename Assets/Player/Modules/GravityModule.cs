using NUnit.Framework;

/// <summary>
/// Gravity module is responsible for applying gravity to the player character. By reading the player current fall state, it will give the appropriate gravity value.
/// Then Rigidbody adapter will apply the gravity value to the player rigidbody.
/// </summary>
public class GravityModule
{
    public float GravityValue { get; private set; } = 0f; // The current gravity value to be applied to the player character.
    public GravityModule()
    {

    }
    public void Tick(bool isGrounded, float velocityY)
    {
        // if the player is falling it multiplies the gravity value by 2, otherwise it will be 1. This is to make the player fall faster than it rises.
        if (!isGrounded && velocityY <= 0f) // If the player is not grounded and is falling (velocityY <= 0), apply falling gravity logic. 
        {
            GravityValue = 2f;
        }
        else
        {
            GravityValue = 1f; // Reset the gravity value when the player is grounded or not falling.
        }
    }
}

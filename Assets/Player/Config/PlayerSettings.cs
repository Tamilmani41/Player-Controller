/// <summary>
/// PlayerSettings is a configuration class that holds various settings related to the player character in the game. 
/// It can include parameters such as movement speed, jump height, health, and other gameplay-related attributes. 
/// This class can be used to easily adjust player behavior without modifying the core logic of the player controller or modules.
/// </summary>
[System.Serializable]   // This attribute allows the class to be serialized, making it possible to save and load settings, or to expose them in the Unity Inspector.
public class PlayerSettings
{
    public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float groundCheckDistance = 0.5f;
    public float jumpforce = 5f;
    public float jumpBufferTime = 0.2f; // Time window to allow jump input after leaving the ground.
    public float coyoteTime = 0.2f; // Time window to allow jump input after leaving the ground (Coyote Time).
    public enum Check
    {
        Down, Up, Left, Right
    }
}

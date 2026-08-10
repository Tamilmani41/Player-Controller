using UnityEngine;

/// <summary>
///  Player controller tick, used to orchestrate every module
///  and produce final ourput Unity's rigid body.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput input;
    private MovementModule movementModule;
    private JumpModule jumpModule;
    private void Awake()
    {
        if (input == null)
            input = gameObject.GetComponent<PlayerInput>();
        // Initialize all modules here.
        movementModule = new MovementModule();
        jumpModule = new JumpModule();
    }
    public void Tick()
    {
        // Call Module TIcks's || tells the modules to operate at the exact order.
        input.Tick();
        movementModule.Tick();
        jumpModule.Tick();
    }

}

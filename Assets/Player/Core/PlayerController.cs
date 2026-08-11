using UnityEngine;

/// <summary>
///  Player controller tick, used to orchestrate every module
///  and produce final ourput Unity's rigid body.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput input;
    PlayerSettings settings;
    private Grounddetector grounddetector;
    private MovementModule movementModule;
    private JumpModule jumpModule;

    private float moveDir;
    private float moveSpeed;

    private void Awake()
    {
        if (input == null)
            input = gameObject.GetComponent<PlayerInput>();
        // Initialize all modules here.
        settings = new PlayerSettings();
        grounddetector = new Grounddetector();
        movementModule = new MovementModule(settings, moveDir);
        jumpModule = new JumpModule();
    }
    public void Tick()
    {
        // Call Module TIcks's || tells the modules to operate at the exact order.
        input.Tick();
        moveDir = input.Move.x;

        //grounddetector.Tick();
        movementModule.Tick();
        moveSpeed = movementModule.FinalMove;

        jumpModule.Tick();
    }

}

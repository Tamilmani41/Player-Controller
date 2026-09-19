using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour
{
    public Transform targetPosition;
    public float movementDuration = 2f;
    private Vector2 startPosition;
    private Rigidbody2D rb;
    private bool isMovingToTarget = true;
    private float elapsedTime = 0f;
    private Vector2 previousPosition;
    private Vector2 velocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = rb.position;
        previousPosition = rb.position;
    }

    void FixedUpdate()
    {
        if (targetPosition == null) return;

        elapsedTime += Time.fixedDeltaTime;
        float t = Mathf.Clamp01(elapsedTime / movementDuration);
        float easedT = Mathf.SmoothStep(0f, 1f, t);

        Vector2 nextPosition;   
        if (isMovingToTarget)   
        {
            nextPosition = Vector2.Lerp(startPosition, targetPosition.position, easedT);
        }
        else
        {
            nextPosition = Vector2.Lerp(targetPosition.position, startPosition, easedT);
        }
        velocity = (nextPosition - previousPosition) / Time.fixedDeltaTime;

        rb.MovePosition(nextPosition);  // move the platform to next position.

        if (t >= 1)
        {
            isMovingToTarget = !isMovingToTarget;
            elapsedTime = 0f;
            previousPosition = rb.position; // Reset previous position when changing direction
        }
        else
        {
            previousPosition = rb.position;
        }
    }

    public Vector2 GetCurrentVelocity()
    {
        return velocity;
    }
}

using UnityEngine;
public class SheepMovement : MonoBehaviour
{
    public float circleSpeed = 1f;   // Speed of circular motion
    public float circleRadius = 0.2f; // Radius of circular motion
    public float jumpForce = 5f;     // Force of occasional jumps
    public float jumpInterval = 3f;  // Time between jumps (randomized)
    
    [HideInInspector] // Hide in inspector but keep public access
    public bool isBeingDragged = false; // Flag to stop movement when dragged
    
    private Rigidbody rb;
    private float timeOffset;
    private float nextJumpTime;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        timeOffset = Random.Range(0f, 100f); // Prevents all sheep moving the same way
        nextJumpTime = Time.time + Random.Range(jumpInterval, jumpInterval * 2); // Randomize first jump
    }
    
    void FixedUpdate()
    {
        if (!isBeingDragged) // Only move if not being dragged
        {
            // Generate smooth movement using Perlin Noise
            float xOffset = Mathf.PerlinNoise(Time.time * circleSpeed + timeOffset, 0) * 2 - 1;
            float yOffset = Mathf.PerlinNoise(0, Time.time * circleSpeed + timeOffset) * 2 - 1;
            // Apply circular motion
            Vector3 movement = new Vector3(xOffset, yOffset, 0) * circleRadius;
            rb.MovePosition(transform.position + movement * Time.fixedDeltaTime);
            // Occasional Jump
            if (Time.time >= nextJumpTime)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                nextJumpTime = Time.time + Random.Range(jumpInterval, jumpInterval * 2); // Schedule next jump
            }
        }
    }
    
    public void SetDragging(bool isDragging)
    {
        isBeingDragged = isDragging;
        if (isDragging)
        {
            rb.linearVelocity = Vector3.zero;  // Stop movement instantly
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            // Allow movement to resume after dragging
            timeOffset = Random.Range(0f, 100f);  // Randomize to change direction
        }
    }
}
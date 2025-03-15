using UnityEngine;

public class Drag : MonoBehaviour
{
    private Camera cam;
    private Vector3 offset;
    private Rigidbody selectedSheep;
    private SheepKill sheepKill;

    private Vector3 previousPosition;
    private Vector3 currentVelocity;

    public float maxVelocity = 10f; // Maximum velocity to prevent excessive speed

    private float initialZ; // To store the initial Z position of the sheep

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Sheep"))
            {
                selectedSheep = hit.collider.GetComponent<Rigidbody>();
                sheepKill = hit.collider.GetComponent<SheepKill>();

                if (sheepKill != null)
                    sheepKill.SetDragging(true); // Stop movement

                offset = hit.point - selectedSheep.transform.position;
                selectedSheep.useGravity = false;
                selectedSheep.constraints = RigidbodyConstraints.FreezeRotation; // Prevent rotation during drag

                // Store the initial position for velocity calculation and Z locking
                initialZ = selectedSheep.transform.position.z;
                previousPosition = selectedSheep.transform.position;
            }
        }

        if (Input.GetMouseButton(0) && selectedSheep)
        {
            // Get mouse position in world coordinates, maintaining the object's original z-position
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = cam.WorldToScreenPoint(selectedSheep.transform.position).z;
            Vector3 newPos = cam.ScreenToWorldPoint(mousePos);

            // Apply offset and set position
            newPos = newPos - offset;
            newPos.z = initialZ; // Lock the Z position to the initial Z value

            // Prevent the sheep from leaving the camera's view
            newPos = ClampPositionToScreenBounds(newPos);

            // Use MovePosition for consistent physics behavior
            selectedSheep.MovePosition(newPos);

            // Calculate velocity by comparing the current position to the previous frame
            currentVelocity = (selectedSheep.transform.position - previousPosition) / Time.deltaTime;
            previousPosition = selectedSheep.transform.position;
        }

        // Always freeze the Z position during normal movement (when not dragging)
        if (selectedSheep != null)
        {
            Vector3 currentPosition = selectedSheep.transform.position;
            currentPosition.z = initialZ; // Always lock the Z position to the initial value
            selectedSheep.transform.position = currentPosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (selectedSheep)
            {
                // Apply the momentum when released
                ApplyReleaseMomentum(currentVelocity);

                selectedSheep.useGravity = true;
                selectedSheep.constraints = RigidbodyConstraints.None; // Re-enable rotation

                if (sheepKill != null)
                    sheepKill.SetDragging(false); // Resume movement

                selectedSheep = null;
                sheepKill = null;
            }
        }
    }

    // Clamp position to the screen bounds (to prevent sheep from going out of view)
    private Vector3 ClampPositionToScreenBounds(Vector3 worldPosition)
    {
        Vector3 screenPos = cam.WorldToScreenPoint(worldPosition);

        // Get the screen boundaries (0,0 is bottom-left and Screen.width, Screen.height is top-right)
        screenPos.x = Mathf.Clamp(screenPos.x, 0, Screen.width);
        screenPos.y = Mathf.Clamp(screenPos.y, 0, Screen.height);

        // Convert the clamped screen position back to world space
        worldPosition = cam.ScreenToWorldPoint(screenPos);

        return worldPosition;
    }

    // Apply momentum to the sheep's Rigidbody when released
    private void ApplyReleaseMomentum(Vector3 momentum)
    {
        if (selectedSheep != null)
        {
            // Apply the velocity to the Rigidbody
            selectedSheep.linearVelocity = momentum;

            // Clamp the velocity to the maximum allowed value to prevent excessive speed
            selectedSheep.linearVelocity = Vector3.ClampMagnitude(selectedSheep.linearVelocity, maxVelocity);
        }
    }
}

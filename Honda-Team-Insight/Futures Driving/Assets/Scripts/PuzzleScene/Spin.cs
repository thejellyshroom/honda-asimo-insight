using UnityEngine;

public class Spin : MonoBehaviour
{
    private float y;
    private float rotationSpeed;

    void Start()
    {
        y = transform.localRotation.eulerAngles.y; // Initialize with current Y rotation
        rotationSpeed = 15.0f;
    }

    void FixedUpdate()
    {
        y += Time.deltaTime * rotationSpeed;

        // Preserve existing X and Z rotations while updating Y
        Vector3 currentRotation = transform.localRotation.eulerAngles;
        transform.localRotation = Quaternion.Euler(currentRotation.x, y, currentRotation.z);
    }
}

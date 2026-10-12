using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Transform ground;
    public Transform launcherObject;

    public float moveSpeed = 0.5f;

    Vector3 moveDirection;

    private void Start()
    {
        if (launcherObject == null)
        {
            Debug.LogWarning("Reference object not assigned.");
            return;
        }

        // Store the launcher's direction relative to the ground (local-space instead of world-space)
        moveDirection = ground.InverseTransformDirection(launcherObject.forward);
    }

    // Update is called once per frame
    void Update()
    {
        // Move the projectile in world-space after calculating the direction
        Vector3 currentDirection = ground.TransformDirection(moveDirection);

        currentDirection.Normalize();

        transform.position += currentDirection * moveSpeed * Time.deltaTime;
    }
}

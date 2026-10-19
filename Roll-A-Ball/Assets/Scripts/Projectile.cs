using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Transform ground;
    public Transform launcherObject;

    public Transform groundCheck;
    public float groundDistance;
    public LayerMask groundMask;
    public bool isGrounded;
    private bool wasGrounded;

    public float moveSpeed = 0.5f;

    Vector3 moveDirection;

    private void Start()
    {
        if (launcherObject == null)
        {
            Debug.LogWarning("Reference object not assigned.");
            return;
        }

        groundDistance = 0.4f;

        // Store the launcher's direction relative to the ground (local-space instead of world-space)
        moveDirection = ground.InverseTransformDirection(launcherObject.forward);
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (wasGrounded && !isGrounded)
        {
            moveDirection *= -1f;
        }

        wasGrounded = isGrounded;

        // Move the projectile in world-space after calculating the direction
        Vector3 currentDirection = ground.TransformDirection(moveDirection).normalized;

        transform.position += currentDirection * moveSpeed * Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
}

using UnityEngine;

// A class that creates an offset that the camera can stick to
// to remain at a distance from the player
public class FollowTarget : MonoBehaviour
{
    // Player reference
    [SerializeField] public Transform player;
    // Fixed y and z coordinates
    [SerializeField] public float fixedY = 3.5f;
    [SerializeField] private float fixedZ = -5.5f;

    private void LateUpdate()
    {
        // Follows the player on only the x axis
        transform.position = new Vector3(
            player.position.x,
            fixedY,
            fixedZ
        );
    }
}

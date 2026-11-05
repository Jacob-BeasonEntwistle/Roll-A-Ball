using UnityEngine;

public class DebuggingSpawns : MonoBehaviour
{
    public void OnDrawGizmos()
    {
        Gizmos.color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, 0.5f);
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}

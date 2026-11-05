using UnityEngine;

public class Spin : MonoBehaviour
{
    public float spinSpeed;

    // Update is called once per frame
    void Update()
    {
        float yRotation = spinSpeed * Time.deltaTime;

        transform.Rotate(0, yRotation, 0);
    }
}

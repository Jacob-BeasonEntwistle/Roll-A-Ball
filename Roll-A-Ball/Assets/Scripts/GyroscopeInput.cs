using UnityEngine;

public class GyroscopeInput : MonoBehaviour
{
    public float smoothSpeed = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Input.gyro.enabled = true;
        Debug.Log("Gyro Enabled!");
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion targetRotation = GyroToUnity(Input.gyro.attitude);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            smoothSpeed * Time.deltaTime
        );
    }

    private Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
}

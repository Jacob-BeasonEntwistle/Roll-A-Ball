using System.Collections;
using UnityEngine;

public class GyroscopeInput : MonoBehaviour
{
    // Original orientation that the user holds the device in when starting the game
    private Quaternion referenceRotation;

    public float smoothSpeed = 5f;

    private bool gyroReady = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Input.gyro.enabled = false;
        StartCoroutine(InitialiseGyro());
    }

    // Update is called once per frame
    void Update()
    {
        // If the gyro hasn't finished initalising (during 3s countdown),
        // don't perform any updates yet
        if (!gyroReady) return;

        // Get the current rotation of the gyroscope
        Quaternion currentRotation = GyroToUnity(Input.gyro.attitude);
        // Calculate how far the device has rotated away from the starting position
        Quaternion relativeRotation = Quaternion.Inverse(referenceRotation) * currentRotation;

        // Convert the quaternion rotation into euler angles
        Vector3 eulerAngles = relativeRotation.eulerAngles;

        // DeltaAngle is used to return a value between -180 and 180 degrees
        // Useful to balance the device tilt controls
        float signedPitch = Mathf.DeltaAngle(0, eulerAngles.x);
        float signedRoll = Mathf.DeltaAngle(0, eulerAngles.y);

        // Clamp the pitch and roll to -35 and 35 degrees
        float pitch = Mathf.Clamp(signedPitch, -35.0f, 35.0f);
        float roll = Mathf.Clamp(signedRoll, -35.0f, 35.0f);


        /* Debug.Log($"Pitch: {pitch} Roll: {roll}"); */


        // Find the target rotation using the current pitch and roll
        Quaternion target = Quaternion.Euler(pitch, 0, roll);

        // Apply the Pitch and Roll to the platform, but discard the Yaw
        transform.rotation = Quaternion.Slerp(transform.rotation, target, smoothSpeed * Time.deltaTime);
    }

    private Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }

    IEnumerator InitialiseGyro()
    {
        Input.gyro.enabled = true;

        yield return new WaitForSeconds(4);

        // Saving the current phone orientation, this = flat
        referenceRotation = GyroToUnity(Input.gyro.attitude);
        gyroReady = true;

        Debug.Log("Gyro Calibrated!");
    }

    public void RecenterGyro()
    {
        referenceRotation = GyroToUnity(Input.gyro.attitude);
        gyroReady = true;
    }
}

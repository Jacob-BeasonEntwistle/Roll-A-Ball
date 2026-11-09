using System.Collections;
using UnityEngine;

public class GyroscopeInput : MonoBehaviour
{
    // Original orientation that the user holds the device in when starting the game
    private Quaternion referenceRotation;

    public float smoothSpeed = 5f;

    public bool gyroReady = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(InitialiseGyro());
    }

    // Update is called once per frame
    void Update()
    {
        // If the gyro hasn't finished initalising (during 3s countdown),
        // don't perform any updates yet
        if (!gyroReady)
        {
            //Debug.Log("Waiting...");
            return;
        }
        if (gyroReady)
        {
            Debug.Log(Input.gyro.attitude);
        }

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
        Debug.Log("Supports gyro: " + SystemInfo.supportsGyroscope);

        // Enable gyro input
        Input.gyro.enabled = true;

        yield return new WaitForSeconds(2f);

        Debug.Log("Gyro enabled: " + Input.gyro.enabled);
        Debug.Log("Initial attitude: " + Input.gyro.attitude);

        //// Get a sample of the current gyro readings
        //Quaternion firstSample = Input.gyro.attitude;

        //// Create timer and movement detected variables
        //float timer = 0f;
        //bool movementDetected = false;

        //// This occurs over a 3s timeframe
        //while (timer < 3f)
        //{
        //    // Get another sample of the gyro readings
        //    Quaternion currentSample = Input.gyro.attitude;

        //    // Calculate the difference between the second sample from the first
        //    float angleDifference = Quaternion.Angle(currentSample, firstSample);

        //    // If the angle has moved more than 1 degree
        //    if (angleDifference > 1f)
        //    {
        //        // Break the loop and set the movement detected to true
        //        movementDetected = true;
        //        break;
        //    }

        //    timer += Time.deltaTime;
        //    yield return null;
        //}

        //// Send a warning message to the console if there is no movement detected
        //if (!movementDetected)
        //{
        //    Debug.LogWarning("No gyro movement detected.");
        //}

        // Saving the current phone orientation, this = flat
        referenceRotation = GyroToUnity(Input.gyro.attitude);

        Debug.Log("Setting gyroReady true");
        gyroReady = true;

        Debug.Log("Gyro Calibrated!");
    }

    public void RecenterGyro()
    {
        // Get the current readings from the gyro and save them as the new reference
        referenceRotation = GyroToUnity(Input.gyro.attitude);
        gyroReady = true;
    }
}

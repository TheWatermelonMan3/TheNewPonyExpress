using UnityEngine;

public class YawAnchor : MonoBehaviour
{
    public Transform car; // assign your car in the Inspector
    public float pitchdown;
    public float turnAmount;

    void LateUpdate()
    {
        if (car == null) return;

        // Match position
        transform.position = car.position;

        // Copy only yaw (rotation around Y axis)

        Vector3 euler = car.eulerAngles;

        float turnInput = Input.GetAxis("Horizontal");

        transform.rotation = Quaternion.Euler(0f, euler.y + turnInput * turnAmount, 0f) * Quaternion.Euler(pitchdown, 0f, 0f);
    }
}

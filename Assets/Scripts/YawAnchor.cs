using UnityEngine;

public class YawAnchor : MonoBehaviour
{
    public Transform car; // assign your car in the Inspector

    void LateUpdate()
    {
        if (car == null) return;

        // Match position
        transform.position = car.position;

        // Copy only yaw (rotation around Y axis)
        Vector3 euler = car.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, euler.y, 0f);
    }
}

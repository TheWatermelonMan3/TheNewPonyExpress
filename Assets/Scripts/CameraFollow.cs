using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform Car;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject carObject = GameObject.Find("Car");
        if (carObject != null)
        {
            Car = carObject.transform;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Car == null) return;

        Vector3 camPos = transform.position;
        camPos.x = Car.position.x;
        camPos.z = Car.position.z;

        transform.position = camPos;
    }
}

using UnityEngine;

public class CarController : MonoBehaviour
{
    public float acceleration = 10f;
    public float maxspeed = 20f;
    public float turnspeed = 2.5f;

    private Rigidbody rb;

    private bool driftEngage = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Debug.Log("found rigidbody " + rb);
    }

    void Update()
    {
        // Acceleration
        float moveInput = Input.GetAxis("Vertical");
        Vector3 force = transform.forward * moveInput * acceleration;
        // Slower in Reverse
        if (moveInput < 0) force = force * 0.5f;

        // Driving
        if (rb.linearVelocity.magnitude < maxspeed * (Vector3.Dot(rb.linearVelocity, transform.forward) >= 0 ? 1f : 0.5f))
        {
            rb.AddForce(force, ForceMode.Acceleration);
        }

        // Steering
        float turnInput = Input.GetAxis("Horizontal");
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            float turn = turnInput * rb.linearVelocity.magnitude * turnspeed * Time.fixedDeltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);

            // Tracking Direction
            if (!driftEngage)
            {
                if (Vector3.Dot(rb.linearVelocity, transform.forward) >= 0)
                {
                    rb.linearVelocity = transform.forward * rb.linearVelocity.magnitude;
                }
                else
                {
                    rb.linearVelocity = -1 * transform.forward * rb.linearVelocity.magnitude;
                }
            }
        }
    }

}

using UnityEngine;

public class CarController : MonoBehaviour
{
    public float acceleration = 10f;
    public float maxspeed = 20f;
    public float turnspeed = 2.5f;

    public float traction = 1.0f;
    //public float extragravity = 10f;

    private Rigidbody rb;

    private bool driftEngage = false;
    //public float hoverHeight = 1f;

    //public float hoverForce = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Debug.Log("found rigidbody " + rb);

        rb.ResetInertiaTensor();
        Vector3 tensor = rb.inertiaTensor;
        tensor.y *= 5f; // increase resistance to yaw by 2×
        tensor.x *= 0.2f; // increase resistance to roll by 5×
        rb.inertiaTensor = tensor;
    }

    void Update()
    {

        /*if (Input.GetKeyUp(KeyCode.Space)){
            rb.AddForce(Vector3.up * 100, ForceMode.Impulse);
        }*/
        // Acceleration
        float moveInput = Input.GetAxis("Vertical");

        Vector3 flatforward = transform.forward;
        Vector3 flatvelocity = rb.linearVelocity;
        flatvelocity.y = 0f;
        flatforward.y = 0f;

        Vector3 force = flatforward * moveInput * acceleration;
        // Slower in Reverse
        if (moveInput < 0) force = force * 0.5f;

        //rb.AddForce(Vector3.down * extragravity, ForceMode.Acceleration);

        // Driving
        if (rb.linearVelocity.magnitude < maxspeed * (Vector3.Dot(flatvelocity, flatforward) >= 0 ? 1f : 0.5f) || Vector3.Dot(flatvelocity, force) < 0)
        {
            rb.AddForce(force, ForceMode.Acceleration);
        }


        //rb.AddForce(Vector3.down * 2.0f, ForceMode.Acceleration);

        /*if (Physics.Raycast(transform.position, -Vector3.up, out RaycastHit hit, hoverHeight))
        {
            float compression = (hoverHeight - hit.distance) / hoverHeight;
            rb.AddForce(Vector3.up * compression * hoverForce, ForceMode.Acceleration);
        }*/

        // Steering
        float turnInput = Input.GetAxis("Horizontal");
        if (flatvelocity.magnitude > 0.1f)
        {
            float turn = turnInput * flatvelocity.magnitude * turnspeed * Time.fixedDeltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);

            // Tracking Direction
            if (!driftEngage)
            {

                float speed = flatvelocity.magnitude;
                Vector3 targetvelocity = flatforward * speed * (Vector3.Dot(rb.linearVelocity, flatforward) >= 0 ? 1f : -1f);
                rb.AddForce(traction * (targetvelocity - flatvelocity), ForceMode.Acceleration);
            }
        }

        //Quaternion uprightRotation = Quaternion.FromToRotation(transform.up, Vector3.up) * transform.rotation;
        //Quaternion smoothedRotation = Quaternion.Slerp(rb.rotation, uprightRotation, 0.1f);
        //rb.MoveRotation(smoothedRotation);
    }

}

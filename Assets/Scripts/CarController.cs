using UnityEngine;

public class CarController : MonoBehaviour
{
    public float acceleration = 10f;
    public float maxspeed = 20f;
    public float turnspeed = 2.5f;

    public float driftcontrol = 4.0f;
    public float driftturnspeed = 8.0f;

    public float traction = 1.0f;
    //public float extragravity = 10f;
    public GameObject SkidMark;

    public Transform wheel1;
    public Transform wheel2;
    public Transform wheel3;
    public Transform wheel4;

    private TrailRenderer skid1;
    private TrailRenderer skid2;
    private TrailRenderer skid3;
    private TrailRenderer skid4;

    private Rigidbody rb;

    private bool driftEngage = false;
    private float turnMemory = 0f;
    private float turnAmount = 0f;
    private float turnInput = 0f;
    //public float hoverHeight = 1f;

    //public float hoverForce = 1f;

    [SerializeField] private Vector3 flatvelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Debug.Log("found rigidbody " + rb);

        rb.ResetInertiaTensor();
        Vector3 tensor = rb.inertiaTensor;
        tensor.y *= 5f; // increase resistance to yaw by 2×
        tensor.x *= 0.2f; // increase resistance to roll by 5×
        rb.inertiaTensor = tensor;

        GameObject trailprefab1 = Instantiate(SkidMark, wheel1.position, Quaternion.identity, wheel1);
        skid1 = trailprefab1.GetComponent<TrailRenderer>();
        skid1.emitting = false;
        GameObject trailprefab2 = Instantiate(SkidMark, wheel2.position, Quaternion.identity, wheel2);
        skid2 = trailprefab2.GetComponent<TrailRenderer>();
        skid2.emitting = false;
        GameObject trailprefab3 = Instantiate(SkidMark, wheel3.position, Quaternion.identity, wheel3);
        skid3 = trailprefab3.GetComponent<TrailRenderer>();
        skid3.emitting = false;
        GameObject trailprefab4 = Instantiate(SkidMark, wheel4.position, Quaternion.identity, wheel4);
        skid4 = trailprefab4.GetComponent<TrailRenderer>();
        skid4.emitting = false;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!driftEngage) turnMemory = turnInput * driftturnspeed;
            driftEngage = true;
        }
        else
        {
            if (driftEngage) turnMemory = 0f;
            driftEngage = false;
        }

        //Jump
        /*if (Input.GetKeyUp(KeyCode.Space)){
            rb.AddForce(Vector3.up * 100, ForceMode.Impulse);
        }*/

        // Acceleration
        float moveInput = Input.GetAxis("Vertical");

        Vector3 flatforward = transform.forward;
        flatvelocity = rb.linearVelocity;
        flatvelocity.y = 0f;
        flatforward.y = 0f;

        Vector3 speedproj = Vector3.Project(flatvelocity, flatforward);

        Vector3 force = flatforward * moveInput * acceleration;
        // Slower in Reverse
        if (moveInput < 0) force = force * 0.5f;

        //rb.AddForce(Vector3.down * extragravity, ForceMode.Acceleration);

        float dragCoeff = acceleration / (maxspeed * maxspeed);
        Vector3 drag = (-1 * dragCoeff * flatvelocity.magnitude) * flatvelocity;

        if (IsGrounded())
        {
            // Driving
            rb.AddForce(force, ForceMode.Acceleration);
            rb.AddForce(drag, ForceMode.Acceleration);

            /*if (speedproj.magnitude < maxspeed * (Vector3.Dot(flatvelocity, flatforward) >= 0 ? 1f : 0.5f) || Vector3.Dot(flatvelocity, force) < 0)
            {
                rb.AddForce(force, ForceMode.Acceleration);
            }*/

            // Steering
            turnInput = Input.GetAxis("Horizontal");

            if (flatvelocity.magnitude > 0.1f)
            {
                if (!driftEngage) turnAmount = turnInput * flatvelocity.magnitude * turnspeed * Time.fixedDeltaTime;
                else turnAmount = turnMemory * Time.fixedDeltaTime + (turnInput * driftcontrol * Time.fixedDeltaTime);
                Quaternion turnRotation = Quaternion.Euler(0f, turnAmount, 0f);
                rb.MoveRotation(rb.rotation * turnRotation);

                // Tracking Direction
                if (!driftEngage)
                {

                    float speed = flatvelocity.magnitude;
                    Vector3 targetvelocity = flatforward * speed * (Vector3.Dot(rb.linearVelocity, flatforward) >= 0 ? 1f : -1f);
                    rb.AddForce(traction * (targetvelocity - flatvelocity), ForceMode.Acceleration);
                }
            }

            if (driftEngage)
            {
                skid1.emitting = true;
                skid2.emitting = true;
                skid3.emitting = true;
                skid4.emitting = true;
            } else
            {
                skid1.emitting = false;
                skid2.emitting = false;
                skid3.emitting = false;
                skid4.emitting = false;
            }

        } else
        {
            skid1.emitting = false;
            skid2.emitting = false;
            skid3.emitting = false;
            skid4.emitting = false;
        }
    }

    private bool IsGrounded()
    {
        RaycastHit hit;

        // Cast a ray straight down from the car’s center
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f))
        {
            // Ground is between 0.4 and 2 units below the car
            if (hit.distance >= 0.4f && hit.distance <= 1.1f)
            {
                return true;
            }
        }

        return false;
    }

}
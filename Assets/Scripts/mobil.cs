using UnityEngine;

public class mobil : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private bool usemajuAndBrake = true;

    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;
    private Rigidbody rb;

    [Header("Settings")]
    [SerializeField] private float motorForce = 1500f;
    [SerializeField] private float breakForce = 3000f;
    [SerializeField] private float maxSteerAngle = 30f;
    [SerializeField] private float antiRollForce = 5000f;

    [Header("Center Of Mass")]
    [SerializeField] private Vector3 centerOfMass = new Vector3(0, -0.5f, 0);

    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [Header("Wheel Meshes")]
    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.centerOfMass = centerOfMass;
        }
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        AntiRoll();
    }

    private void GetInput()
    {
        if (usemajuAndBrake)
        {
            horizontalInput = SimpleInput.GetAxis("Horizontal");

            verticalInput = 0f;
            isBreaking = false;

            bool maju =
                SimpleInput.GetButton("maju") ||
                Input.GetKey(KeyCode.W) ||
                Input.GetKey(KeyCode.UpArrow);

            bool mundur =
                SimpleInput.GetButton("mundur") ||
                Input.GetKey(KeyCode.S) ||
                Input.GetKey(KeyCode.DownArrow);

            if (maju)
                verticalInput = 1f;
            else if (mundur)
                verticalInput = -1f;
        }
        else
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            isBreaking = Input.GetKey(KeyCode.Space);
        }
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;

        currentbreakForce = isBreaking ? breakForce : 0f;

        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;

        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;

        wheelCollider.GetWorldPose(out pos, out rot);

        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }

    private void AntiRoll()
    {
        WheelHit hit;
        float travelL = 1f;
        float travelR = 1f;

        bool groundedL = frontLeftWheelCollider.GetGroundHit(out hit);

        if (groundedL)
        {
            travelL =
                (-frontLeftWheelCollider.transform.InverseTransformPoint(hit.point).y
                - frontLeftWheelCollider.radius)
                / frontLeftWheelCollider.suspensionDistance;
        }

        bool groundedR = frontRightWheelCollider.GetGroundHit(out hit);

        if (groundedR)
        {
            travelR =
                (-frontRightWheelCollider.transform.InverseTransformPoint(hit.point).y
                - frontRightWheelCollider.radius)
                / frontRightWheelCollider.suspensionDistance;
        }

        float force = (travelL - travelR) * antiRollForce;

        if (groundedL)
        {
            rb.AddForceAtPosition(
                frontLeftWheelCollider.transform.up * -force,
                frontLeftWheelCollider.transform.position
            );
        }

        if (groundedR)
        {
            rb.AddForceAtPosition(
                frontRightWheelCollider.transform.up * force,
                frontRightWheelCollider.transform.position
            );
        }

        bool groundedRearL = rearLeftWheelCollider.GetGroundHit(out hit);

        if (groundedRearL)
        {
            travelL =
                (-rearLeftWheelCollider.transform.InverseTransformPoint(hit.point).y
                - rearLeftWheelCollider.radius)
                / rearLeftWheelCollider.suspensionDistance;
        }

        bool groundedRearR = rearRightWheelCollider.GetGroundHit(out hit);

        if (groundedRearR)
        {
            travelR =
                (-rearRightWheelCollider.transform.InverseTransformPoint(hit.point).y
                - rearRightWheelCollider.radius)
                / rearRightWheelCollider.suspensionDistance;
        }

        force = (travelL - travelR) * antiRollForce;

        if (groundedRearL)
        {
            rb.AddForceAtPosition(
                rearLeftWheelCollider.transform.up * -force,
                rearLeftWheelCollider.transform.position
            );
        }

        if (groundedRearR)
        {
            rb.AddForceAtPosition(
                rearRightWheelCollider.transform.up * force,
                rearRightWheelCollider.transform.position
            );
        }
    }
}
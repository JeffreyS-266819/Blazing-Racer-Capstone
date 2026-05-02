using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class CarController : MonoBehaviour
{
    [Header("Wheel Colliders")]
    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider rearLeft;
    public WheelCollider rearRight;

    [Header("Wheel Visuals")]
    public Transform frontLeftMesh;
    public Transform frontRightMesh;
    public Transform rearLeftMesh;
    public Transform rearRightMesh;

    [Header("Driving")]
    public float motorTorque = 3500f;
    public float maxSteerAngle = 30f;
    public float brakeTorque = 4000f;

    [Header("Auto Stop")]
    public float autoBrakeTorque = 1200f;
    public float autoBrakeSpeed = 0.5f;

    [Header("Tuning")]
    public float downforce = 80f;
    public Vector3 centerOfMassOffset = new Vector3(0f, -0.4f, 0f);

    [Header("Drive Type")]
    public bool rearWheelDrive = true;

    [Header("Audio")]
    public AudioClip idleClip;
    public AudioClip engineRoarClip;
    public AudioClip tireScreechClip;

    private AudioSource audioSource;
    private Rigidbody rb;

    private Quaternion flOffset, frOffset, rlOffset, rrOffset;

    private bool isTouchingGround;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass += centerOfMassOffset;

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
    }

    void Start()
    {
        flOffset = ComputeMeshOffset(frontLeft, frontLeftMesh);
        frOffset = ComputeMeshOffset(frontRight, frontRightMesh);
        rlOffset = ComputeMeshOffset(rearLeft, rearLeftMesh);
        rrOffset = ComputeMeshOffset(rearRight, rearRightMesh);
    }

    void FixedUpdate()
    {
        ReadInputs(out float steer, out float throttle, out bool braking);

        float speed = rb.linearVelocity.magnitude;

        // Detect if wheels are touching ground
        isTouchingGround = frontLeft.isGrounded || frontRight.isGrounded ||
                           rearLeft.isGrounded || rearRight.isGrounded;

        HandleAudio(throttle, braking);

        // Steering
        float steerAngle = steer * maxSteerAngle;
        frontLeft.steerAngle = steerAngle;
        frontRight.steerAngle = steerAngle;

        // Reset forces
        frontLeft.motorTorque = frontRight.motorTorque = 0f;
        rearLeft.motorTorque = rearRight.motorTorque = 0f;

        frontLeft.brakeTorque = frontRight.brakeTorque = 0f;
        rearLeft.brakeTorque = rearRight.brakeTorque = 0f;

        // Motor torque
        float torque = throttle * motorTorque;

        if (rearWheelDrive)
        {
            rearLeft.motorTorque = torque;
            rearRight.motorTorque = torque;
        }
        else
        {
            frontLeft.motorTorque = torque;
            frontRight.motorTorque = torque;
        }

        // Braking
        if (braking)
        {
            frontLeft.brakeTorque = brakeTorque;
            frontRight.brakeTorque = brakeTorque;
            rearLeft.brakeTorque = brakeTorque;
            rearRight.brakeTorque = brakeTorque;
        }

        // Auto brake
        bool noThrottle = Mathf.Abs(throttle) < 0.05f;

        if (!braking && noThrottle)
        {
            float dynamicBrake = autoBrakeTorque * Mathf.Clamp01(speed / 10f);

            frontLeft.brakeTorque = dynamicBrake;
            frontRight.brakeTorque = dynamicBrake;
            rearLeft.brakeTorque = dynamicBrake;
            rearRight.brakeTorque = dynamicBrake;

            if (speed < autoBrakeSpeed)
            {
                frontLeft.brakeTorque = brakeTorque;
                frontRight.brakeTorque = brakeTorque;
                rearLeft.brakeTorque = brakeTorque;
                rearRight.brakeTorque = brakeTorque;

                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        // Downforce
        rb.AddForce(-transform.up * downforce * speed);

        // Wheel visuals
        UpdateWheelVisual(frontLeft, frontLeftMesh, flOffset);
        UpdateWheelVisual(frontRight, frontRightMesh, frOffset);
        UpdateWheelVisual(rearLeft, rearLeftMesh, rlOffset);
        UpdateWheelVisual(rearRight, rearRightMesh, rrOffset);
    }

    void HandleAudio(float throttle, bool braking)
    {
        // Priority: Brake > Engine > Idle

        if (braking && isTouchingGround)
        {
            if (audioSource.clip != tireScreechClip)
            {
                audioSource.clip = tireScreechClip;
                audioSource.Play();
            }
        }
        else if (throttle > 0.1f && isTouchingGround)
        {
            if (audioSource.clip != engineRoarClip)
            {
                audioSource.clip = engineRoarClip;
                audioSource.Play();
            }
        }
        else if (isTouchingGround)
        {
            if (audioSource.clip != idleClip)
            {
                audioSource.clip = idleClip;
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    static void ReadInputs(out float steer, out float throttle, out bool braking)
    {
        steer = 0f;
        throttle = 0f;
        braking = false;

        if (Keyboard.current != null)
        {
            steer = (Keyboard.current.dKey.isPressed ? 1f : 0f)
                  - (Keyboard.current.aKey.isPressed ? 1f : 0f);

            throttle = (Keyboard.current.wKey.isPressed ? 1f : 0f)
                     - (Keyboard.current.sKey.isPressed ? 1f : 0f);

            braking = Keyboard.current.spaceKey.isPressed;
        }

        if (Gamepad.current != null)
        {
            steer = Gamepad.current.leftStick.x.ReadValue();
            throttle = Gamepad.current.rightTrigger.ReadValue()
                     - Gamepad.current.leftTrigger.ReadValue();
            braking = Gamepad.current.buttonSouth.isPressed;
        }

        steer = Mathf.Clamp(steer, -1f, 1f);
        throttle = Mathf.Clamp(throttle, -1f, 1f);
    }

    static Quaternion ComputeMeshOffset(WheelCollider col, Transform mesh)
    {
        if (col == null || mesh == null) return Quaternion.identity;

        col.GetWorldPose(out _, out Quaternion wheelRot);
        return Quaternion.Inverse(wheelRot) * mesh.rotation;
    }

    static void UpdateWheelVisual(WheelCollider col, Transform mesh, Quaternion offset)
    {
        if (col == null || mesh == null) return;

        col.GetWorldPose(out Vector3 pos, out Quaternion rot);
        mesh.SetPositionAndRotation(pos, rot * offset);
    }
}
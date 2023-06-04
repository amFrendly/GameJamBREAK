using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    Camera cam;

    [SerializeField]
    float jumpForce;

    [SerializeField]
    LayerMask groundMask;

    bool isGrounded;

    int lookAxis = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //Invert Y look axis
        if (Input.GetKeyDown(KeyCode.Keypad4)) lookAxis = -lookAxis;

        LookAround();

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    [SerializeField]
    float walkingSpeed;

    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(transform.position + Vector3.down * 0.93f, 0.1f, groundMask, QueryTriggerInteraction.Ignore);

        //Constantly walking at walkingSpeed!
        float deltaSpeed = walkingSpeed - rb.velocity.magnitude;
        if (deltaSpeed > 0 && isGrounded)rb.AddForce(new Vector3(transform.forward.x, 0, transform.forward.z).normalized * deltaSpeed, ForceMode.VelocityChange);
        if (isGrounded)
        {
            rb.AddForce(new Vector3(-rb.velocity.x, 0, -rb.velocity.z) * 0.5f, ForceMode.VelocityChange);
        }
    }

    [SerializeField]
    float mouseLookSpeed = 3, joyLookSpeed = 0.3f, lookXLimit;

    float rotationX;
    float currentAxisX, currentAxisY;

    private void LookAround()
    {
        currentAxisY = Smooth(currentAxisY, Input.GetAxisRaw("Vertical"));
        currentAxisX = Smooth(currentAxisX, Input.GetAxisRaw("Horizontal"));

        rotationX += -currentAxisY * joyLookSpeed * lookAxis; //Joystick
        rotationX += -Input.GetAxis("Mouse Y") * mouseLookSpeed;             //Mouse
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        cam.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, currentAxisX * joyLookSpeed, 0);  //Joystick
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * mouseLookSpeed, 0);     //Mouse
    }

    private float Smooth(float from, float to)
    {
        float error = Mathf.Clamp(to - from, -0.05f, 0.05f) * Time.unscaledDeltaTime * 100f;
        from += error;
        return from;
        
        //return Mathf.Lerp(f1, f2, Time.unscaledDeltaTime * 0.05f);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    isGrounded = true;
    //}

    //private void OnTriggerStay(Collider other)
    //{
    //    isGrounded = true;
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    isGrounded = false;
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    Camera cam;

    [SerializeField]
    float jumpForce;

    bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        LookAround();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    [SerializeField]
    float walkingSpeed;

    private void FixedUpdate()
    {
        //Constantly walking at walkingSpeed!
        float deltaSpeed = walkingSpeed - rb.velocity.magnitude;
        if (deltaSpeed > 0)rb.AddForce(new Vector3(transform.forward.x, 0, transform.forward.z).normalized * deltaSpeed, ForceMode.VelocityChange);
        if (isGrounded)
        {
            rb.AddForce(-rb.velocity * 0.9f, ForceMode.VelocityChange);
        }
    }

    [SerializeField]
    float lookSpeed, lookXLimit;

    float rotationX;

    private void LookAround()
    {
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        cam.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        isGrounded = true;
    }

    private void OnTriggerStay(Collider other)
    {
        isGrounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isGrounded = false;
    }
}

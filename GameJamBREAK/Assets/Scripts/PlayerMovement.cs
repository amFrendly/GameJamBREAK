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
        isGrounded = Physics.CheckSphere(transform.position + Vector3.down * 0.93f, 0.1f, groundMask, QueryTriggerInteraction.Ignore);

        //Constantly walking at walkingSpeed!
        float deltaSpeed = walkingSpeed - rb.velocity.magnitude;
        if (deltaSpeed > 0 && isGrounded)rb.AddForce(new Vector3(transform.forward.x, 0, transform.forward.z).normalized * deltaSpeed, ForceMode.VelocityChange);
        if (isGrounded)
        {
            rb.AddForce(new Vector3(-rb.velocity.x, 0, -rb.velocity.z) * 0.5f, ForceMode.VelocityChange);
        }
        Debug.Log(isGrounded);
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

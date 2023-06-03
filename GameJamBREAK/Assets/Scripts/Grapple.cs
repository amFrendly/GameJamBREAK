using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField]
    Camera cam;

    [SerializeField]
    float grappleForce, maxGrappleDistance;

    [SerializeField]
    Transform grappleBarrel;

    [SerializeField]
    LayerMask layerMask;

    [SerializeField]
    LayerMask groundMask;

    [SerializeField]
    LineRenderer line;

    Vector3 grapplePoint;

    bool grapple;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, maxGrappleDistance, layerMask))
            {
                grapplePoint = hit.point;
                grapple = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            grapple = false;
        }

        if (grapple)
        {
            line.enabled = true;
            line.SetPositions(new Vector3[] { grappleBarrel.position, grapplePoint });
        }
        else
        {
            line.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (grapple)
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, (grapplePoint - transform.position).magnitude - 0.5f, groundMask))
            {
                grapple = false;
                return;
            }

            Vector3 grappleDir = (grapplePoint - transform.position).normalized;
            if (Vector3.Dot(rb.velocity, grappleDir) < grappleForce || Vector3.Dot(rb.velocity.normalized, grappleDir) < Mathf.Cos(45 * Mathf.Deg2Rad)) rb.AddForce(grappleDir * grappleForce, ForceMode.VelocityChange);
        }
    }
}

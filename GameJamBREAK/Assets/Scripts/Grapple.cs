using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    float minGrappleDistance = 1;

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
            float grappleDist = (grapplePoint - grappleBarrel.position).magnitude;
            Vector3 grappleDir = (grapplePoint - grappleBarrel.position).normalized;

            if (Physics.Raycast(grappleBarrel.position, grappleDir, grappleDist - 0.5f, groundMask) || grappleDist < minGrappleDistance)
            {
                grapple = false;
                return;
            }

            float velocityRatio = 2 - (Vector3.Dot(rb.velocity.normalized, grappleDir) + 1);
            float distanceRatio = Mathf.Clamp01(grappleDist / 10);
            if (Vector3.Dot(rb.velocity, grappleDir) < grappleForce /*|| Vector3.Dot(rb.velocity.normalized, grappleDir) < Mathf.Cos(45 * Mathf.Deg2Rad)*/)
            {
                rb.AddForce(grappleDir * velocityRatio * grappleForce * distanceRatio, ForceMode.VelocityChange);
            }

            //rb.useGravity = false;
            //rb.velocity = Vector3.zero;

            //rb.position += grappleDir * grappleForce * Time.deltaTime;
        }
    }
}

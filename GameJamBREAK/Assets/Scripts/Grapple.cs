using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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


    [SerializeField] private GameObject hook;
    //[SerializeField] private Transform hookPoint;
    [SerializeField] private int segments = 100;
    [SerializeField] private float grappleAnimationTime = 1.0f;
    [SerializeField] private float amplitude = 3.0f;
    [SerializeField] private AnimationCurve dropOff;

    Vector3 grapplePoint;

    bool grapple;
    float grappleTime;
    Vector3 grapplePointNormal;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        line.positionCount = segments;
       // hook.transform.SetPositionAndRotation(grappleBarrel.position, grappleBarrel.rotation);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, maxGrappleDistance, layerMask))
            {
                grapplePoint = hit.point;
                grapplePointNormal = hit.normal;
                grappleTime = 0;
                grapple = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.Keypad6) || Input.GetKeyUp(KeyCode.Mouse1))
        {
            grapple = false;
        }

        if (grapple)
        {
            grappleTime += Time.deltaTime;
            //if(grappleTime >= )

            line.enabled = true;
            //line.SetPositions(new Vector3[] { grappleBarrel.position, grapplePoint });
            AnimateLine();
            //hook.SetActive(false);
        }
        else
        {
            line.enabled = false;
            hook.transform.SetPositionAndRotation(grappleBarrel.position, grappleBarrel.rotation);
            //hook.SetActive(true);
        }
    }

    private void AnimateLine()
    {
        
        float animTime = Mathf.InverseLerp(0, grappleAnimationTime, grappleTime); //animation time normalized 0 anim start 1 is end
        Vector3 toPoint = grapplePoint - grappleBarrel.position;
        Vector3 direction = toPoint.normalized;
        //Vector3 dirNormal = Vector3.Cross(direction, Vector3.up);
        //dirNormal = Vector3.Cross(direction, dirNormal);
        float distance = toPoint.magnitude * animTime;
        //float distanceMultiplier = animTime * 
        for (int i = 0; i < line.positionCount; i++)
        {
            float t = (float)i / segments; //how far along the line
            Vector3 position = grappleBarrel.position + t * distance * direction;

            Vector3 positionOffset = grappleBarrel.up * amplitude * Mathf.Sin(distance * t * Mathf.PI) * dropOff.Evaluate(t) * (1-animTime);
            positionOffset += grappleBarrel.right * amplitude * Mathf.Cos(distance * t * Mathf.PI) * dropOff.Evaluate(t) * (1 - animTime);
            position += positionOffset;

            line.SetPosition(i, position);
        }

        Vector3 hookPosition = grappleBarrel.position + distance * direction;
        Quaternion rotation = Quaternion.Slerp(grappleBarrel.rotation, Quaternion.LookRotation(-grapplePointNormal, Vector3.up), animTime);
        hook.transform.SetPositionAndRotation(hookPosition, rotation);
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

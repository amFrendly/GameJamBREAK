using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaActivation : MonoBehaviour
{
    KatanaSlicer katanaSlicer;
    Animator animator;

    [SerializeField] Transform katana;
    [SerializeField] float distance;
    [SerializeField] Rigidbody playerRB;
    [SerializeField] float distanceUsed;
    Collider currentSlice;

    private void Start()
    {
        katanaSlicer = katana.GetComponent<KatanaSlicer>();
        animator = katana.GetComponent<Animator>();
    }

    private void Update()
    {
        float extra = playerRB.velocity.magnitude;
        if (Physics.Raycast(transform.position, transform.forward + playerRB.velocity.normalized, out RaycastHit hit, distance + extra * distanceUsed, LayerMask.GetMask("Targets")))
        {
            if(currentSlice == hit.collider)
            {
                return;
            }

            currentSlice = hit.collider;
            animator.SetTrigger("swing");
            katanaSlicer.canSlice = true;

            if (extra > 30)
            {
                katanaSlicer.Slice(hit.collider);
            }
        }
        else
        {
            animator.ResetTrigger("swing");
            katanaSlicer.canSlice = false;
            currentSlice = null;
        }
    }

    private void OnDrawGizmos()
    {
        float extra = playerRB.velocity.magnitude;
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * distance + transform.forward * extra * distanceUsed);
    }
}

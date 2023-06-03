using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaActivation : MonoBehaviour
{
    KatanaSlicer katanaSlicer;
    Animator animator;

    [SerializeField] Transform katana;
    [SerializeField] float distance;

    private void Start()
    {
        katanaSlicer = katana.GetComponent<KatanaSlicer>();
        animator = katana.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, distance, LayerMask.GetMask("Targets")))
        {
            animator.SetBool("swing", true);
            katanaSlicer.canSlice = true;   
        }
        else
        {
            animator.SetBool("swing", false);
            katanaSlicer.canSlice = false;
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawLine(transform.position, transform.position + transform.forward * distance);
    }
}

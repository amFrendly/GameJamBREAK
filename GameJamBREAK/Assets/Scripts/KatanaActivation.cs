using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaActivation : MonoBehaviour
{
    Animator animator;

    [SerializeField] Transform katana;
    [SerializeField] float distance;
    [SerializeField] Rigidbody playerRB;
    KatanaSlicer2 katanaSlicer;

    [SerializeField] Vector2 detectionSize;

    private void Start()
    {
        animator = katana.GetComponent<Animator>();
        katanaSlicer = katana.GetComponent<KatanaSlicer2>();
    }

    private void Update()
    {
        float speed = playerRB.velocity.magnitude;
        Vector3 newDetectionSize = detectionSize + detectionSize * speed / 100;
        ExtDebug.DrawBoxCastBox(transform.position, new Vector3(newDetectionSize.x, newDetectionSize.y, 0), transform.forward, transform.rotation, distance + distance * speed / 100, Color.magenta);
        int targetLayer = LayerMask.GetMask("Targets");
        RaycastHit[] hits = Physics.BoxCastAll(transform.position, new Vector3(newDetectionSize.x, newDetectionSize.y, 0), transform.forward, transform.rotation, distance + distance * speed / 100,targetLayer);
        if(hits.Length > 0)
        {
            animator.SetTrigger("swing");
            katanaSlicer.canSlice = true;
            katanaSlicer.DoSlice(hits);
        }
        else
        {
            animator.ResetTrigger("swing");
            katanaSlicer.canSlice = false;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField] Camera cam;
    //[SerializeField] float distanceCheck = 100f;
    [SerializeField] Image crosshair;

    [Header("Layers")]
    [SerializeField] LayerMask grappleLayer;
    [SerializeField] LayerMask targetLayer;

    [Header ("Colors")]
    [SerializeField] Color defaultColor;
    [SerializeField] Color targetColor;
    [SerializeField] Color grappleColor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit))
        {
            //(killLayer & (1 << collision.gameObject.layer)) != 0)
            if ((grappleLayer & (1 << hit.transform.gameObject.layer)) != 0) crosshair.color = grappleColor;
            else if ((targetLayer & (1 << hit.transform.gameObject.layer)) != 0) crosshair.color = targetColor;
            else crosshair.color = defaultColor;
        }
    }
}

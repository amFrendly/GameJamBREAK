using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KatanaActivation : MonoBehaviour
{

    [SerializeField] float sliceTimer;
    float timer;
    bool canSlice = true;

    private void Update()
    {
        if (canSlice) return;

        timer += Time.deltaTime;
        if(timer >= sliceTimer)
        {
            canSlice = true;
        }
    }


    private void OnTriggerStay(Collider collider)
    {
        if (!canSlice) return; // cant slice so dont slice

        if (collider.gameObject.GetComponent<Collider>().gameObject.GetComponent<Collider>().gameObject.layer == LayerMask.NameToLayer("Targets"))
        {
            // Slice the gameObject inside the trigger
            GameObject meshHolder = KatanaSlice.Cut(collider.transform, transform, true);
            // add some force would be cool

            timer = 0;
            canSlice = false;

        }
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (!canSlice) return; // cant slice so dont slice

        if(collider.gameObject.layer == LayerMask.NameToLayer("Targets"))
        {
            // Slice the gameObject inside the trigger
            GameObject meshHolder = KatanaSlice.Cut(collider.transform, transform, true);
            // add some force would be cool

            timer = 0;
            canSlice = false;

        }
    }
}
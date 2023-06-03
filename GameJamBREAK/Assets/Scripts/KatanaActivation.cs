using System.Collections;
using System.Collections.Generic;
<<<<<<< Updated upstream
using Unity.VisualScripting;
=======
>>>>>>> Stashed changes
using UnityEngine;

public class KatanaActivation : MonoBehaviour
{
<<<<<<< Updated upstream

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
=======
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        
    }
}
>>>>>>> Stashed changes

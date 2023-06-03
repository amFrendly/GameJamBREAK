using System.Collections.Generic;
using UnityEngine;

public class KatanaSlicer : MonoBehaviour
{
    [SerializeField] Transform sliceWith;
    bool canSliceAnimation = false;

    List<Transform> dontSliceAgain = new List<Transform>();

    public void CanNotSlice()
    {
        canSliceAnimation = false;
    }

    public void CanSlice()
    {
        canSliceAnimation = true;
        dontSliceAgain.Clear();
    }

    private void Slice(Collider collider)
    {
        if (!canSliceAnimation) return;

        if (IsAlreadySliced(collider)) return;

        if (collider.gameObject.layer == LayerMask.NameToLayer("Targets"))
        {
            // Slice the gameObject inside the trigger
            GameObject meshHolder = KatanaSlice.Cut(collider.transform, sliceWith, true);
            dontSliceAgain.Add(meshHolder.transform);

            // add some force would be cool

        }
    }

    private bool IsAlreadySliced(Collider collider)
    {
        for (int i = 0; i < dontSliceAgain.Count; i++)
        {
            for (int k = 0; k < dontSliceAgain[i].childCount; k++)
            {
                Transform transform = dontSliceAgain[i].GetChild(k);
                if (transform == collider.transform)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void OnTriggerStay(Collider collider)
    {
        Slice(collider);
    }
}
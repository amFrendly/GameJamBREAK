using System.Collections.Generic;
using UnityEngine;

public class KatanaSlicer : MonoBehaviour
{
    [SerializeField] Transform sliceWith;
    bool canSliceAnimation = true;
    public bool canSlice = true;

    List<Transform> dontSliceAgain = new List<Transform>();

    public delegate void OnSlice();
    public OnSlice onSlice;

    public void CanNotSlice()
    {
        canSliceAnimation = false;
    }

    public void CanSlice()
    {
        canSliceAnimation = true;
        dontSliceAgain.Clear();
    }

    public void Slice(Collider collider)
    {
        if (!canSliceAnimation) return;
        if (!canSlice) return;

        if (IsAlreadySliced(collider)) return;

        if (collider.gameObject.layer == LayerMask.NameToLayer("Targets"))
        {
            // Slice the gameObject inside the trigger
            GameObject meshHolder = KatanaSlice.Cut(collider.transform, sliceWith, true);
            dontSliceAgain.Add(meshHolder.transform);
            onSlice?.Invoke();
            if(collider.gameObject.TryGetComponent(out Destructable destructable))
            {
                destructable.Sliced();
            }
            // add some force would be cool

        }
    }

    private bool IsAlreadySliced(Collider collider)
    {
        for (int i = 0; i < dontSliceAgain.Count; i++)
        {
            for (int k = 0; k < dontSliceAgain[i].childCount; k++)
            {
                if (dontSliceAgain[i] == null) continue;
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
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class KatanaSlicer2 : MonoBehaviour
{
    [SerializeField] Transform sliceWith;
    [SerializeField] Rigidbody rigidbody;
    [SerializeField] Transform debugSlice;
    bool canSliceAnimation = true;
    public bool canSlice;

    [SerializeField] Vector3 detectionSize;
    Vector3 offset;

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

    //Vector3 detectionSize = new Vector3(1.5f, 0.2f, 0.3f);
    float distance;
    public void DoSlice(RaycastHit[] canhit)
    {
        // Find what to slice
        float speed = rigidbody.velocity.magnitude;
        Vector3 newDetectionSize = detectionSize + detectionSize * speed / 100;
        Vector3 offset = transform.right * newDetectionSize.x - transform.up * newDetectionSize.z + transform.up / 2;
        RaycastHit[] hits = Physics.BoxCastAll(sliceWith.position + offset, newDetectionSize, sliceWith.forward, sliceWith.rotation, distance, LayerMask.GetMask("Targets"));
        // slice the stuff
        if (hits.Length == 0) return;

        List<Collider> colliders = new List<Collider>();

        for(int i = 0; i < canhit.Length; i++)
        {
            for(int k = 0; k < hits.Length; k++)
            {
                if (canhit[i].collider.transform == hits[k].collider.transform)
                {
                    colliders.Add(canhit[i].collider);
                }
            }
        }

        for (int i = 0; i < colliders.Count; i++)
        {
            Slice(colliders[i]);
        }
    }



    private void Update()
    {
        float speed = rigidbody.velocity.magnitude;
        Vector3 newDetectionSize = detectionSize + detectionSize * speed / 100;
        Vector3 offset = transform.right * newDetectionSize.x - transform.up * newDetectionSize.z + transform.up / 2;
        ExtDebug.DrawBoxCastBox(sliceWith.position + offset, newDetectionSize, sliceWith.forward, sliceWith.rotation, distance, Color.magenta);
    }

    public void Slice(Collider collider)
    {
        if (!canSlice) return;
        if (!canSliceAnimation) return;

        if (IsAlreadySliced(collider)) return;

        if (collider.gameObject.layer == LayerMask.NameToLayer("Targets"))
        {
            // Slice the gameObject inside the trigger
            GameObject meshHolder = KatanaSlice.Cut(collider.transform, sliceWith, true);
            if (meshHolder == null) return; // the slice missed
            dontSliceAgain.Add(meshHolder.transform);
            onSlice?.Invoke();
            Transform transform = Instantiate(debugSlice);
            transform.position = sliceWith.position;
            transform.rotation = sliceWith.rotation;
            //Debug.Break();
            if (collider.gameObject.TryGetComponent(out Destructable destructable))
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
            if (dontSliceAgain != null)
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
        //Slice(collider);
    }
}

public static class ExtDebug
{
    //Draws just the box at where it is currently hitting.
    public static void DrawBoxCastOnHit(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Vector3 direction, float hitInfoDistance, Color color)
    {
        origin = CastCenterOnCollision(origin, direction, hitInfoDistance);
        DrawBox(origin, halfExtents, orientation, color);
    }

    //Draws the full box from start of cast to its end distance. Can also pass in hitInfoDistance instead of full distance
    public static void DrawBoxCastBox(Vector3 origin, Vector3 halfExtents, Vector3 direction, Quaternion orientation, float distance, Color color)
    {
        direction.Normalize();
        Box bottomBox = new Box(origin, halfExtents, orientation);
        Box topBox = new Box(origin + (direction * distance), halfExtents, orientation);

        Debug.DrawLine(bottomBox.backBottomLeft, topBox.backBottomLeft, color);
        Debug.DrawLine(bottomBox.backBottomRight, topBox.backBottomRight, color);
        Debug.DrawLine(bottomBox.backTopLeft, topBox.backTopLeft, color);
        Debug.DrawLine(bottomBox.backTopRight, topBox.backTopRight, color);
        Debug.DrawLine(bottomBox.frontTopLeft, topBox.frontTopLeft, color);
        Debug.DrawLine(bottomBox.frontTopRight, topBox.frontTopRight, color);
        Debug.DrawLine(bottomBox.frontBottomLeft, topBox.frontBottomLeft, color);
        Debug.DrawLine(bottomBox.frontBottomRight, topBox.frontBottomRight, color);

        DrawBox(bottomBox, color);
        DrawBox(topBox, color);
    }

    public static void DrawBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Color color)
    {
        DrawBox(new Box(origin, halfExtents, orientation), color);
    }
    public static void DrawBox(Box box, Color color)
    {
        Debug.DrawLine(box.frontTopLeft, box.frontTopRight, color);
        Debug.DrawLine(box.frontTopRight, box.frontBottomRight, color);
        Debug.DrawLine(box.frontBottomRight, box.frontBottomLeft, color);
        Debug.DrawLine(box.frontBottomLeft, box.frontTopLeft, color);

        Debug.DrawLine(box.backTopLeft, box.backTopRight, color);
        Debug.DrawLine(box.backTopRight, box.backBottomRight, color);
        Debug.DrawLine(box.backBottomRight, box.backBottomLeft, color);
        Debug.DrawLine(box.backBottomLeft, box.backTopLeft, color);

        Debug.DrawLine(box.frontTopLeft, box.backTopLeft, color);
        Debug.DrawLine(box.frontTopRight, box.backTopRight, color);
        Debug.DrawLine(box.frontBottomRight, box.backBottomRight, color);
        Debug.DrawLine(box.frontBottomLeft, box.backBottomLeft, color);
    }

    public struct Box
    {
        public Vector3 localFrontTopLeft { get; private set; }
        public Vector3 localFrontTopRight { get; private set; }
        public Vector3 localFrontBottomLeft { get; private set; }
        public Vector3 localFrontBottomRight { get; private set; }
        public Vector3 localBackTopLeft { get { return -localFrontBottomRight; } }
        public Vector3 localBackTopRight { get { return -localFrontBottomLeft; } }
        public Vector3 localBackBottomLeft { get { return -localFrontTopRight; } }
        public Vector3 localBackBottomRight { get { return -localFrontTopLeft; } }

        public Vector3 frontTopLeft { get { return localFrontTopLeft + origin; } }
        public Vector3 frontTopRight { get { return localFrontTopRight + origin; } }
        public Vector3 frontBottomLeft { get { return localFrontBottomLeft + origin; } }
        public Vector3 frontBottomRight { get { return localFrontBottomRight + origin; } }
        public Vector3 backTopLeft { get { return localBackTopLeft + origin; } }
        public Vector3 backTopRight { get { return localBackTopRight + origin; } }
        public Vector3 backBottomLeft { get { return localBackBottomLeft + origin; } }
        public Vector3 backBottomRight { get { return localBackBottomRight + origin; } }

        public Vector3 origin { get; private set; }

        public Box(Vector3 origin, Vector3 halfExtents, Quaternion orientation) : this(origin, halfExtents)
        {
            Rotate(orientation);
        }
        public Box(Vector3 origin, Vector3 halfExtents)
        {
            this.localFrontTopLeft = new Vector3(-halfExtents.x, halfExtents.y, -halfExtents.z);
            this.localFrontTopRight = new Vector3(halfExtents.x, halfExtents.y, -halfExtents.z);
            this.localFrontBottomLeft = new Vector3(-halfExtents.x, -halfExtents.y, -halfExtents.z);
            this.localFrontBottomRight = new Vector3(halfExtents.x, -halfExtents.y, -halfExtents.z);

            this.origin = origin;
        }


        public void Rotate(Quaternion orientation)
        {
            localFrontTopLeft = RotatePointAroundPivot(localFrontTopLeft, Vector3.zero, orientation);
            localFrontTopRight = RotatePointAroundPivot(localFrontTopRight, Vector3.zero, orientation);
            localFrontBottomLeft = RotatePointAroundPivot(localFrontBottomLeft, Vector3.zero, orientation);
            localFrontBottomRight = RotatePointAroundPivot(localFrontBottomRight, Vector3.zero, orientation);
        }
    }

    //This should work for all cast types
    static Vector3 CastCenterOnCollision(Vector3 origin, Vector3 direction, float hitInfoDistance)
    {
        return origin + (direction.normalized * hitInfoDistance);
    }

    static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
    {
        Vector3 direction = point - pivot;
        return pivot + rotation * direction;
    }
}
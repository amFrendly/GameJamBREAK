using System;
using UnityEngine;

public class Destructable : MonoBehaviour
{

    Slicer slicer;
    public delegate void Destructed();
    public Destructed onDestruct;

    // Start is called before the first frame update
    void Start()
    {
        slicer= GetComponent<Slicer>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Destruct(Vector3 position, float radius, float force)
    {
        //gameObject.SetActive(false);
       // slicer.split = true;
        slicer.SliceWithForce(position, radius, force);
        onDestruct?.Invoke();
    }

    public void Sliced()
    {
        onDestruct?.Invoke();
    }
}

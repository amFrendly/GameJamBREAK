using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Rotate : MonoBehaviour
{

    [SerializeField] float speed;
    [SerializeField] Vector3 axis;

    private void Start()
    {
        transform.Rotate(axis, speed);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(axis, speed);
    }
}

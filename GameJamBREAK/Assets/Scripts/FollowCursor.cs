using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowCursor : MonoBehaviour
{
    [SerializeField] private Image image;
    Vector3 offset;
    void Start()
    {
        offset = new Vector3(-image.sprite.bounds.size.x/4, -image.sprite.bounds.size.y / 4);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

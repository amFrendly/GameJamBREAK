using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float speed = 100;
    [SerializeField] private float force = 50;
    [SerializeField] private float radius = 5;
    [SerializeField] private float timeToLive = 3;
    [SerializeField] private Explode explode;
    [SerializeField] private GameObject explosionPrefab;
    private Rigidbody rb;
    private bool hasExploded;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, timeToLive);

    }
    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.TryGetComponent(out Destructable destructable))
        //{
        //    destructable.Destruct(Vector3.zero, 0, 0);
        //}

        if (hasExploded) return;

        hasExploded = true;
        explode.DoExplode(collision.contacts[0].point, radius, force);
        Instantiate(explosionPrefab, transform.position,Quaternion.identity);
        gameObject.SetActive(false);
    }
    void FixedUpdate()
    {
        rb.AddForce(transform.forward * force);
        if (rb.velocity.sqrMagnitude > speed * speed)
        {
            rb.velocity = transform.forward * speed;
        }


    }
}

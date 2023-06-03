using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KillScript : MonoBehaviour
{
    [SerializeField]
    LayerMask killLayer;

    public void Kill()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((killLayer & (1 << collision.gameObject.layer)) != 0)
        {
            Kill();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((killLayer & (1 << other.gameObject.layer)) != 0)
        {
            Kill();
        }
    }
}

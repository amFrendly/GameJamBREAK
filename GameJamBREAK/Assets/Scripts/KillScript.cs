using UnityEngine;
using UnityEngine.SceneManagement;

public class KillScript : MonoBehaviour
{
    [SerializeField]
    LayerMask killLayer;

    public bool Invincible { get; set; }

    public void Kill()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Invincible) return;

        if ((killLayer & (1 << collision.gameObject.layer)) != 0)
        {
            Kill();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Invincible) return;

        if ((killLayer & (1 << other.gameObject.layer)) != 0)
        {
            Kill();
        }
    }
}

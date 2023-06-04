using UnityEngine;

public class TurnTowardsPlayer : MonoBehaviour
{

    [SerializeField] private Transform playerTransform;

    Vector3 up;

    // Start is called before the first frame update
    void Start()
    {
        if (playerTransform == null)
        {
            enabled = false;
        }
        else
        {
            up = transform.up;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(playerTransform.position);
        Vector3 toPlayer = playerTransform.position - transform.position;
        Vector3 direction = Vector3.ProjectOnPlane(toPlayer, up);
        transform.rotation = Quaternion.LookRotation(direction, up);
    }
}

using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Explode", fileName = "NewExplode")]
public class Explode : ScriptableObject
{
    Collider[] colliders = new Collider[300];
    //[SerializeField]private float radius;
    //[SerializeField]public float force;
    public LayerMask layerMask;
    public Rigidbody playerBody;

    [SerializeField] LayerMask playerLayer;


    public void DoExplode(Vector3 position, float radius, float force)
    {
        int hits = Physics.OverlapSphereNonAlloc(position, radius, colliders, layerMask, QueryTriggerInteraction.Ignore);
        for (int i = 0; i < hits; i++)
        {
            Rigidbody rigidbody = colliders[i].attachedRigidbody;
            if(rigidbody != null)
            {
                if ((playerLayer & (1 << colliders[i].gameObject.layer)) != 0)
                {
                    //rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z)/2;
                    if (rigidbody.velocity.y < 0) rigidbody.AddForce(new Vector3(0, -rigidbody.velocity.y - Physics.gravity.y * Time.deltaTime, 0), ForceMode.VelocityChange);
                }
                rigidbody.AddExplosionForce(force, position, radius);
                
            }
            else if(colliders[i].TryGetComponent(out Destructable destructable))
            {
                destructable.Destruct(position, radius, force);
            }


        }
    }

}

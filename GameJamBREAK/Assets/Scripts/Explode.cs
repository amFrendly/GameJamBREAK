using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Explode", fileName = "NewExplode")]
public class Explode : ScriptableObject
{
    Collider[] colliders = new Collider[30];
    //[SerializeField]private float radius;
    //[SerializeField]public float force;
    LayerMask layerMask;


    public void DoExplode(Vector3 position, float radius, float force)
    {
        int hits = Physics.OverlapSphereNonAlloc(position, radius, colliders, layerMask, QueryTriggerInteraction.Ignore);
        for (int i = 0; i < hits; i++)
        {
            Rigidbody rigidbody = colliders[i].attachedRigidbody;
            if(rigidbody != null)
            {
                rigidbody.AddExplosionForce(force, position, radius);
            }
            else if(colliders[i].TryGetComponent(out Destructable destructable))
            {
                destructable.Destruct(position, radius, force);
            }


        }
    }

}

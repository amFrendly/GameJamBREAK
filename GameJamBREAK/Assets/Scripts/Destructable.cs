using UnityEngine;

public class Destructable : MonoBehaviour
{
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Destruct(Vector3 position, float radius, float force)
    {
        gameObject.SetActive(false);
    }
}

using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private Transform rocketSpawn;
    [SerializeField] private float fireRate = 0.5f;
    float fireCounter;

    void Start()
    {
        fireCounter = fireRate;
    }

    
    void Update()
    {
        fireCounter += Time.deltaTime;
        if ((Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Mouse0)) && fireCounter >= fireRate)
        {
            fireCounter = 0;
            var rocket = Instantiate(rocketPrefab, rocketSpawn.position, rocketSpawn.rotation);
        }
    }
}

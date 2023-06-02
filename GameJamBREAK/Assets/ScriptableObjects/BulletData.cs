using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletData", menuName = "ScriptableObject/BulletData")]
public class BulletData : ScriptableObject
{
    public float timeToLive;
    public float speed;
    public float damage;
    public float gravity;

    public float ImpulseFromSpeed
    {
        get { return speed; }
    }
}

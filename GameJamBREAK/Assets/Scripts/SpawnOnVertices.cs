using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.FilePathAttribute;
using System;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpawnOnVertices : MonoBehaviour
{
    [SerializeField] public Mesh mesh;
    [SerializeField] public GameObject gameObject;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        for(int i = 0; i < mesh.vertices.Length; i++)
        {
            Gizmos.DrawSphere(mesh.vertices[i].Multiply(transform.localScale).Rotate(transform.rotation.eulerAngles) + transform.position, transform.localScale.magnitude / 100f);
        }
    }
}

[CustomEditor(typeof(SpawnOnVertices))]
public class SpawnOnVerticesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SpawnOnVertices spawnOnVertices = (SpawnOnVertices)target;

        if(GUILayout.Button("Create"))
        {

            List<Vector3> verticies = spawnOnVertices.mesh.vertices.ToList();
            for(int i = 0; i < verticies.Count; i++)
            {
                for (int k = 0; k < verticies.Count; k++)
                {
                    if (verticies[k] == verticies[i] && i != k)
                    {
                        verticies.RemoveAt(k);
                    }
                }
            }

            for (int i = 0; i < verticies.Count; i++)
            {
                Vector3 position = verticies[i].Multiply(spawnOnVertices.transform.localScale).Rotate(spawnOnVertices.transform.rotation.eulerAngles) + spawnOnVertices.transform.position;
                GameObject.Instantiate(spawnOnVertices.gameObject).transform.position = position;
            }
        }
        base.OnInspectorGUI();
    }
}

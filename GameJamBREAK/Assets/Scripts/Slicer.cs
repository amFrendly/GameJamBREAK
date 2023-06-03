using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Slicer : MonoBehaviour
{
    [SerializeField]
    public bool flatShade;

    [SerializeField]
    public int splitAmount;

    [SerializeField]
    public bool split;

    private bool didSplit;

    private void Start()
    {
        didSplit = false;
    }

    private void Update()
    {
        if (split && !didSplit)
        {
            Slice(transform, flatShade, splitAmount);
            didSplit = true;
        }
    }

    public void SliceWithForce(Vector3 explosionPosition, float radius, float force)
    {
        if (didSplit) return;
        didSplit = true;
        Transform sliceMe = transform;
        Bounds bounds = GetBounds(sliceMe);

        for (int i = 0; i < splitAmount; i++)
        {
            MeshFilter[] meshFilters = sliceMe.transform.GetComponentsInChildren<MeshFilter>();
            GameObject meshHolder = new GameObject("Mesh Holder");
            meshHolder.transform.position = sliceMe.position;


            Vector3 rotation = Random.rotation.eulerAngles;
            Debug.Log(rotation);
            Vector3 position = GetRandomPositionWithin(bounds) - sliceMe.position;

            Plane plane = new Plane(rotation, position);

            for (int k = 0; k < meshFilters.Length; k++)
            {
                Slice(meshFilters[k], plane, meshHolder.transform, flatShade);
            }

            Destroy(sliceMe.gameObject);
            sliceMe = meshHolder.transform;
        }
        //StartCoroutine(Delay(sliceMe, force, explosionPosition, radius));
        for (int i = 0; i < sliceMe.childCount; i++)
        {
            sliceMe.GetChild(i).GetComponent<Rigidbody>().AddExplosionForce(force, explosionPosition + Vector3.down, radius + 1.4f);
        }
    }

    IEnumerator Delay(Transform sliceMe, float force, Vector3 explosionPosition, float radius)
    {
        yield return new WaitForFixedUpdate();
        int length = sliceMe.childCount;
        for (int i = 0; i < length; i++)
        {
            sliceMe.GetChild(i).GetComponent<Rigidbody>().AddExplosionForce(force, explosionPosition+Vector3.down, radius+1.4f);
        }
    }

    public void Slice(Transform sliceMe, bool flatShade, int amount)
    {
        Bounds bounds = GetBounds(sliceMe);

        for (int i = 0; i < amount; i++)
        {
            MeshFilter[] meshFilters = sliceMe.transform.GetComponentsInChildren<MeshFilter>();
            GameObject meshHolder = new GameObject("Mesh Holder");
            meshHolder.transform.position = sliceMe.position;


            Vector3 rotation = Random.rotation.eulerAngles;
            Debug.Log(rotation);
            Vector3 position = GetRandomPositionWithin(bounds) - sliceMe.position;

            Plane plane = new Plane(rotation, position);

            for (int k = 0; k < meshFilters.Length; k++)
            {
                Slice(meshFilters[k], plane, meshHolder.transform, flatShade);
            }

            Destroy(sliceMe.gameObject);
            sliceMe = meshHolder.transform;
        }
    }

    private Bounds GetBounds(Transform transform)
    {
        MeshRenderer[] renderers = transform.GetComponentsInChildren<MeshRenderer>();
        if (renderers.Length == 0) return new Bounds();

        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length - 1; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }
        return new Bounds(bounds.center, bounds.size);
    }
    private Vector3 GetRandomPositionWithin(Vector3 center, Vector3 halfSize)
    {
        float x = Random.Range(-halfSize.x, halfSize.x);
        float y = Random.Range(-halfSize.y, halfSize.y);
        float z = Random.Range(-halfSize.z, halfSize.z);

        return new Vector3(x, y, z) + center;
    }
    private Vector3 GetRandomPositionWithin(Bounds bounds)
    {
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        float z = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(x, y, z) + bounds.center;
    }

    public void Slice(MeshFilter sliceMe, Plane plane, Transform bucketHolder, bool flatShade)
    {
        GameObject top;
        GameObject bottom;

        Mesh splitMesh = sliceMe.mesh;
        MeshInformation split = new MeshInformation(splitMesh.vertices, splitMesh.triangles, splitMesh.normals, splitMesh.uv);
        SplitMesh(split, sliceMe.transform, plane, out MeshInformation negative, out MeshInformation positive);

        Material material = sliceMe.GetComponent<MeshRenderer>().material;

        if (flatShade)
        {
            positive.FlatShade();
            negative.FlatShade();
        }

        if (positive.triangles.Count > 0)
        {
            top = MakeGameObject(sliceMe.gameObject, bucketHolder, material, positive);
        }
        if (negative.triangles.Count > 0)
        {
            bottom = MakeGameObject(sliceMe.gameObject, bucketHolder, material, negative);
        }
    }

    private GameObject MakeGameObject(GameObject origin, Transform parent, Material material, MeshInformation meshInformation)
    {
        GameObject gameObject = new GameObject();
        gameObject.transform.parent = parent;
        gameObject.transform.position = origin.transform.position;
        gameObject.transform.localScale = origin.transform.localScale;
        Mesh mesh = new Mesh();
        mesh.Clear();
        mesh.vertices = meshInformation.vertices.ToArray();
        mesh.triangles = meshInformation.triangles.ToArray();
        mesh.normals = meshInformation.normals.ToArray();
        mesh.uv = meshInformation.uvs.ToArray();
        mesh.RecalculateNormals();

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = material;
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        meshCollider.convex = true;
        Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();

        return gameObject;
    }


    private void SplitMesh(MeshInformation split, Transform transform, Plane plane, out MeshInformation negative, out MeshInformation positive)
    {
        Vector3 scale = transform.localScale;
        Vector3 position = transform.position;
        Vector3 rotation = transform.rotation.eulerAngles;

        positive = new MeshInformation();
        negative = new MeshInformation();

        List<Vector3> edgePoints = new List<Vector3>();

        for (int i = 0; i < split.triangles.Count; i += 3)
        {
            int index1 = split.triangles[i];
            Vector3 vertex1 = split.vertices[index1].Multiply(scale).Rotate(rotation) + position;
            Vector3 normal1 = split.vertices[index1];
            Vector2 uv1 = split.uvs[index1];
            MeshSide vertex1Side = GetSide(plane, vertex1);

            int index2 = split.triangles[i + 1];
            Vector3 vertex2 = split.vertices[index2].Multiply(scale).Rotate(rotation) + position;
            Vector3 normal2 = split.normals[index2];
            Vector2 uv2 = split.uvs[index2];
            MeshSide vertex2Side = GetSide(plane, vertex2);

            int index3 = split.triangles[i + 2];
            Vector3 vertex3 = split.vertices[index3].Multiply(scale).Rotate(rotation) + position;
            Vector3 normal3 = split.normals[index3];
            Vector2 uv3 = split.uvs[index3];
            MeshSide vertex3Side = GetSide(plane, vertex3);


            // All points are on the same side
            if (vertex1Side == vertex2Side && vertex2Side == vertex3Side)
            {

                if (vertex1Side == MeshSide.positive)
                {
                    positive.AddTriangleNormalWorldSpace(transform, vertex1, normal1, uv1, vertex2, normal2, uv2, vertex3, normal3, uv3, false);
                }
                else
                {
                    negative.AddTriangleNormalWorldSpace(transform, vertex1, normal1, uv1, vertex2, normal2, uv2, vertex3, normal3, uv3, false);
                }
            }
            // all the other combinations "yay"
            else
            {
                if (vertex1Side == MeshSide.positive && vertex2Side == MeshSide.negative && vertex3Side == MeshSide.negative)
                {
                    Vector3 newVertex2 = PlaneIntersectionPointAndUv(vertex1, vertex2, plane, uv1, uv2, out Vector2 newUv2);
                    Vector3 newVertex3 = PlaneIntersectionPointAndUv(vertex1, vertex3, plane, uv1, uv3, out Vector2 newUv3);

                    positive.AddTriangleNormalWorldSpace(transform, vertex1, null, uv1, newVertex2, null, newUv2, newVertex3, null, newUv3, false);
                    negative.AddTriangleNormalWorldSpace(transform, vertex3, null, uv3, newVertex3, null, newUv3, newVertex2, null, newUv2, false);
                    negative.AddTriangleNormalWorldSpace(transform, vertex3, null, uv3, newVertex2, null, newUv2, vertex2, null, uv2, false);

                    edgePoints.Add(newVertex2);
                    edgePoints.Add(newVertex3);
                }
                else if (vertex1Side == MeshSide.negative && vertex2Side == MeshSide.positive && vertex3Side == MeshSide.negative)
                {
                    Vector3 newVertex1 = PlaneIntersectionPointAndUv(vertex2, vertex1, plane, uv2, uv1, out Vector2 newUv1);
                    Vector3 newVertex3 = PlaneIntersectionPointAndUv(vertex2, vertex3, plane, uv2, uv3, out Vector2 newUv3);

                    positive.AddTriangleNormalWorldSpace(transform, newVertex1, null, newUv1, vertex2, null, uv2, newVertex3, null, newUv3, false);
                    negative.AddTriangleNormalWorldSpace(transform, vertex1, null, uv1, newVertex1, null, newUv1, newVertex3, null, newUv3, false);
                    negative.AddTriangleNormalWorldSpace(transform, vertex1, null, uv1, newVertex3, null, newUv3, vertex3, null, uv3, false);

                    edgePoints.Add(newVertex1);
                    edgePoints.Add(newVertex3);
                }
                else if (vertex1Side == MeshSide.negative && vertex2Side == MeshSide.negative && vertex3Side == MeshSide.positive)
                {
                    Vector3 newVertex1 = PlaneIntersectionPointAndUv(vertex3, vertex1, plane, uv3, uv1, out Vector2 newUv1);
                    Vector3 newVertex2 = PlaneIntersectionPointAndUv(vertex3, vertex2, plane, uv3, uv2, out Vector2 newUv2);

                    positive.AddTriangleNormalWorldSpace(transform, newVertex1, null, newUv1, newVertex2, null, newUv2, vertex3, null, uv3, false);
                    negative.AddTriangleNormalWorldSpace(transform, vertex1, null, uv1, newVertex2, null, newUv2, newVertex1, null, newUv1, false);
                    negative.AddTriangleNormalWorldSpace(transform, vertex1, null, uv1, vertex2, null, uv2, newVertex2, null, newUv2, false);

                    edgePoints.Add(newVertex1);
                    edgePoints.Add(newVertex2);
                }
                else if (vertex1Side == MeshSide.negative && vertex2Side == MeshSide.positive && vertex3Side == MeshSide.positive)
                {
                    Vector3 newVertex2 = PlaneIntersectionPointAndUv(vertex1, vertex2, plane, uv1, uv2, out Vector2 newUv2);
                    Vector3 newVertex3 = PlaneIntersectionPointAndUv(vertex1, vertex3, plane, uv1, uv3, out Vector2 newUv3);

                    negative.AddTriangleNormalWorldSpace(transform, vertex1, null, uv1, newVertex2, null, newUv2, newVertex3, null, newUv3, false);
                    positive.AddTriangleNormalWorldSpace(transform, vertex3, null, uv3, newVertex3, null, newUv3, newVertex2, null, newUv2, false);
                    positive.AddTriangleNormalWorldSpace(transform, vertex3, null, uv3, newVertex2, null, newUv2, vertex2, null, uv2, false);

                    edgePoints.Add(newVertex2);
                    edgePoints.Add(newVertex3);
                }
                else if (vertex1Side == MeshSide.positive && vertex2Side == MeshSide.negative && vertex3Side == MeshSide.positive)
                {
                    Vector3 newVertex1 = PlaneIntersectionPointAndUv(vertex2, vertex1, plane, uv2, uv1, out Vector2 newUv1);
                    Vector3 newVertex3 = PlaneIntersectionPointAndUv(vertex2, vertex3, plane, uv2, uv3, out Vector2 newUv3);

                    negative.AddTriangleNormalWorldSpace(transform, newVertex1, null, newUv1, vertex2, null, uv2, newVertex3, null, newUv3, false);
                    positive.AddTriangleNormalWorldSpace(transform, vertex1, null, uv1, newVertex1, null, newUv1, newVertex3, null, newUv3, false);
                    positive.AddTriangleNormalWorldSpace(transform, vertex1, null, uv1, newVertex3, null, newUv3, vertex3, null, uv3, false);

                    edgePoints.Add(newVertex1);
                    edgePoints.Add(newVertex3);
                }
                else if (vertex1Side == MeshSide.positive && vertex2Side == MeshSide.positive && vertex3Side == MeshSide.negative)
                {
                    Vector3 newVertex1 = PlaneIntersectionPointAndUv(vertex3, vertex1, plane, uv3, uv1, out Vector2 newUv1);
                    Vector3 newVertex2 = PlaneIntersectionPointAndUv(vertex3, vertex2, plane, uv3, uv2, out Vector2 newUv2);

                    negative.AddTriangleNormalWorldSpace(transform, newVertex1, null, newUv1, newVertex2, null, newUv2, vertex3, null, uv3, false);
                    positive.AddTriangleNormalWorldSpace(transform, vertex1, null, uv1, newVertex2, null, newUv2, newVertex1, null, newUv1, false);
                    positive.AddTriangleNormalWorldSpace(transform, vertex1, null, uv1, vertex2, null, uv2, newVertex2, null, newUv2, false);

                    edgePoints.Add(newVertex1);
                    edgePoints.Add(newVertex2);
                }
            }
        }

        FillEdge(edgePoints, plane, transform, ref positive, ref negative);
    }
    private void FillEdge(List<Vector3> edgePoints, Plane plane, Transform transform, ref MeshInformation positive, ref MeshInformation negative)
    {
        Vector3 middle = GetMiddle(edgePoints);
        for (int i = 0; i < edgePoints.Count; i += 2)
        {
            Vector3 firstVertex;
            Vector3 secondVertex;

            firstVertex = edgePoints[i];
            secondVertex = edgePoints[i + 1];

            Vector3 normal3 = ComputeNormal(middle, secondVertex, firstVertex);
            normal3.Normalize();

            var direction = Vector3.Dot(normal3, plane.normal);

            if (direction > 0)
            {
                positive.AddTriangleNormalWorldSpace(transform, middle, -normal3, Vector2.zero, firstVertex, -normal3, Vector2.zero, secondVertex, -normal3, Vector2.zero, true);
                negative.AddTriangleNormalWorldSpace(transform, middle, normal3, Vector2.zero, secondVertex, normal3, Vector2.zero, firstVertex, normal3, Vector2.zero, true);
            }
            else
            {
                positive.AddTriangleNormalWorldSpace(transform, middle, normal3, Vector2.zero, secondVertex, normal3, Vector2.zero, firstVertex, normal3, Vector2.zero, true);
                negative.AddTriangleNormalWorldSpace(transform, middle, -normal3, Vector2.zero, firstVertex, -normal3, Vector2.zero, secondVertex, -normal3, Vector2.zero, true);
            }
        }
    }
    private Vector3 ComputeNormal(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
    {
        Vector3 side1 = vertex2 - vertex1;
        Vector3 side2 = vertex3 - vertex1;

        Vector3 normal = Vector3.Cross(side1, side2);

        return normal;
    }
    private Vector3 GetMiddle(List<Vector3> points)
    {
        Vector3 sum = new Vector3();
        for (int i = 0; i < points.Count; i++)
        {
            sum += points[i];
        }
        return sum / points.Count;
    }
    private MeshSide GetSide(Plane plane, Vector3 point)
    {
        if (plane.GetSide(point))
        {
            return MeshSide.positive;
        }
        return MeshSide.negative;
    }
    private Vector3 PlaneIntersectionPointAndUv(Vector3 start, Vector3 end, Plane plane, Vector2 startUv, Vector2 endUv, out Vector2 uv)
    {
        Vector3 direction = end - start;
        Ray ray = new Ray(start, direction);
        plane.Raycast(ray, out float distance);
        float totalDistance = Vector3.Distance(start, end);
        float t = distance / totalDistance;
        uv = Vector2.Lerp(startUv, endUv, t);
        Vector3 intersect = start + Vector3.Normalize(direction) * distance;
        return intersect;
    }

    class MeshInformation
    {
        public List<Vector3> vertices;
        public List<int> triangles;
        public List<Vector3> normals;
        public List<Vector2> uvs;
        public MeshInformation(Vector3[] vertices, int[] triangles, Vector3[] normals, Vector2[] uvs)
        {
            this.vertices = vertices.ToList();
            this.triangles = triangles.ToList();
            this.normals = normals.ToList();
            this.uvs = uvs.ToList();
        }
        public MeshInformation(List<Vector3> vertices, List<int> triangles, List<Vector3> normals, List<Vector2> uvs)
        {
            this.vertices = vertices;
            this.triangles = triangles;
            this.normals = normals;
            this.uvs = uvs;
        }
        public MeshInformation()
        {
            vertices = new List<Vector3>();
            triangles = new List<int>();
            uvs = new List<Vector2>();
            normals = new List<Vector3>();
        }

        public void AddTriangleNormalWorldSpace(Transform transform, Vector3 vertex1, Vector3? normal1, Vector2 uv1, Vector3 vertex2, Vector3? normal2, Vector2 uv2, Vector3 vertex3, Vector3? normal3, Vector2 uv3, bool newTriangle)
        {
            Vector3 position = transform.position;
            Vector3 scale = transform.localScale;
            Vector3 rotation = transform.rotation.eulerAngles;

            vertex1 = (vertex1 - position).Rotate(Vector2.zero).Divide(scale);
            vertex2 = (vertex2 - position).Rotate(Vector2.zero).Divide(scale);
            vertex3 = (vertex3 - position).Rotate(Vector2.zero).Divide(scale);

            AddTriangleNormal(vertex1, normal1, uv1, vertex2, normal2, uv2, vertex3, normal3, uv3, newTriangle);
        }
        public void AddTriangleNormal(Vector3 vertex1, Vector3? normal1, Vector2 uv1, Vector3 vertex2, Vector3? normal2, Vector2 uv2, Vector3 vertex3, Vector3? normal3, Vector2 uv3, bool newTriangle)
        {
            if (normal1 == null) normal1 = ComputeNormal(vertex1, vertex2, vertex3);
            if (normal2 == null) normal2 = ComputeNormal(vertex2, vertex3, vertex1);
            if (normal3 == null) normal3 = ComputeNormal(vertex3, vertex1, vertex2);

            int index1 = newTriangle ? -1 : vertices.IndexOf(vertex1);
            int index2 = newTriangle ? -1 : vertices.IndexOf(vertex2);
            int index3 = newTriangle ? -1 : vertices.IndexOf(vertex3);

            AddVertNormalUv(vertex1, (Vector3)normal1, uv1, index1, newTriangle);
            AddVertNormalUv(vertex2, (Vector3)normal2, uv2, index2, newTriangle);
            AddVertNormalUv(vertex3, (Vector3)normal3, uv3, index3, newTriangle);
        }
        private void AddVertNormalUv(Vector3 vertex, Vector3 normal, Vector2 uv, int index, bool newTriangle)
        {
            if (index < 0) // There is no vertex added on the same position already, add an entierly new vertex
            {
                vertices.Add(vertex);
                normals.Add(normal);
                uvs.Add(uv);
                triangles.Add(vertices.Count - 1);
            }
            else // Vertex has alreay been added, now add a reference to the triangles
            {
                triangles.Add(index);
            }
        }

        /// <summary>
        /// remakes the meshInformation but every triangle gets its very own vertex, normal and uv that makes the mesh way more highpoly and looks more lowPoly
        /// </summary>
        public void FlatShade()
        {
            int[] triangles = new int[this.triangles.Count];
            Vector3[] vertices = new Vector3[this.triangles.Count];
            Vector3[] normals = new Vector3[this.triangles.Count];
            Vector2[] uvs = new Vector2[this.triangles.Count];
            for (int i = 0; i < this.triangles.Count; i++)
            {
                int triangleIndex = this.triangles[i];
                vertices[i] = this.vertices[triangleIndex];
                normals[i] = this.normals[triangleIndex];
                uvs[i] = this.uvs[triangleIndex];
                triangles[i] = i;
            }

            this.vertices = vertices.ToList();
            this.normals = normals.ToList();
            this.uvs = uvs.ToList();
            this.triangles = triangles.ToList();
        }

        public Bounds GetBounds(Transform transform)
        {
            Bounds bounds = new Bounds();
            for (int i = 0; i < vertices.Count; i++)
            {
                bounds.Encapsulate(vertices[i]);
            }

            bounds.center += transform.position;
            return bounds;
        }

        public void NormalizeNormals()
        {
            for (int i = 0; i < normals.Count; i++)
            {
                normals[i] = normals[i].normalized;
            }
        }
        private Vector3 ComputeNormal(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
        {
            Vector3 side1 = vertex2 - vertex1;
            Vector3 side2 = vertex3 - vertex1;

            Vector3 normal = Vector3.Cross(side1, side2);
            return normal;
        }
    }

    enum MeshSide
    {
        negative = 0,
        positive = 1
    }
}

public static class Vector3Exstend
{
    static public Vector3 Multiply(this Vector3 first, Vector3 second)
    {
        return new Vector3(first.x * second.x, first.y * second.y, first.z * second.z);
    }

    static public Vector3 Divide(this Vector3 first, Vector3 second)
    {
        return new Vector3(first.x / second.x, first.y / second.y, first.z / second.z);
    }

    static public Vector3 Rotate(this Vector3 position, Vector3 rotation)
    {
        Vector3 newPostion = Quaternion.Euler(rotation) * position;
        return new Vector3(newPostion.x, newPostion.y, newPostion.z);
    }
}
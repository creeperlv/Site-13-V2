using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Rendering
{
    public class UVLess : MonoBehaviour
    {
        public MeshFilter MeshFilter;
        public float ScalingFactor;
        float LastScaleFactor;
        Vector3 LastScale;
        Mesh mesh;
        Transform t;
        // Start is called before the first frame update
        void Start()
        {
            mesh = MeshFilter.mesh;
            t = MeshFilter.transform;
        }
        void ApplyUVLess()
        {
            int[] tris = mesh.triangles;

            Vector3[] verts = mesh.vertices;
            Vector2[] uvs = new Vector2[verts.Length];

            for (int index = 0; index < tris.Length; index += 3)
            {
                Vector3 v1 = t.TransformPoint(verts[tris[index]]);
                Vector3 v2 = t.TransformPoint(verts[tris[index + 1]]);
                Vector3 v3 = t.TransformPoint(verts[tris[index + 2]]);

                Vector3 normal = Vector3.Cross(v3 - v1, v2 - v1);

                Quaternion rotation = Quaternion.Inverse(Quaternion.LookRotation(normal));

                uvs[tris[index]] = (Vector2)(rotation * v1) * ScalingFactor;
                uvs[tris[index + 1]] = (Vector2)(rotation * v2) * ScalingFactor;
                uvs[tris[index + 2]] = (Vector2)(rotation * v3) * ScalingFactor;
            }
            mesh.uv = uvs;
            MeshFilter.mesh = mesh;
            LastScale = t.lossyScale;
            LastScaleFactor = ScalingFactor;
        }
        // Update is called once per frame
        void Update()
        {
            if (LastScale == t.lossyScale && LastScaleFactor == ScalingFactor)
            {
                return;
            }
            ApplyUVLess();
        }
    }
}

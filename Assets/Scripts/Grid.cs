using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour {

    // Sizd of the mesh to be generated
    public int xSize, ySize;

    // Set of vectors for the mesh
    private Vector3[] vertices;

    // Reference to the mesh in the mesh filter
    private Mesh mesh;

    private void Awake() {

        // Generate as soon as empty game object awakens but visualize it
        Generate();
    }

    // This will generate the mesh
    private void Generate()
    {
        // Time interval between drawing two gizmos
        WaitForSeconds wait = new WaitForSeconds(0.05f);

        // Create new mesh and assign it to the filters mesh and also keep a reference
        // to ourselves
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";

        // There is always one more vertex than tiles in each dimension
        vertices = new Vector3[(xSize + 1) * (ySize + 1)];

        Vector2[] uv = new Vector2[vertices.Length];

        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);


        // Positioning the vertices
        for (int i = 0, y = 0; y <= ySize; y++) {
            for (int x = 0; x <= xSize; x++, i++) {
                vertices[i] = new Vector3(x, y);

                uv[i] = new Vector2((float)x / xSize, (float)y / ySize);

                tangents[i] = tangent;
            }
        }

        // Give the vertices that were just created to the mesh
        mesh.vertices = vertices;

        mesh.uv = uv;

        mesh.tangents = tangents;

       

        // Each quad has two triangles, and each triangle has three vertices
        int[] triangles = new int[xSize * ySize * 2 * 3];

        // Using loop to create triangles
        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++) {
            for (int x = 0; x < xSize; x++, ti += 6, vi++) {

                // Remember, only the triangles with vertices in the clockwise order are
                // displayed
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
            }
        }
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    // Called by Unity automatically
    private void OnDrawGizmos() {

        // Also invoked in the editor mode when vertices are not there, simply return
        if (vertices == null) {
            return;
        }

        Gizmos.color = Color.black;

        // Gizmos will be drawn into world space directly if the first arg to DrawSphere
        // function is vertices[i] and therefore wouldn't move when the object moves
        for (int i = 0; i < vertices.Length; i++) {
            Gizmos.DrawSphere(transform.TransformPoint(vertices[i]), 0.1f);
        }
    }


   
}

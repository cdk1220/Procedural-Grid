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
        StartCoroutine(Generate());
    }

    // This will generate the mesh
    private IEnumerator Generate()
    {
        // Time interval between drawing two gizmos
        WaitForSeconds wait = new WaitForSeconds(0.05f);

        // Create new mesh and assign it to the filters mesh and also keep a reference
        // to ourselves
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";

        // There is always one more vertex than tiles in each dimension
        vertices = new Vector3[(xSize + 1) * (ySize + 1)];


        // Positioning the vertices
        for (int i = 0, y = 0; y <= ySize; y++) {
            for (int x = 0; x <= xSize; x++, i++) {
                vertices[i] = new Vector3(x, y);

                yield return wait;
            }
        }

        // Give the vertices that were just created to the mesh
        mesh.vertices = vertices;

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

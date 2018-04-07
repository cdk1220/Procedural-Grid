using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour {

    // Sizd of the mesh to be generated
    public int xSize, ySize;

    private Vector3[] vertices;

    private void Awake() {

        // Generate as soon as empty game object awakens
        Generate();
    }

    // This will generate the mesh
    private void Generate()
    {
        // There is always one more vertex than tiles in each dimension
        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
    }

    // Called by Unity automatically
    private void OnDrawGizmos() {

        // Also invoked in the editor mode when vertices are not there, simply return
        if (vertices == null) {
            return;
        }

        Gizmos.color = Color.black;

        for (int i = 0; i < vertices.Length; i++) {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
}

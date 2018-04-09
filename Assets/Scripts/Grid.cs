using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour {

    // Sizd of the mesh to be generated
    public int xSize, zSize;

    // Set of vectors for the mesh
    private Vector3[] vertices;

    // Reference to the mesh in the mesh filter
    private Mesh mesh;

    // Perlin noise related fields
    public float power = 3;
    public float scale = 1;
    public float timeScale = 1;

    private float xOffset;
    private float yOffset;

    private void Awake() {

        // Generate as soon as empty game object awakens but visualize it
        Generate();
    }

    void Update() {
        PerlinNoise();
        xOffset += Time.deltaTime * timeScale;
        yOffset += Time.deltaTime * timeScale;
    }


    // This will generate the mesh
    private void Generate()
    {
        // Create new mesh and assign it to the filters mesh and also keep a reference
        // to ourselves
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";

        // There is always one more vertex than tiles in each dimension
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        // Texture coordinates
        Vector2[] uv = new Vector2[vertices.Length];

        // Tangent stuff
        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);


        // Positioning the vertices
        for (int i = 0, z = 0; z <= zSize; z++) {
            for (int x = 0; x <= xSize; x++, i++) {
                vertices[i] = new Vector3(x, 0f, z);

                uv[i] = new Vector2((float)x / xSize, (float)z / zSize);

                tangents[i] = tangent;
            }
        }

        // Give the vertices that were just created to the mesh
        mesh.vertices = vertices;

        // Setting uv coordinates of the mesh
        mesh.uv = uv;

        // Setting tangents of the mesh
        mesh.tangents = tangents;

       

        // Each quad has two triangles, and each triangle has three vertices
        int[] triangles = new int[xSize * zSize * 2 * 3];

        // Using loop to create triangles
        for (int ti = 0, vi = 0, z = 0; z < zSize; z++, vi++) {
            for (int x = 0; x < xSize; x++, ti += 6, vi++) {

                // Remember, only the triangles with vertices in the clockwise order are
                // displayed
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
            }
        }

        // Set triangles
        mesh.triangles = triangles;

        // Calculate normals
        mesh.RecalculateNormals();

    }


    // Create noise
    private void PerlinNoise() {

        // Get vertices from mesh
        vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; i++) {
            vertices[i].y = CalculateHeight(vertices[i].x, vertices[i].z) * power;
        }

        // Set the vertices after modfying 
        mesh.vertices = vertices;
    }

    // Helper function
    float CalculateHeight(float x, float y) {
        float xCord = x * scale + xOffset;
        float yCord = y * scale + yOffset;

        return Mathf.PerlinNoise(xCord, yCord);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenTest : MonoBehaviour
{

  GameObject meshContainer;
  MeshFilter meshFilter;
  MeshRenderer meshRenderer;

  void Start()
  {

  }

  public void CreateMesh()
  {
    this.meshContainer = GameObject.Find("Mesh Container");
    if (this.meshContainer)
    {
      DestroyImmediate(this.meshContainer);
    }

    this.meshContainer = new GameObject("Mesh Container");
    this.meshFilter = this.meshContainer.AddComponent<MeshFilter>();
    this.meshRenderer = this.meshContainer.AddComponent<MeshRenderer>();
    this.meshRenderer.material = Resources.Load("TestMaterial") as Material;

    /*
      We're mapping a 9-pixel blanket of colors
      To our single square
      The uvs array indicates the 2-dimensionally mapped
      start and end coordinates for our texture to draw on.
      For the single square, we've mapped 9 pixels to 1 square (made of 2 triangles)
      For the large mesh, we've mapped 9 pixels across the full size of the mesh
    */
    Color[] colors = new Color[9];
    colors[0] = Color.green;
    colors[1] = Color.red;
    colors[2] = Color.blue;
    colors[3] = Color.yellow;
    colors[4] = Color.black;
    colors[5] = Color.cyan;
    colors[6] = Color.gray;
    colors[7] = Color.magenta;
    colors[8] = Color.white;

    Texture2D texture = new Texture2D(3, 3);
    texture.filterMode = FilterMode.Point;
    texture.wrapMode = TextureWrapMode.Clamp;
    texture.SetPixels(colors);
    texture.Apply();

    this.meshRenderer.sharedMaterial.mainTexture = texture;

    // this.CreateSingleSquare();
    this.CreateLargeMesh(18);
  }

  /// <summary>Highlight vertices with gizmos</summary>
  void OnDrawGizmos()
  {
    if (!this.meshFilter) return;
    Gizmos.color = Color.red;
    for (int i = 0; i < this.meshFilter.sharedMesh.vertices.Length; i++)
    {
      Gizmos.DrawSphere(this.meshFilter.sharedMesh.vertices[i], 0.025f);
    }
  }

  void CreateSingleSquare()
  {
    /*
      Computer graphics 101
      Graphics are processed by connecting points into triangles
      Each point is defined by a position in world space
      Each triangle is defined by the 3 indices of the points that compose it
      The *order of points in a triangle matters*
      We define the points in clockwise order relative to the viewing perspective
      This allows the engine to cull whatever is "below" the viewing perspective
      We can apply a texture of pixels to the mesh
      In order to apply pixels properly, we "map" the points to apply the texture to
      We can use uvs for this, which is just a ratio indicating the start and end of the mesh to draw on
    */
    Vector3[] vertices = new Vector3[4];
    Vector2[] uvs = new Vector2[4];
    int[] triangles = new int[6];

    vertices[0] = new Vector3(0, 0, 0);
    vertices[1] = new Vector3(1, 0, 0);
    vertices[2] = new Vector3(0, 0, 1);

    vertices[3] = new Vector3(1, 1, 1);

    uvs[0] = new Vector2(0, 0);
    uvs[1] = new Vector2(1, 0);
    uvs[2] = new Vector2(0, 1);
    uvs[3] = new Vector2(1, 1);

    triangles[0] = 0;
    triangles[1] = 2;
    triangles[2] = 1;

    triangles[3] = 2;
    triangles[4] = 3;
    triangles[5] = 1;

    Mesh mesh = new Mesh();
    mesh.vertices = vertices;
    mesh.triangles = triangles;
    mesh.uv = uvs;
    mesh.RecalculateNormals();

    this.meshFilter.sharedMesh = mesh;
  }


  /// <summary>
  /// Create a square mesh
  /// </summary>
  /// <param name="meshSize">width and height of mesh</param>
  void CreateLargeMesh(int meshSize)
  {

    float[,] noiseMap = Noise.GenerateNoiseMap(meshSize, meshSize, 141, 43.06f, 12, 0.343f, 2.8f, new Vector2(0.4f, 3.65f));


    Vector3[] vertices = new Vector3[meshSize * meshSize];
    Vector2[] uvs = new Vector2[meshSize * meshSize];
    int trianglesPerRow = (meshSize - 1) * 2;
    int[] triangles = new int[trianglesPerRow * meshSize * 3];

    int verticesIndex = 0;
    int triangleIndex = 0;
    for (int z = 0; z < meshSize; z++)
    {
      for (int x = 0; x < meshSize; x++)
      {
        // vertices[verticesIndex] = new Vector3(x, Random.Range(0f, 1f), z);
        vertices[verticesIndex] = new Vector3(x, noiseMap[x, z] * 5f, z);
        uvs[verticesIndex] = new Vector2(x / (float)meshSize, z / (float)meshSize);

        // Draw triangles bottom-up, top row of vertices has no triangles to draw
        if (z < meshSize - 1)
        {

          if (x < meshSize - 1)
          {
            /*
              @
              @@
              @@@
              @@@@
              x@@@@
            */
            // Draw triangle right of vertex
            // Don't draw at the rightmost vertex
            triangles[triangleIndex] = verticesIndex;
            triangles[triangleIndex + 1] = verticesIndex + meshSize;
            triangles[triangleIndex + 2] = verticesIndex + 1;
            triangleIndex += 3;
          }

          if (x > 0)
          {
            /*
              @@@@@
               @@@@
                @@@
                 @@
                  x
            */
            // Draw triangle left of vertex
            // Don't draw at leftmost vertex
            triangles[triangleIndex] = verticesIndex;
            triangles[triangleIndex + 1] = verticesIndex + meshSize - 1;
            triangles[triangleIndex + 2] = verticesIndex + meshSize;
            triangleIndex += 3;
          }

        }

        verticesIndex++;
      }
    }

    Mesh mesh = new Mesh();
    mesh.vertices = vertices;
    mesh.triangles = triangles;
    mesh.uv = uvs;

    this.meshFilter.sharedMesh = mesh;
  }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{

  public Mesh GenerateMesh(float[,] heightMap)
  {
    // Assuming heightMap is a square array
    int meshSize = heightMap.GetLength(0);
    Vector3[] vertices = new Vector3[meshSize * meshSize];
    Vector2[] uvs = new Vector2[meshSize * meshSize];
    int[] triangles = new int[(meshSize - 1) * (meshSize - 1) * 6];

    int verticesIndex = 0;
    int triangleIndex = 0;

    for (int y = 0; y < meshSize; y++)
    {
      for (int x = 0; x < meshSize; x++)
      {
        vertices[verticesIndex] = new Vector3(x, heightMap[x, y], y);
        uvs[verticesIndex] = new Vector2(x / (float)meshSize, y / (float)meshSize);

        if (x < meshSize - 1 && y < meshSize - 1)
        {
          int topLeftIndex = y * meshSize;
          triangles[triangleIndex] = topLeftIndex + x;
          triangles[triangleIndex + 1] = topLeftIndex + x + 4;
          triangles[triangleIndex + 2] = topLeftIndex + x + 3;

          triangles[triangleIndex + 3] = topLeftIndex + x;
          triangles[triangleIndex + 4] = topLeftIndex + x + 1;
          triangles[triangleIndex + 5] = topLeftIndex + x + 4;

          triangleIndex += 6;
        }

        verticesIndex++;
      }
    }

    Mesh mesh = new Mesh();
    mesh.vertices = vertices;
    mesh.triangles = triangles;
    mesh.uv = uvs;
    mesh.RecalculateNormals();
    return mesh;
  }
}

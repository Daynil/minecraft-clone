using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{

  public Mesh GenerateMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve)
  {
    // Assuming heightMap is a square array
    int meshSize = heightMap.GetLength(0);

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
        vertices[verticesIndex] = new Vector3(x, heightCurve.Evaluate(heightMap[x, z]) * heightMultiplier, z);
        uvs[verticesIndex] = new Vector2(x / (float)meshSize, z / (float)meshSize);

        // Draw triangles bottom-up, top row of vertices has no triangles to draw
        if (z < meshSize - 1)
        {

          if (x < meshSize - 1)
          {
            /*
                |\
                | \
                |  \
                |___\
                x
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
                ____
                \  |
                 \ |
                  \|
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
    mesh.RecalculateNormals();
    return mesh;
  }
}

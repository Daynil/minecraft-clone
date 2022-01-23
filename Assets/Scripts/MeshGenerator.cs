using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{

  public Mesh GenerateMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve, int levelOfDetail)
  {
    // Assuming heightMap is a square array
    int meshSize = heightMap.GetLength(0);

    int meshSimplificationIncrement = levelOfDetail * 2;
    if (meshSimplificationIncrement == 0) meshSimplificationIncrement = 1;
    int verticesPerLine = (meshSize - 1) / meshSimplificationIncrement + 1;

    Vector3[] vertices = new Vector3[verticesPerLine * verticesPerLine];
    Vector2[] uvs = new Vector2[verticesPerLine * verticesPerLine];
    int trianglesPerRow = (verticesPerLine - 1) * 2;
    int[] triangles = new int[trianglesPerRow * verticesPerLine * 3];

    int verticesIndex = 0;
    int triangleIndex = 0;
    for (int z = 0; z < meshSize; z += meshSimplificationIncrement)
    {
      for (int x = 0; x < meshSize; x += meshSimplificationIncrement)
      {
        vertices[verticesIndex] = new Vector3(x, heightCurve.Evaluate(heightMap[x, z]) * heightMultiplier, z);
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
            triangles[triangleIndex + 1] = verticesIndex + verticesPerLine;
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
            triangles[triangleIndex + 1] = verticesIndex + verticesPerLine - 1;
            triangles[triangleIndex + 2] = verticesIndex + verticesPerLine;
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

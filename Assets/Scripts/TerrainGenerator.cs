using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{

  public enum DrawMode { NoiseMap, ColorMap, Mesh };
  public DrawMode drawMode;

  const int mapChunkSize = 241;
  [Range(0, 6)]
  public int levelOfDetail;
  public float noiseScale;

  public int octaves;
  [Range(0, 1)]
  public float persistance;
  public float lacunarity;

  public int seed;
  public Vector2 offset;

  public float meshHeightMultiplier;
  public AnimationCurve meshHeightCurve;

  public bool autoUpdate;

  public TerrainType[] regions;

  public void GenerateMap()
  {
    float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);
    Color[,] colorMap = new Color[mapChunkSize, mapChunkSize];

    for (int x = 0; x < mapChunkSize; x++)
    {
      for (int y = 0; y < mapChunkSize; y++)
      {
        float currentHeight = noiseMap[x, y];
        for (int i = 0; i < regions.Length; i++)
        {
          TerrainType region = regions[i];
          // Note: assuming regions ordered by lowest to highest in array
          if (currentHeight <= region.maxHeight)
          {
            colorMap[x, y] = region.color;
            break;
          }
        }
      }
    }

    TerrainDisplay display = FindObjectOfType<TerrainDisplay>();
    MeshGenerator meshGen = FindObjectOfType<MeshGenerator>();
    if (drawMode == DrawMode.NoiseMap)
    {
      display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
    }
    else if (drawMode == DrawMode.ColorMap)
    {
      display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap));
    }
    else if (drawMode == DrawMode.Mesh)
    {
      display.DrawMesh(meshGen.GenerateMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColorMap(colorMap));
    }
  }

  public void OnValidate()
  {
    if (lacunarity < 1)
    {
      lacunarity = 1;
    }
    if (octaves < 0)
    {
      octaves = 0;
    }
    if (noiseScale < 0.004)
    {
      noiseScale = 0.004f;
    }
  }
}

[System.Serializable]
public struct TerrainType
{
  public string name;
  public float maxHeight;
  public Color color;
}

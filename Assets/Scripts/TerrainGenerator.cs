using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{

  public enum DrawMode { NoiseMap, ColorMap, Mesh };
  public DrawMode drawMode;

  public int mapSize;
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
    float[,] noiseMap = Noise.GenerateNoiseMap(mapSize, mapSize, seed, noiseScale, octaves, persistance, lacunarity, offset);
    Color[,] colorMap = new Color[mapSize, mapSize];

    for (int x = 0; x < mapSize; x++)
    {
      for (int y = 0; y < mapSize; y++)
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
      display.DrawMesh(meshGen.GenerateMesh(noiseMap, meshHeightMultiplier, meshHeightCurve), TextureGenerator.TextureFromColorMap(colorMap));
    }
  }

  public void OnValidate()
  {
    if (this.mapSize < 1)
    {
      this.mapSize = 1;
    }
    if (this.mapSize < 1)
    {
      this.mapSize = 1;
    }
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

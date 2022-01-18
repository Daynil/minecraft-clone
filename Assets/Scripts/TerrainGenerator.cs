using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{

  public enum DrawMode { NoiseMap, ColorMap };
  public DrawMode drawMode;

  public int mapWidth;
  public int mapHeight;
  public float noiseScale;

  public int octaves;
  [Range(0, 1)]
  public float persistance;
  public float lacunarity;

  public int seed;
  public Vector2 offset;

  public bool autoUpdate;

  public TerrainType[] regions;

  public void GenerateMap()
  {
    float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);
    Color[,] colorMap = new Color[mapWidth, mapHeight];

    for (int x = 0; x < mapWidth; x++)
    {
      for (int y = 0; y < mapHeight; y++)
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
    if (drawMode == DrawMode.NoiseMap)
    {
      display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
    }
    else if (drawMode == DrawMode.ColorMap)
    {
      display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap));
    }

  }

  public void OnValidate()
  {
    if (this.mapWidth < 1)
    {
      this.mapWidth = 1;
    }
    if (this.mapHeight < 1)
    {
      this.mapHeight = 1;
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

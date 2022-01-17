using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{

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

  public void GenerateMap()
  {
    float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

    TerrainDisplay display = FindObjectOfType<TerrainDisplay>();
    display.DrawNoiseMap(noiseMap);
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

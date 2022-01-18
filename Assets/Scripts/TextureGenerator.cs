using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator
{

  public static Texture2D TextureFromColorMap(Color[,] colorMap)
  {
    int width = colorMap.GetLength(0);
    int height = colorMap.GetLength(1);

    Texture2D texture = new Texture2D(width, height);
    texture.filterMode = FilterMode.Point;
    texture.wrapMode = TextureWrapMode.Clamp;

    Color[] colorMapFlat = new Color[width * height];
    for (int x = 0; x < width; x++)
    {
      for (int y = 0; y < height; y++)
      {
        colorMapFlat[x + y * width] = colorMap[x, y]; //Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
      }
    }
    texture.SetPixels(colorMapFlat);
    texture.Apply();

    return texture;
  }

  public static Texture2D TextureFromHeightMap(float[,] heightMap)
  {
    int width = heightMap.GetLength(0);
    int height = heightMap.GetLength(1);

    Texture2D texture = new Texture2D(width, height);

    Color[] heightMapFlat = new Color[width * height];
    for (int x = 0; x < width; x++)
    {
      for (int y = 0; y < height; y++)
      {
        heightMapFlat[x + y * width] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
      }
    }
    texture.SetPixels(heightMapFlat);
    texture.Apply();

    return texture;
  }
}

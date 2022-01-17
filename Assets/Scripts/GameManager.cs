using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  public GameObject terrainContainer;

  public GameObject groundPrefab;

  public int landWidth;
  public int landLength;
  public int offset;

  void Start()
  {
    this.terrainContainer = GameObject.Find("Terrain");
    this.GenerateTerrain();
  }

  public void ClearTerrain()
  {
    foreach (Transform cube in this.terrainContainer.GetComponentInChildren<Transform>())
    {
      Destroy(cube.gameObject);
    }
  }

  public void GenerateTerrain()
  {
    for (int x = 0; x < landWidth; x++)
    {
      for (int z = 0; z < landLength; z++)
      {
        Vector3 cubePosition = new Vector3(x, CalculateHeight(x, z), z);
        GameObject groundCube = Instantiate(this.groundPrefab, cubePosition, Quaternion.identity);
        groundCube.transform.SetParent(this.terrainContainer.transform);
      }
    }
  }

  float CalculateHeight(int x, int z)
  {
    float xCoord = (float)x / this.landWidth + this.offset; // * 2f;
    float yCoord = (float)z / this.landLength + this.offset; // * 2f;

    float generated = Mathf.PerlinNoise(xCoord, yCoord);
    Debug.Log(generated);

    return Mathf.Floor(generated * 100);
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndessTerrain : MonoBehaviour
{
  public const float maxViewDist = 450;
  public Transform viewer;

  public static Vector2 viewerPosition;
  int chunkSize;
  int chunksVisibleInViewDist;

  Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
  List<TerrainChunk> terrainCunksVisibleLastUpdate = new List<TerrainChunk>();

  void Start()
  {
    this.chunkSize = TerrainGenerator.mapChunkSize - 1;
    this.chunksVisibleInViewDist = Mathf.RoundToInt(maxViewDist / chunkSize);
  }

  void Update()
  {
    viewerPosition = new Vector2(viewer.position.x, viewer.position.z);
    this.UpdateVisibleChunks();
  }

  void UpdateVisibleChunks()
  {

    for (int i = 0; i < this.terrainCunksVisibleLastUpdate.Count; i++)
    {
      this.terrainCunksVisibleLastUpdate[i].SetVisible(false);
    }
    this.terrainCunksVisibleLastUpdate.Clear();

    int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / this.chunkSize);
    int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / this.chunkSize);

    for (int zOffset = -this.chunksVisibleInViewDist; zOffset <= this.chunksVisibleInViewDist; zOffset++)
    {
      for (int xOffset = -this.chunksVisibleInViewDist; xOffset < this.chunksVisibleInViewDist; xOffset++)
      {
        Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + zOffset);

        if (terrainChunkDictionary.ContainsKey(viewedChunkCoord))
        {
          terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk();
          if (terrainChunkDictionary[viewedChunkCoord].IsVisible())
          {
            this.terrainCunksVisibleLastUpdate.Add(terrainChunkDictionary[viewedChunkCoord]);
          }
        }
        else
        {
          terrainChunkDictionary.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, chunkSize, transform));
        }
      }
    }
  }

  public class TerrainChunk
  {
    GameObject meshObject;
    Vector2 position;
    Bounds bounds;

    public TerrainChunk(Vector2 coord, int size, Transform parent)
    {
      position = coord * size;
      bounds = new Bounds(position, Vector2.one * size);
      Vector3 positionV3 = new Vector3(position.x, 0, position.y);

      meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
      meshObject.transform.position = positionV3;
      meshObject.transform.localScale = Vector3.one * size / 10f;
      meshObject.transform.parent = parent;
      SetVisible(false);
    }

    public void UpdateTerrainChunk()
    {
      float viewerDistFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
      bool visible = viewerDistFromNearestEdge <= maxViewDist;
      this.SetVisible(visible);
    }

    public void SetVisible(bool visible)
    {
      meshObject.SetActive(visible);
    }

    public bool IsVisible()
    {
      return this.meshObject.activeSelf;
    }
  }
}

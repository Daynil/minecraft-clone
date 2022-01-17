using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainGenerator))]
public class TerrainGeneratorEditor : Editor
{
  public override void OnInspectorGUI()
  {
    TerrainGenerator gen = (TerrainGenerator)target;

    if (DrawDefaultInspector())
    {
      if (gen.autoUpdate)
      {
        gen.GenerateMap();
      }
    }


    if (GUILayout.Button("Regenerate Level"))
    {
      gen.GenerateMap();
    }
  }
}
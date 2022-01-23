using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshGenTest))]
public class MeshGenTestEditor : Editor
{
  public override void OnInspectorGUI()
  {
    MeshGenTest gen = (MeshGenTest)target;

    // if (DrawDefaultInspector())
    // {
    //   if (gen.autoUpdate)
    //   {

    //     gen.GenerateMap();
    //   }
    // }

    if (GUILayout.Button("Gen Mesh"))
    {
      gen.CreateMesh();
    }
  }
}
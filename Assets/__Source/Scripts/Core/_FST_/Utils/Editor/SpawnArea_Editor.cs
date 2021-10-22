using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpawnArea)), CanEditMultipleObjects()]
public class SpawnArea_Editor : Editor
{
    SpawnArea tgt;
    private void OnEnable()
    {
        tgt = target as SpawnArea;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Spawn at Points"))
        {
            tgt.Spawn();
        }
    }
}

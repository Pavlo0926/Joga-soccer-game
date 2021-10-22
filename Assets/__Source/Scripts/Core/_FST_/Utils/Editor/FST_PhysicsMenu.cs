using UnityEditor;
using UnityEngine;
public class FST_PhysicsMenu
{
    [MenuItem("FST Tools/Select Physics Data")]
    public static void SelectPhysData()
    {
        Object o = Resources.Load("FST_PhysicsData");
        Selection.activeObject = o;
        EditorGUIUtility.PingObject(o);
    }
}

using UnityEditor;

namespace DigitalRuby.RainMaker
{
    public class RainMakerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }
    }

    [CustomEditor(typeof(RainScript))]
    public class RainMakerEditor3D : RainMakerEditor
    {
    }

    [CustomEditor(typeof(RainScript2D))]
    public class RainMakerEditor2D : RainMakerEditor
    {
    }
}
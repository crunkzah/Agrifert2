using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VerticalLayoutCustom))]
public class VerticalLayoutCustomEditor : Editor
{
    public override void  OnInspectorGUI()
    {
        base.OnInspectorGUI();
        VerticalLayoutCustom vlc = (VerticalLayoutCustom)target;
        if(GUILayout.Button("Resize TMPros"))
        {
            vlc.ResizeTextContainers();
        }
        

        if(GUILayout.Button("Align elements"))
        {
            vlc.AlignElements();
        }

        if(GUILayout.Button("Resize container"))
        {
            vlc.ResizeContainer();
        }
    }
}

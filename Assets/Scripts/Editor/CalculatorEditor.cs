using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Calculator))]
public class CalculatorEditor : Editor
{
    public override void  OnInspectorGUI()
    {
        base.OnInspectorGUI();
        // Calculator t = (Calculator)target;
        // if(GUILayout.Button("Make standard backup"))
        // {
        //     t.MakeStandardsBackup();
        //     EditorUtility.SetDirty(t);
        //     EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        // }
        
    }
}

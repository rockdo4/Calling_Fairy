using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InvUI))]
public class InvUIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        InvUI invUI = (InvUI)target;

        // Mode 필드 그리기
        invUI.mode = (InvUI.Mode)EditorGUILayout.EnumPopup("Mode", invUI.mode);
        invUI.parentWindow = (UI)EditorGUILayout.ObjectField("Parent Window", invUI.parentWindow, typeof(UI), true);
        invUI.childrenWindow = (UI)EditorGUILayout.ObjectField("Children Window", invUI.childrenWindow, typeof(UI), true);

        // Mode에 따라 보여주는 필드 변경
        switch (invUI.mode)
        {
            case InvUI.Mode.GrowthUI:
                invUI.fairyGrowthSys = (FairyGrowthSystem)EditorGUILayout.ObjectField("Fairy Growth System", invUI.fairyGrowthSys, typeof(FairyGrowthSystem), true);
                break;
            case InvUI.Mode.FormationUI:
                
                break;
        }

        invUI.iconPrefab = (GameObject)EditorGUILayout.ObjectField("Icon Prefab", invUI.iconPrefab, typeof(GameObject), true);
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("contents"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("seters"), true);

        serializedObject.ApplyModifiedProperties();
    }
}
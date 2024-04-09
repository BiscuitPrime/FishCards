using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Script that will be used to create the Opponent creator window, to facilitate their creation (modular design and all that).
/// </summary>
public class OpponentCreatorWindow : EditorWindow
{
    #region SYSTEM_FUNCTIONS
    //------------------------------------------------
    //SYSTEM FUNCTIONS
    //function that actually creates access to the editor :
    [MenuItem("Custom Tools/NPC Creator")]
    public static void DisplayWindow()
    {
        GetWindow<OpponentCreatorWindow>("OPPONENT CREATOR WINDOW");
    }

    //function that keeps updating the editor window :
    private void OnInspectorUpdate()
    {
        Repaint();
    }
    //------------------------------------------------
    #endregion

    Editor editor;
    private Vector2 scrollPos;

    private void OnGUI()
    {
        GUILayout.Label("Opponent Creator Window", EditorStyles.boldLabel); //label of the window
        //scrollPos = GUILayout.BeginScrollView(scrollPos, false, true);
    }
}

public class OpponentCreatorWindowDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();
        //var statesList = serializedObject.FindProperty("AvailableStatesList");
        //EditorGUILayout.PropertyField(statesList, new GUIContent("Available States List"), true);
        EditorGUILayout.EndHorizontal();
    }
}

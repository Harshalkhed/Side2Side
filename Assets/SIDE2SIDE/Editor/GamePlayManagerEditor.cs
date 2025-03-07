using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GamePlayManager))]
public class GamePlayManagerEditor : Editor
{
    
    //This is a script for custom inspector for GamePlayManager script
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GamePlayManager gamePlayManager = (GamePlayManager)target;

        //Button in inspector to reset all checkpoints from PlayerPrefs
        GUIContent buttonContent = new GUIContent("Reset Checkpoints", "Clicking this will reset all saved checkpoints.");

        if (GUILayout.Button(buttonContent))
        {
            gamePlayManager.ResetCheckpoints();
            
        }
    }
}

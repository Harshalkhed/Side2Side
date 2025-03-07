using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GamePlayManager))]
public class GamePlayManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GamePlayManager gamePlayManager = (GamePlayManager)target;

        if (GUILayout.Button("Reset Checkpoints"))
        {
            gamePlayManager.ResetCheckpoints();
            
        }
    }
}

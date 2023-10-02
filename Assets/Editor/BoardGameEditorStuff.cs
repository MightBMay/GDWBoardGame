using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class BoardGameEditorStuff
{
    [MenuItem("Assets/Create/My Scriptable Object")]
    public static void CreateMyAsset()
    {
        Card asset = ScriptableObject.CreateInstance<Card>();

        AssetDatabase.CreateAsset(asset, "Assets/NewScripableObject.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}

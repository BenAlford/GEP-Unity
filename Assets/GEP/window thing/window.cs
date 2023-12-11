using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class window : EditorWindow
{
    [MenuItem("Tools/Test")]
    public static void ShowWindow()
    {
        var window = GetWindow<window>();
        window.titleContent = new GUIContent("Editor");
        window.minSize = new Vector2(800, 600);
    }

    private void OnEnable()
    {
        VisualTreeAsset original = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/GEP/window thing/window.uxml");
        TemplateContainer treeAsset = original.CloneTree();
        rootVisualElement.Add(treeAsset);
    }
}

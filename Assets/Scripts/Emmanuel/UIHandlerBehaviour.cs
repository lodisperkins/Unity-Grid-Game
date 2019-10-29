using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
public class UIHandlerBehaviour : MonoBehaviour
{
    [FormerlySerializedAs("canvas")] [SerializeField] private GameObject parentGameObject;
    
    [SerializeField]
    internal List<GameObject> parentGameObjectChildren;

    [SerializeField] internal List<string> tags;

    internal void RefreshList(List<string> includeObjectsWithTags = null)
    {
        parentGameObjectChildren = new List<GameObject>();

        if (tags.Count > 0)
        {
            parentGameObjectChildren = GameObjectBehaviour.GetAllChildrenWithTags(parentGameObject, tags.ToArray());
        }
        else
        {
            parentGameObjectChildren = GameObjectBehaviour.GetAllChildren(parentGameObject);
        }

        //foreach (Transform childTransform in parentGameObject.transform)
        //{
//            if (includeObjectsWithTags.Contains(childTransform.tag))
//            {
//                parentGameObjectChildren.Add(childTransform.gameObject);
//            }
        //}
    }//
}
#if UNITY_EDITOR
[CustomEditor(typeof(UIHandlerBehaviour))]
public class UIHandlerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var uiHandler = (UIHandlerBehaviour) target;
        DrawDefaultInspector();

        if(GUILayout.Button("Refresh List"))
        {
            uiHandler.RefreshList(uiHandler.tags);
        }
    }
    
    public void ShowArrayPropertyUsingParameterName(SerializedProperty list, string specifiedName = "Element")
    {
        EditorGUILayout.PropertyField(list);

        EditorGUI.indentLevel += 1;
        for (int i = 0; i < list.arraySize; i++)
        {
            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i),
                new GUIContent(specifiedName + " " + i));
        }
    }
    
    public void ShowArrayPropertyUsingGameObjectNames(SerializedProperty list)
    {
        EditorGUILayout.PropertyField(list);

        EditorGUI.indentLevel += 1;
        for (int i = 0; i < list.arraySize; i++)
        {
            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i),
                new GUIContent(list.GetArrayElementAtIndex(i).name));
        }
    }

}
#endif


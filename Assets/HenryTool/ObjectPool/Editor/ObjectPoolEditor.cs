using UnityEngine;
using System.Collections;
using UnityEditor;
using HenryTool;


[CustomEditor(typeof(ObjectPool))]
[CanEditMultipleObjects]
public class ObjectPoolEditor : Editor
{
    ObjectPool thePool;

    string poolDescription;

    SerializedProperty objectPrefabEditor;
    SerializedProperty maxObjCount;
    SerializedProperty usePoolRootObject;

    void OnEnable()
    {
        thePool = (ObjectPool)target;
        // Setup the SerializedProperties
        objectPrefabEditor = serializedObject.FindProperty("objectPrefab");
        maxObjCount = serializedObject.FindProperty("maxObjCount");
        usePoolRootObject = serializedObject.FindProperty("usePoolRootObject");

        EditorUtility.SetDirty(target);
    }





    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //thePool = (ObjectPool)target;
        MonoScript script;

        script = MonoScript.FromScriptableObject((ObjectPool)target);
        script = EditorGUILayout.ObjectField("Script:", script, typeof(MonoScript), false) as MonoScript;


        EditorGUILayout.PropertyField(objectPrefabEditor);
        if (thePool.objectPrefab == null) {
            EditorGUILayout.LabelField("");
            EditorGUILayout.LabelField(" === The PoolObject Prefab is NULL! ===");
            EditorGUILayout.LabelField("");
        }
        else {
            EditorGUILayout.PropertyField(maxObjCount);
            
            if (thePool.poolQueue != null) {
                if (Application.isPlaying) {
                    EditorGUILayout.IntField("Objects in Pool: ", thePool.poolQueue.Count);

                    int cnt = 0;
                    foreach (PoolObject po in thePool.poolQueue) {
                        EditorGUILayout.TextField("Pool Object:", po.name + cnt.ToString());
                        cnt++;
                    }
                }
                else {
                    if (thePool.poolQueue.Count > 0) {
                        thePool.ClearPool();

                    }

                }
            }
            else {
                //EditorGUILayout.LabelField("Number of Objects: N/A");
            }

        }


        //EditorGUILayout.LabelField("Description:");
        //poolDescription = EditorGUILayout.TextArea(poolDescription);


        serializedObject.ApplyModifiedProperties();

        EditorUtility.SetDirty(target);

    }
}

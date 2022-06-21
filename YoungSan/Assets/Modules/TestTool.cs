using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TestTool : MonoBehaviour
{
#if UNITY_EDITOR
    public static Dictionary<string, object> objects = new Dictionary<string, object>();
    public static List<GameObject> resources = new List<GameObject>();

    [MenuItem("TestTool/Yeah")]
    public static void MapEditor_ResourceSelector_Show()
    {
        EditorWindow.GetWindow<Yeah>().titleContent = new GUIContent("Yeah");
    }

#endif
}

#if UNITY_EDITOR
public class Yeah : EditorWindow
{
    GameObject go;
    DefaultAsset asset;

    void OnEnable()
    {

    }

    private void OnGUI()
    {
        go = EditorGUILayout.ObjectField(go, typeof(GameObject), true) as GameObject;
        asset = EditorGUILayout.ObjectField(asset, typeof(DefaultAsset), false) as DefaultAsset;
        if (asset != null) EditorGUILayout.LabelField(AssetDatabase.GetAssetPath(asset));

        if (GUILayout.Button("wa sibal"))
        {
            Hashtable table = new Hashtable();
            Hashtable prefabs = new Hashtable();
            string path = AssetDatabase.GetAssetPath(asset);

            if (go == null) return;

            for (int i = 0; i < go.transform.childCount; i++)
            {
                GameObject o = go.transform.GetChild(i).gameObject;
                try
                {
                    PrefabUtility.UnpackPrefabInstance(o, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                }
                catch { }

                Sprite sprite = o.GetComponent<SpriteRenderer>().sprite;

                if (table.ContainsKey(sprite))
                {
                    (table[sprite] as List<Vector3>).Add(o.transform.position);
                }
                else
                {
                    table.Add(sprite, new List<Vector3>());
                    (table[sprite] as List<Vector3>).Add(o.transform.position);
                    o.name = sprite.name;
                    o.transform.position = Vector3.zero;

                    bool b = false;
                    prefabs[sprite] = PrefabUtility.SaveAsPrefabAsset(o, path + $"/{o.name}.prefab", out b);
                }

                DestroyImmediate(o);
                i--;
            }

            foreach (var sprite in table.Keys)
            {
                var list = (table[sprite] as List<Vector3>);
                GameObject clone = prefabs[sprite] as GameObject;

                foreach (var item in list)
                {
                    GameObject co = PrefabUtility.InstantiatePrefab(clone) as GameObject;
                    PrefabUtility.RevertPrefabInstance(co, InteractionMode.UserAction);
                    co.transform.position = item;
                    co.transform.parent = go.transform;
                }
            }
        }
    }

}
#endif

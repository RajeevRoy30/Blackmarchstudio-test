using UnityEngine;
using UnityEditor;

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObstacleData))]
public class ObstacleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ObstacleData data = (ObstacleData)target;

        if (data == null)
            return;

        // Ensure the grid is initialized
        if (data.GetObstacleList() == null || data.GetObstacleList().Count == 0)
        {
            if (GUILayout.Button("Initialize Grid"))
            {
                data.Initialize();
                EditorUtility.SetDirty(data);
                AssetDatabase.SaveAssets();
            }
            return;
        }

        bool dataChanged = false;

        for (int y = 0; y < 10; y++)
        {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < 10; x++)
            {
                bool newValue = GUILayout.Toggle(data.GetObstacle(x, y), "");
                if (newValue != data.GetObstacle(x, y))
                {
                    data.SetObstacle(x, y, newValue);
                    dataChanged = true;
                }
            }
            GUILayout.EndHorizontal();
        }

        if (dataChanged)
        {
            EditorUtility.SetDirty(data);
            AssetDatabase.SaveAssets();
            data.SaveData();
        }

        if (GUILayout.Button("Load Saved Data"))
        {
            data.LoadData();
            EditorUtility.SetDirty(data);
            AssetDatabase.SaveAssets();
        }
    }
}
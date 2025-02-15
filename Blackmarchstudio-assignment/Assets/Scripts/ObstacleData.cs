using UnityEngine;
using System.Collections.Generic;
using System.IO;


[CreateAssetMenu(fileName = "ObstacleData", menuName = "Game/ObstacleData")]
public class ObstacleData : ScriptableObject
{
    private static string savePath => Application.persistentDataPath + "/obstacleData.json";

    [SerializeField]
    private List<ObstacleGrid> obstacleList = new List<ObstacleGrid>();

    public bool GetObstacle(int x, int y)
    {
        if (obstacleList == null || obstacleList.Count <= x || obstacleList[x].row.Count <= y)
        {
            Debug.LogError("Obstacle grid is not initialized or indices are out of range.");
            return false; // Default value
        }
        return obstacleList[x].row[y];
    }

    public void SetObstacle(int x, int y, bool value)
    {
        if (obstacleList == null || obstacleList.Count <= x || obstacleList[x].row.Count <= y)
        {
            Debug.LogError("Obstacle grid is not initialized or indices are out of range.");
            return;
        }
        obstacleList[x].row[y] = value;
    }

    public void Initialize()
    {
        obstacleList = new List<ObstacleGrid>();
        for (int x = 0; x < 10; x++)
        {
            ObstacleGrid row = new ObstacleGrid();
            row.row = new List<bool>(new bool[10]); // Initializes with false
            obstacleList.Add(row);
        }
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(new SerializationWrapper(obstacleList));
        File.WriteAllText(savePath, json);
        Debug.Log("Obstacle Data Saved: " + savePath);
    }

    public void LoadData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SerializationWrapper wrapper = JsonUtility.FromJson<SerializationWrapper>(json);
            obstacleList = wrapper.grid;
            Debug.Log("Obstacle Data Loaded");
        }
    }

    private void OnEnable()
    {
        if (obstacleList == null || obstacleList.Count == 0)
        {
            Initialize(); // Ensures it's initialized on enable
        }
        LoadData();
    }

    private void OnDisable()
    {
        if (!Application.isPlaying)
        {
            SaveData();
        }
    }

    public List<ObstacleGrid> GetObstacleList()
    {
        return obstacleList;
    }

    [System.Serializable]
    private class SerializationWrapper
    {
        public List<ObstacleGrid> grid;

        public SerializationWrapper(List<ObstacleGrid> grid)
        {
            this.grid = grid;
        }
    }
    [System.Serializable]
    public class ObstacleGrid
    {
        public List<bool> row = new List<bool>();
    }
}
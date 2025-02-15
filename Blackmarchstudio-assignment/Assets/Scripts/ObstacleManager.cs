using UnityEngine;
using System.Collections.Generic;

public class ObstacleManager : MonoBehaviour
{
    public ObstacleData obstacleData;
    public GameObject obstaclePrefab;
    private List<GameObject> obstacles = new List<GameObject>();

    void Start()
    {
        GenerateObstacles();
    }

    void GenerateObstacles()
    {
        // Clear old obstacles before generating new ones
        foreach (GameObject obj in obstacles)
        {
            Destroy(obj);
        }
        obstacles.Clear();

        Debug.Log("Generating Obstacles");

        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                if (obstacleData.GetObstacle(x, y))  // Use new GetObstacle() method
                {
                    Vector3 position = new Vector3(x * 1.2f, 0.5f, y * 1.2f); // Adjust Y if needed
                    GameObject obj = Instantiate(obstaclePrefab, position, Quaternion.identity);
                    obstacles.Add(obj);
                }
            }
        }
    }

    public void RefreshObstacles()
    {
        GenerateObstacles(); // Call this method when updating obstacles dynamically
    }
}

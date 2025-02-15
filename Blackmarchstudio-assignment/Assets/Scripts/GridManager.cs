using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridSize = 10;
    public float tileSize = 1.0f;
    public float gapSize = 0.1f;
    public GameObject tilePrefab;
    public ObstacleData obstacleData;
    public float yOffset = -0.7f;

    private GameObject[,] grid;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        grid = new GameObject[gridSize, gridSize];
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector3 position = new Vector3(
                    x * (tileSize + gapSize),
                    yOffset, // Y-axis position grid
                    y * (tileSize + gapSize)
                );

                grid[x, y] = Instantiate(tilePrefab, position, Quaternion.identity, transform);

                //obstacles
                if (obstacleData.GetObstacle(x, y))
                {
                    grid[x, y].GetComponent<Renderer>().material.color = Color.red;
                }
            }
        }
    }

    public bool IsWalkable(int x, int y)
    {
        if (x >= 0 && x < gridSize && y >= 0 && y < gridSize)
        {
            return !obstacleData.GetObstacle(x, y);
        }
        return false;
    }

    public Vector3 GetTilePosition(Vector2Int gridPosition)
    {
        return new Vector3(
            gridPosition.x * (tileSize + gapSize),
            0, // Y-axis position grid
            gridPosition.y * (tileSize + gapSize)
        );
    }

    public Vector2Int GetTileCoordinates(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / (tileSize + gapSize));
        int y = Mathf.FloorToInt(worldPosition.z / (tileSize + gapSize));
        return new Vector2Int(x, y);
    }
}

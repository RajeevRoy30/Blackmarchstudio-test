using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private GridManager gridManager;

    public Pathfinding(GridManager gridManager)
    {
        this.gridManager = gridManager;
    }

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int target)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        Dictionary<Vector2Int, float> costSoFar = new Dictionary<Vector2Int, float>();
        PriorityQueue<Vector2Int> frontier = new PriorityQueue<Vector2Int>();

        frontier.Enqueue(start, 0);
        cameFrom[start] = start;
        costSoFar[start] = 0;

        while (frontier.Count > 0)
        {
            Vector2Int current = frontier.Dequeue();

            if (current == target)
            {
                break;
            }

            foreach (Vector2Int neighbor in GetNeighbors(current))
            {
                float newCost = costSoFar[current] + 1; // Assuming each step has a cost of 1

                if (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor])
                {
                    costSoFar[neighbor] = newCost;
                    float priority = newCost + Heuristic(neighbor, target);
                    frontier.Enqueue(neighbor, priority);
                    cameFrom[neighbor] = current;
                }
            }
        }

        // Reconstruct path
        Vector2Int currentPath = target;
        while (currentPath != start)
        {
            path.Add(currentPath);
            currentPath = cameFrom[currentPath];
        }
        path.Reverse();

        return path;
    }

    private List<Vector2Int> GetNeighbors(Vector2Int node)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (Vector2Int dir in directions)
        {
            Vector2Int neighbor = node + dir;
            if (gridManager.IsWalkable(neighbor.x, neighbor.y))
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    private float Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y); // distance
    }
}
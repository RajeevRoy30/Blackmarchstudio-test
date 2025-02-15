using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour, IAI
{
    public GridManager gridManager;
    public float moveSpeed = 2.0f;

    private Vector2Int currentPosition;
    private List<Vector2Int> path;
    private bool isMoving = false;
    private Transform player;

    void Start()
    {
        // enemy staarting postion
        currentPosition = new Vector2Int(0, gridManager.gridSize - 1); 
        transform.position = gridManager.GetTilePosition(currentPosition);

        // finding player object through tag
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void UpdateAI()
    {
        if (!isMoving)
        {
            Vector2Int playerPosition = gridManager.GetTileCoordinates(player.position);
            Vector2Int targetPosition = GetAdjacentTile(playerPosition);

            if (targetPosition != currentPosition)
            {
                Pathfinding pathfinding = new Pathfinding(gridManager);
                path = pathfinding.FindPath(currentPosition, targetPosition);

                if (path.Count > 0)
                {
                    StartCoroutine(MoveAlongPath());
                }
            }
        }
    }

    private Vector2Int GetAdjacentTile(Vector2Int playerPosition)
    {
        // Checking
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (Vector2Int dir in directions)
        {
            Vector2Int adjacentTile = playerPosition + dir;
            if (gridManager.IsWalkable(adjacentTile.x, adjacentTile.y))
            {
                return adjacentTile;
            }
        }

        return currentPosition; //staying in current position if there is no place to move through obstacles.
    }

    IEnumerator MoveAlongPath()
    {
        isMoving = true;

        foreach (Vector2Int nextPosition in path)
        {
            Vector3 targetPosition = gridManager.GetTilePosition(nextPosition);
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }
            currentPosition = nextPosition;
        }

        isMoving = false;
    }
}
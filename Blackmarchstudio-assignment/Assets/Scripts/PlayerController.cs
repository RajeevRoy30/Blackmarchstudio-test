using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public GridManager gridManager;
    public float moveSpeed = 2.0f;

    private Vector2Int currentPosition;
    private List<Vector2Int> path;
    private bool isMoving = false;
    private float yOffset = 1.0f; 
    public TextMeshProUGUI text;
    void Start()
    {
        // player starting position
        currentPosition = new Vector2Int(0, 0); // Starting position
        transform.position = gridManager.GetTilePosition(currentPosition) + Vector3.up * yOffset;
    }
    
    void Update()
    {
        if (!isMoving && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector2Int targetPosition = gridManager.GetTileCoordinates(hit.point);
                if (gridManager.IsWalkable(targetPosition.x, targetPosition.y))
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
    }

    IEnumerator MoveAlongPath()
    {
        isMoving = true;

        foreach (Vector2Int nextPosition in path)
        {
            Vector3 targetPosition = gridManager.GetTilePosition(nextPosition) + Vector3.up * yOffset;
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }
            currentPosition = nextPosition;
        }

        isMoving = false;

        // Telling enemy that player has moved
        EnemyAI enemy = FindObjectOfType<EnemyAI>();
        if (enemy != null)
        {
            enemy.UpdateAI();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            this.enabled = false;
            gameObject.SetActive(false);
            text.SetText("YOU ARE DEAD");
        }
    }
   
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    private const string PATH_TAG = "Path";

    [SerializeField] [Range(0f,5f)] private float speed = 1f;

    private List<Node> path = new List<Node>();
    private Enemy enemy;
    private GridManager gridManager;
    private Pathfinder pathfinder;

    private void Awake() {
        enemy = GetComponent<Enemy>();
        gridManager = FindObjectOfType<GridManager>();
        pathfinder = FindObjectOfType<Pathfinder>();
    }

    private void OnEnable() {
        ReturnToStart();
        RecalculatePath(true);
    }

    private void RecalculatePath(bool resetPath) {
        Vector2Int coordinates = new Vector2Int();

        if (resetPath) {
            coordinates = pathfinder.StartCoordinates;
        } else {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        }
        
        StopAllCoroutines();
        path.Clear();
        path = pathfinder.GetNewPath(coordinates);
        StartCoroutine(FollowPath());
    }

    private void ReturnToStart() {
        transform.position = gridManager.GetPositionFromCoordinates(pathfinder.StartCoordinates);
    }

    private void FinishPath() {
        enemy.StealGold();
        gameObject.SetActive(false);
    }

    private IEnumerator FollowPath() {
        for (int i = 1; i < path.Count; i++) {
            Vector3 startingPosition = transform.position;
            Vector3 endPosition = gridManager.GetPositionFromCoordinates(path[i].coordinates);
            float travelPercent = 0f;

            transform.LookAt(endPosition);

            while (travelPercent < 1f) {
                travelPercent += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(startingPosition, endPosition, travelPercent);
                yield return new WaitForEndOfFrame();
            }
        }

        FinishPath();
    }
}
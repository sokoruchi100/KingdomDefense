using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    private const string PATH_TAG = "Path";

    [SerializeField] private List<Waypoint> path = new List<Waypoint>();
    [SerializeField] [Range(0f,5f)] private float speed = 1f;

    private Enemy enemy;

    private void OnEnable() {
        FindPath();
        ReturnToStart();
        StartCoroutine(FollowPath());
    }

    private void Start() {
        enemy = GetComponent<Enemy>();
    }

    private void FindPath() {
        path.Clear();

        GameObject parent = GameObject.FindGameObjectWithTag(PATH_TAG);

        foreach (Transform child in parent.transform) {
            Waypoint waypoint = child.GetComponent<Waypoint>();
            if (waypoint != null) {
                path.Add(waypoint);
            }
        }
    }

    private void ReturnToStart() {
        transform.position = path[0].transform.position;
    }

    private void FinishPath() {
        enemy.StealGold();
        gameObject.SetActive(false);
    }

    private IEnumerator FollowPath() {
        foreach (Waypoint waypoint in path) {
            Vector3 startingPosition = transform.position;
            Vector3 endPosition = waypoint.transform.position;
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
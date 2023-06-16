using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private List<Waypoint> path = new List<Waypoint>();
    [SerializeField] private float waitTime = 1f;

    private void Start() {
        StartCoroutine(FollowPath());
    }

    private IEnumerator FollowPath() {
        foreach (Waypoint waypoint in path) {
            transform.position = waypoint.transform.position;

            yield return new WaitForSeconds(waitTime);
        }
    }
}
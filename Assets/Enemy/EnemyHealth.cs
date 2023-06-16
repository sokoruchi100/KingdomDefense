using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHitPoints = 5;
    private int currentHitPoints = 0;

    private void Start() {
        currentHitPoints = maxHitPoints;
    }

    private void OnParticleCollision(GameObject other) {
        ProcessHit();
    }

    private void ProcessHit() {
        currentHitPoints--;

        if (currentHitPoints <= 0) {
            Destroy(gameObject);
        }
    }
}

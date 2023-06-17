using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHitPoints = 5;

    [Tooltip("Adds amount to maxHitPoints when enemy dies")]
    [SerializeField] private int difficultyRamp = 1;
    
    private int currentHitPoints = 0;

    private Enemy enemy;

    private void OnEnable() {
        currentHitPoints = maxHitPoints;
    }

    private void Start() {
        enemy = GetComponent<Enemy>();
    }

    private void OnParticleCollision(GameObject other) {
        ProcessHit();
    }

    private void ProcessHit() {
        currentHitPoints--;

        if (currentHitPoints <= 0) {
            enemy.RewardGold();
            maxHitPoints += difficultyRamp;
            gameObject.SetActive(false);
        }
    }
}

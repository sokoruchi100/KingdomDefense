using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHitPoints = 5;

    [Tooltip("Adds amount to maxHitPoints when enemy dies")]
    [SerializeField] private int difficultyRamp = 1;

    [SerializeField] private Image healthbar;
    
    private int currentHitPoints = 0;

    private Enemy enemy;

    private void OnEnable() {
        currentHitPoints = maxHitPoints;
        healthbar.fillAmount = 1f;
    }

    private void Start() {
        enemy = GetComponent<Enemy>();
    }

    private void OnParticleCollision(GameObject other) {
        ProcessHit();
    }

    private void ProcessHit() {
        currentHitPoints--;
        healthbar.fillAmount = (float) currentHitPoints / maxHitPoints;

        if (currentHitPoints <= 0) {
            enemy.RewardGold();
            maxHitPoints += difficultyRamp;
            gameObject.SetActive(false);
        }
    }
}
